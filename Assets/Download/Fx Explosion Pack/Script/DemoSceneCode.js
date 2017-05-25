var EffectName = ["Explosion1", "Explosion2", "Explosion3", "Explsoion4","Explosion5","Explosion6","Explosion7","Explosion8","Explosion9","Explosion10"];
var Effect = new Transform[53];
var Text1 : GUIText;
var i : int = 0;

function Start(){var obj = Instantiate(Effect[i], Vector3(0,0,0),Quaternion.identity);}

function Update () {

	Text1.text = i+1 + ":" +EffectName[i];
	
	if(Input.GetKeyDown(KeyCode.Z))
	{
		if(i<=0)
			i= 9;

		else
			i--;
		
		var obz = Instantiate(Effect[i], Vector3(0,0,0),Quaternion.identity);
	}
	
	if(Input.GetKeyDown(KeyCode.X))
	{
		if(i< 9)
			i++;

		else
			i=0;
		
		var obx = Instantiate(Effect[i], Vector3(0,0,0),Quaternion.identity);
	}
	
	if(Input.GetKeyDown(KeyCode.C))
		var obc = Instantiate(Effect[i], Vector3(0,0,0),Quaternion.identity);
}