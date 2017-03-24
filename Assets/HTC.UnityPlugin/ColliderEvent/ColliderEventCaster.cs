//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.ColliderEvent
{
    public interface IColliderEventCaster
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        MonoBehaviour monoBehaviour { get; }

        Rigidbody rigid { get; }
        ColliderHoverEventData HoverEventData { get; }
        ReadOnlyCollection<ColliderButtonEventData> ButtonEventDataList { get; }
        ReadOnlyCollection<Collider> HoveredColliders { get; }
        ReadOnlyCollection<GameObject> HoveredObjects { get; }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class ColliderEventCaster : MonoBehaviour, IColliderEventCaster
    {
        private static HashSet<int> s_gos = new HashSet<int>();

        private bool isUpdating;
        private bool isDisabling;

        private IndexedTable<Collider, int> hoveredColliderCount = new IndexedTable<Collider, int>();
        private IndexedSet<GameObject> hoveredObjects = new IndexedSet<GameObject>();

        private List<Collider> updatingColliders = new List<Collider>();
        private List<GameObject> updatingObjects = new List<GameObject>();

        private Rigidbody m_rigid;
        private ColliderHoverEventData hoverEventData;
        private ReadOnlyCollection<Collider> hoveredCollidersReadOnly;
        private ReadOnlyCollection<GameObject> hoveredObjectsReadOnly;

        private ReadOnlyCollection<ColliderButtonEventData> buttonEventDataListReadOnly;
        private ReadOnlyCollection<ColliderAxisEventData> axisEventDataListReadOnly;

        protected readonly List<ColliderButtonEventData> buttonEventDataList = new List<ColliderButtonEventData>();
        protected readonly List<ColliderAxisEventData> axisEventDataList = new List<ColliderAxisEventData>();

        public MonoBehaviour monoBehaviour
        {
            get
            {
                return this;
            }
        }

        public Rigidbody rigid
        {
            get { return m_rigid ?? (m_rigid = GetComponent<Rigidbody>()); }
        }

        public ColliderHoverEventData HoverEventData
        {
            get { return hoverEventData ?? (hoverEventData = new ColliderHoverEventData(this)); }
        }

        public ReadOnlyCollection<ColliderButtonEventData> ButtonEventDataList
        {
            get { return buttonEventDataListReadOnly ?? (buttonEventDataListReadOnly = buttonEventDataList.AsReadOnly()); }
        }

        public ReadOnlyCollection<ColliderAxisEventData> AxisEventDataList
        {
            get { return axisEventDataListReadOnly ?? (axisEventDataListReadOnly = axisEventDataList.AsReadOnly()); }
        }

        public ReadOnlyCollection<Collider> HoveredColliders
        {
            get { return hoveredCollidersReadOnly ?? (hoveredCollidersReadOnly = hoveredColliderCount.AsReadOnly()); }
        }

        public ReadOnlyCollection<GameObject> HoveredObjects
        {
            get { return hoveredObjectsReadOnly ?? (hoveredObjectsReadOnly = hoveredObjects.AsReadOnly()); }
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (hoveredColliderCount.ContainsKey(collider))
            {
                ++hoveredColliderCount[collider];
            }
            else
            {
                hoveredColliderCount.Add(collider, 1);
            }
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            int triggerCount;
            if (hoveredColliderCount.TryGetValue(collider, out triggerCount))
            {
                if (triggerCount <= 1)
                {
                    hoveredColliderCount.Remove(collider);
                }
                else
                {
                    hoveredColliderCount[collider] = triggerCount - 1;
                }
            }
        }

        private bool RemoveDestroiedCollider(KeyValuePair<Collider, int> item)
        {
            // remove destroied colliders
            return item.Key == null;
        }

        protected virtual void FixedUpdate()
        {
            // fixed dragging
            for (int i = 0, imax = buttonEventDataList.Count; i < imax; ++i)
            {
                var eventData = buttonEventDataList[i];

                if (!eventData.isPressed) { continue; }

                for (int j = eventData.draggingHandlers.Count - 1; j >= 0; --j)
                {
                    ExecuteEvents.Execute(eventData.draggingHandlers[j], eventData, ExecuteColliderEvents.DragFixedUpdateHandler);
                }
            }

            if (isDisabling)
            {
                CleanUp();
            }
        }

        protected virtual void Update()
        {
            isUpdating = true;

            // need remove because OnTriggerExit doesn't automatically called when collider destroied
            hoveredColliderCount.RemoveAll(RemoveDestroiedCollider);

            // process enter
            var hoveredObjectsPrev = hoveredObjects;
            hoveredObjects = IndexedSetPool<GameObject>.Get();

            for (int i = hoveredColliderCount.Count - 1; i >= 0; --i)
            {
                var collider = hoveredColliderCount.GetKeyByIndex(i);

                if (!collider.gameObject.activeInHierarchy || !collider.enabled) { continue; }

                for (var tr = collider.transform; !ReferenceEquals(tr, null); tr = tr.parent)
                {
                    var go = tr.gameObject;

                    if (!hoveredObjects.AddUnique(go)) { break; }

                    if (!hoveredObjectsPrev.Remove(go))
                    {
                        updatingObjects.Add(go);
                    }
                }
            }

            for (int i = updatingObjects.Count - 1; i >= 0; --i)
            {
                ExecuteEvents.Execute(updatingObjects[i], HoverEventData, ExecuteColliderEvents.HoverEnterHandler);
            }
            updatingObjects.Clear();

            for (int i = 0; i < hoveredColliderCount.Count; ++i) { updatingColliders.Add(hoveredColliderCount.GetKeyByIndex(i)); }
            for (int i = 0; i < hoveredObjects.Count; ++i) { updatingObjects.Add(hoveredObjects[i]); }

            // process button events
            for (int i = 0, imax = buttonEventDataList.Count; i < imax; ++i)
            {
                var eventData = buttonEventDataList[i];

                // process button press
                if (!eventData.isPressed)
                {
                    if (eventData.GetPress())
                    {
                        ProcessPressDown(eventData);
                        ProcessPressing(eventData);
                    }
                }
                else
                {
                    if (eventData.GetPress())
                    {
                        ProcessPressing(eventData);
                    }
                    else
                    {
                        ProcessPressUp(eventData);
                    }
                }

                // process pressed button enter/exit
                if (eventData.isPressed)
                {
                    var pressEnteredObjectsPrev = eventData.pressEnteredObjects;
                    eventData.pressEnteredObjects = IndexedSetPool<GameObject>.Get();

                    for (int j = updatingObjects.Count - 1; j >= 0; --j)
                    {
                        var go = updatingObjects[j];

                        if (!eventData.pressEnteredObjects.AddUnique(go)) { continue; }

                        if (!pressEnteredObjectsPrev.Remove(go))
                        {
                            ExecuteEvents.Execute(go, eventData, ExecuteColliderEvents.PressEnterHandler);
                        }
                    }

                    for (int j = pressEnteredObjectsPrev.Count - 1; j >= 0; --j)
                    {
                        if (eventData.clickingHandlers.Count > 0)
                        {
                            eventData.clickingHandlers.Remove(pressEnteredObjectsPrev[j]);
                        }

                        ExecuteEvents.Execute(pressEnteredObjectsPrev[j], eventData, ExecuteColliderEvents.PressExitHandler);
                    }

                    IndexedSetPool<GameObject>.Release(pressEnteredObjectsPrev);
                }
                else
                {
                    for (int j = eventData.pressEnteredObjects.Count - 1; j >= 0; --j)
                    {
                        ExecuteEvents.Execute(eventData.pressEnteredObjects[j], eventData, ExecuteColliderEvents.PressExitHandler);
                    }

                    eventData.pressEnteredObjects.Clear();
                }
            }

            // process axis events
            for (int i = 0, imax = axisEventDataList.Count; i < imax; ++i)
            {
                var eventData = axisEventDataList[i];

                if (!eventData.IsValueChangedThisFrame()) { continue; }

                for (int j = updatingColliders.Count - 1; j >= 0; --j)
                {
                    var handler = ExecuteEvents.GetEventHandler<IColliderEventAxisChangedHandler>(updatingColliders[j].gameObject);

                    if (ReferenceEquals(handler, null)) { continue; }

                    if (!s_gos.Add(handler.GetInstanceID())) { continue; }

                    ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.AxisChangedHandler);
                }
            }
            s_gos.Clear();

            // process leave
            // now stayingObjectsPrev left with handlers that are exited
            for (int i = hoveredObjectsPrev.Count - 1; i >= 0; --i)
            {
                ExecuteEvents.Execute(hoveredObjectsPrev[i], HoverEventData, ExecuteColliderEvents.HoverExitHandler);
            }

            IndexedSetPool<GameObject>.Release(hoveredObjectsPrev);
            updatingColliders.Clear();
            updatingObjects.Clear();

            if (isDisabling)
            {
                CleanUp();
            }

            isUpdating = false;
        }

        protected void ProcessPressDown(ColliderButtonEventData eventData)
        {
            eventData.isPressed = true;
            eventData.pressPosition = transform.position;
            eventData.pressRotation = transform.rotation;

            // click start
            for (int i = updatingColliders.Count - 1; i >= 0; --i)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventClickHandler>(updatingColliders[i].gameObject);

                if (ReferenceEquals(handler, null)) { continue; }

                eventData.clickingHandlers.AddUnique(handler);
            }
            // press down
            for (int i = updatingColliders.Count - 1; i >= 0; --i)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventPressDownHandler>(updatingColliders[i].gameObject);

                if (ReferenceEquals(handler, null)) { continue; }

                if (!s_gos.Add(handler.GetInstanceID())) { continue; }

                ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.PressDownHandler);
            }
            s_gos.Clear();
            // drag start
            for (int i = updatingColliders.Count - 1; i >= 0; --i)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventDragStartHandler>(updatingColliders[i].gameObject);

                if (ReferenceEquals(handler, null)) { continue; }

                if (!eventData.draggingHandlers.AddUnique(handler)) { continue; }

                ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.DragStartHandler);
            }
        }

        protected void ProcessPressing(ColliderButtonEventData eventData)
        {
            // dragging
            for (int i = eventData.draggingHandlers.Count - 1; i >= 0; --i)
            {
                ExecuteEvents.Execute(eventData.draggingHandlers[i], eventData, ExecuteColliderEvents.DragUpdateHandler);
            }
        }

        protected void ProcessPressUp(ColliderButtonEventData eventData)
        {
            eventData.isPressed = false;

            // press up
            for (int i = updatingColliders.Count - 1; i >= 0; --i)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventPressUpHandler>(updatingColliders[i].gameObject);

                if (ReferenceEquals(handler, null)) { continue; }

                if (!s_gos.Add(handler.GetInstanceID())) { continue; }

                ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.PressUpHandler);
            }
            s_gos.Clear();
            // drag end
            for (int i = eventData.draggingHandlers.Count - 1; i >= 0; --i)
            {
                ExecuteEvents.Execute(eventData.draggingHandlers[i], eventData, ExecuteColliderEvents.DragEndHandler);
            }
            // drop
            if (eventData.draggingHandlers.Count > 0)
            {
                for (int i = updatingColliders.Count - 1; i >= 0; --i)
                {
                    var handler = ExecuteEvents.GetEventHandler<IColliderEventDropHandler>(updatingColliders[i].gameObject);

                    if (ReferenceEquals(handler, null)) { continue; }

                    if (!s_gos.Add(handler.GetInstanceID())) { continue; }

                    ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.DropHandler);
                }
            }
            s_gos.Clear();
            // click end (execute only if pressDown handler and pressUp handler are the same)
            for (int i = updatingColliders.Count - 1; i >= 0; --i)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventClickHandler>(updatingColliders[i].gameObject);

                if (ReferenceEquals(handler, null)) { continue; }

                if (eventData.clickingHandlers.Remove(handler))
                {
                    ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.ClickHandler);
                }
            }

            eventData.clickingHandlers.Clear();
            eventData.draggingHandlers.Clear();
        }

        protected virtual void OnDisable()
        {
            isDisabling = true;
            if (!isUpdating)
            {
                CleanUp();
            }
        }

        private void CleanUp()
        {
            hoveredColliderCount.RemoveAll(RemoveDestroiedCollider);

            // release all
            for (int i = 0, imax = buttonEventDataList.Count; i < imax; ++i)
            {
                var eventData = buttonEventDataList[i];

                if (eventData.isPressed)
                {
                    ProcessPressUp(eventData);
                }

                for (int j = eventData.pressEnteredObjects.Count - 1; j >= 0; --j)
                {
                    ExecuteEvents.Execute(eventData.pressEnteredObjects[j], eventData, ExecuteColliderEvents.PressExitHandler);
                }

                eventData.clickingHandlers.Clear();
                eventData.draggingHandlers.Clear();
                eventData.pressEnteredObjects.Clear();
            }

            for (int i = hoveredObjects.Count - 1; i >= 0; --i)
            {
                ExecuteEvents.Execute(hoveredObjects[i], hoverEventData, ExecuteColliderEvents.HoverExitHandler);
            }

            hoveredObjects.Clear();
            hoveredColliderCount.Clear();

            isDisabling = false;
        }
    }
}