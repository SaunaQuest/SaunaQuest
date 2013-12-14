/// <summary>
/// Simple horizontal style compass for 3D world
/// Attach this to your Player. 0 degrees is North. 
/// Adjust if your orientation is different by changing the letters.
/// Use/modify anyway you like.
/// </summary>
 
using UnityEngine;
using System.Collections;
 
public class Compass : MonoBehaviour {
 
    // Feel free to set these all to private once you are done tweaking:
    private Transform myTransform;
    public int facingDir;
    public int degreeOffset;
	GUIStyle style = new GUIStyle();

 
    void Start () {
       	myTransform = transform;

    }
 
    // Minor adjustments from 0,90,180,270 due to Font width
    // Adjust letter offset to match your Font size/width
 
    void OnGUI() {

		style.normal.textColor = Color.yellow;
		GUI.backgroundColor = Color.black;

		style.fontSize = 30;
 
       if(degreeOffset > -85 && degreeOffset < 90) {
         GUI.Label( new Rect((Screen.width/2)-degreeOffset*2,
         (Screen.height)-50,
         180,
         50),
         "N",style);
       }
 
       if(degreeOffset > 5 && degreeOffset < 180) {
         GUI.Label( new Rect((Screen.width/2)-degreeOffset*2+180,
         (Screen.height)-50,
         180,
         50),
         "E",style);
       } 
 
       if((facingDir > 95 && degreeOffset> 95) || (facingDir < 276 && degreeOffset < -90)) {
         GUI.Label( new Rect((Screen.width/2)-facingDir*2+360,
         (Screen.height)-50,
         180,
         50),
         "S",style);    
       }
 
       if((facingDir > 186 && degreeOffset < -5)) {
         GUI.Label( new Rect((Screen.width/2)-facingDir*2+540,
         (Screen.height)-50,
         180,
         50),
         "W",style);
       }
 
       GUI.Box( new Rect((Screen.width/2)-90,
         (Screen.height)-75,
         360,
         60),
         "Heading",style);
    }
 
    void Update () {
 
       facingDir = (int)Mathf.Abs(myTransform.eulerAngles.y);
        if (facingDir > 360) facingDir = facingDir % 360;
 
       degreeOffset = facingDir;
 
       if(degreeOffset > 180) degreeOffset = degreeOffset - 360;
 
    }
}