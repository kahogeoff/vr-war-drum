ViveInputUtility for Unity - v1.5.3
Copyright 2016, HTC Corporation. All rights reserved.


Introduction:

- Vive Input Utility is a tool based on the SteamVR plugin that allows developers to access Vive device status in handy way.

- We also introduce a mouse pointer solution that works in 3D space and is compatible with the Unity Event System.

- By importing this utility, developers can save lots of time in writing redundant code to manage Vive devices.


Requirements:

- The SteamVR plugin 1.2.1 must be installed. This can be found in Unity Asset Store.


Changes for v1.5.3:

* Make compatible with SteamVR plugin 1.2.1
* Fix a bug in ColliderEventCaster that cause crash when disabling event caster and handling events at the same time.
* Change default teleportButton in Teleportable to TeleportButton.Pad instead of TeleportButton.Trigger
* Containers optimize
  - Re-write IndexedTable, should be more efficient
  - Add read-only interface


Changes for v1.5.2:

* Make compatible with SteamVR plugin 1.2.0


Changes for v1.5.1:

* Update guide document
  - Reveal used namespace in some example scripts.
  - Add ready-to-used component list.

* New controllers prefab that handles both hand EventRaycaster, ColliderEventCaster, guidelines and models
  - Hide controllers' models when grabbing or dragging
  - Enable EventRaycaster on pad touched, otherwise enable ColliderEventCaster

* Pointer3D
  - Expose Pointer3DRaycaster at Pointer3DEventData.raycaster, to get Raycaster from eventData easily.
  - Move dragThreshold and clickInterval settings from Pointer3DInputModule to Pointer3DRaycaster.
  - Re-design RaySegmentGenerator. Now RaycastMode setting is replaced by applying ProjectionGenerator & ProjectileGenerator component with Pointer3DRaycaster.
    Add or enable only one generator at a time, or which generator used by the raycaster is unexpected.
    Also customize your own generators by implementing BaseRaySegmentGenerator.

* ColliderEvent
  - Now OnColliderEventClick won't invoke if caster has leaved the pressed object.
  - Fix a bug in ColliderEventCaster that doesn't handle hovered colliders correctly.
  - Fix a bug that ColliderEventCaster doesn't handle event correctly when disable.
  - Add ColliderEventTrigger component, work just like built-in EventTrigger

* Add series of EventData extensions
  - Pointer3DEventData
      Pointer3DRaycaster PointerEventData.GetRaycaster3D()
	                bool PointerEventData.TryGetRaycaster3D(out Pointer3DRaycaster raycaster)
	        TRaycaster3D PointerEventData.GetRaycaster3D<TRaycaster3D>()
	                bool PointerEventData.TryGetRaycaster3D<TRaycaster3D>(out TRaycaster3D raycaster)
  - ColliderEventData
            TEventCaster ColliderEventData.GetEventCaster<TEventCaster>()
	                bool ColliderEventData.TryGetEventCaster<TEventCaster>(out TEventCaster eventCaster)
  - VivePointerEventData
                    bool PointerEventData.IsViveButton(HandRole hand)
                    bool PointerEventData.IsViveButton(ControllerButton button)
                    bool PointerEventData.IsViveButton(HandRole hand, ControllerButton button)
	                bool PointerEventData.TryGetViveButtonEventData(out VivePointerEventData viveEventData)
  - ViveColliderEventData
                    bool ColliderEventData.IsViveButton(HandRole hand)
                    bool ColliderEventData.IsViveButton(ControllerButton button)
                    bool ColliderEventData.IsViveButton(HandRole hand, ControllerButton button)
					bool ColliderEventData.TryGetViveButtonEventData(out ViveColliderButtonEventData viveEventData)
					bool ColliderAxisEventData.IsViveTriggerValue()
					bool ColliderAxisEventData.IsViveTriggerValue(HandRole hand)
					bool ColliderAxisEventData.TryGetViveTriggerValueEventData(out ViveColliderTriggerValueEventData viveEventData)
					bool ColliderAxisEventData.IsVivePadAxis()
					bool ColliderAxisEventData.IsVivePadAxis(HandRole hand)
					bool ColliderAxisEventData.TryGetVivePadAxisEventData(out ViveColliderPadAxisEventData viveEventData)

* Improve BasicGrabbable component, and Draggable(in 3D Drag example) as well
  - Now grabbed object can collide properly into other colliders.
  - Now handles multiple grabbers.
  - Add speed factor parameter to adjast grabbed object following speed.
  - Add afterGrabbed & beforeRelease event handler.

* Add dragging state material in MaterialChanger.

* Fix a bug in Teleportable so that GuideLineDrawer won't draw in wrong position.

* New containers in Utiliy
  - IndexedSet<TKey>                  : container that combinds set and list, order is not preserved, removing complexity is O(1)
  - OrderedIndexedSet<TKey>           : container that combinds set and list, order is preserved, removing complexity is O(N)
  - IndexedTable<TKey, TValue>        : container that combinds dictionary and list, order is not preserved, removing complexity is O(1)
  - OrderedIndexedTable<TKey, TValue> : container that combinds dictionary and list, order is preserved, removing complexity is O(N)


Changes for v1.5.0:

* Add new raycast mode for Pointer3DRaycaster
  - Default : one simple raycast
  - Projection : raycast in a constant distance then raycast toward gravity
  - Projectile : raycast multiple times alone the projectile curve using initial velocity 

* Add ViveInput.GetCurrentRawControllerState and ViveInput.GetPreviousRawControllerState.

* BaseRaycastMethod now registered into Pointer3DRaycaster at Start instead of Awake.

* Remove RequireComponent(typeof(BaseMultiMethodRaycaster)) attribute from BaseRaycastMethod.

* Pointer3DRaycaster now registered into Pointer3DInputModule at Start instead of Awake.

* EventCamera for Pointer3DRaycaster now place at root, instead of child of Pointer3DRaycaster.

* New ColliderEventSyatem. Hover thins using collider (instead of raycast), send button events to them, handle events by EventSystem-like handlers.
  - IColliderEventHoverEnterHandler
  - IColliderEventHoverExitHandler
  - IColliderEventPressDownHandler
  - IColliderEventPressUpHandler
  - IColliderEventPressEnterHandler
  - IColliderEventPressExitHandler
  - IColliderEventClickHandler
  - IColliderEventDragStartHandler
  - IColliderEventDragUpdateHandler
  - IColliderEventDragEndHandler
  - IColliderEventDropHandler
  - IColliderEventAxisChangeHandler

* New example scene to demonstrate how ColliderEvent works.
  - Assets\HTC.UnityPlugin\ViveInputUtility\Examples\5.ColliderEvent\ColliderEvent.unity

* Update tutorial & guide document.


Changes for v1.4.7:

* Now HandRole defines more then 2 controllers.

* Add some comment and description to public API.


Changes for v1.4.6:

* Fix a bug in the examples, now reticle posed correctly when scaling VROrigin.


Changes for v1.4.5:

* Fix a rare issue in Pointer3DInputModule when processing event raycast.


Changes for v1.4.4:

* Remove example 5 & 6 from package for release(still available in full package), since they are not good standard practices in VR for avoiding motion sickness by moving the player.

* Reset pointer's tranform(to align default laser pointer direction) in examples.

* Adjust default threshold to proper value in PoseStablizer & Pointer3DInputModule.

* Fix a bug in Pointer3DRaycaster that causes other input module to drive Pointer3DRaycaster(witch should be only driven by Poinster3DInputModule).

* Now Pointer3DRaycaster can optionally show event raycast line in editor for debugging.

* Add step by step tutorial document and example scene.

* Replace about document with developer guide.


Changes for v1.4.3:

* Update usage document(rewrite sample code).

* Add copyright terms.

* Define new controller button : FullTrigger(consider pressed only when trigger value is 1.0).

* Fix ViveInput.GetPadPressDelta and ViveInput.GetPadTouchDelta to work properly.

* Add scroll delta scale property for ViveRaycaster(to adjust scrolling sensitivity).

* Add PoseEaser effect settings and PoseEaserEditor to show properties.

* Add ViveInput.TriggerHapticPulse for triggering controller vibration.


Changes for v1.4.2:

* Update usage document.

* Reorder parameters in Pose.SetPose.

* Now click interval can be configured by setting ViveInput.clickInterval.


Changes for v1.4.1:

* Fix wrong initial status for ViveRole and ViveInput.

* Example: showLocalAvatar (property for LANGamePlayer) won't hide shadow (hide mesh only) if set to false.


Changes for v1.4.0:

* Separate PoseTracker module from VivePose.

* New tracking effect PoseFreezer.

* Reorganize folders.


Changes for v1.3.0:

* VivePose is now pure static class (Since Unity 5.3.5 fixed issue with double rendering of canvas on Vive VR, PoseUpdateMode is no longer needed).

* New components CanvasRaycastMethod and CanvasRaycastTarget.
  - CanvasRaycastMethod works like GraphicRaycastMethod, but use CanvasRaycastTarget component to target canvases, instead of asigning canvas property once at a time.


Changes for v1.2.0:

* Fix misspelling from ConvertRoleExtention to ConvertRoleExtension

* New containter class IndexedSet<T>

* New class ObjectPool<T> and relative class ListPool<T>, DictionaryPool<T>, IndexedSetPool<T>, to reduce allocating new containers.

* Change some data structure from LinkList to InedexedSet (VivePose, Pointer3DInputModule, BaseMultiMethodRaycaster, BaseVivePoseTracker).

* Rewrite GraphicRaycastMethod to align GraphicRaycaster's behaviour.


Changes for v1.1.0:

* New API VivePose.SetPose().

* New API VivePose.GetVelocity().

* New API VivePose.GetAngularVelocity().

* Fix some null reference in VivePose.