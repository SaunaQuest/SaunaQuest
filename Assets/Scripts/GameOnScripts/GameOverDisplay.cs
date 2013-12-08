using UnityEngine;
using System.Collections;

public class GameOverDisplay : MonoBehaviour {

	public bool showText=false;
	public string textToShow;
	
public void OnGUI()
    {
		
		if(showText){
	     	
	     	GUIStyle style = new GUIStyle();
	     	style.fontSize=Screen.width/50;
			style.normal.textColor = Color.yellow;
	    	var textArea = new Rect(Screen.width/3,Screen.height/3,Screen.width, Screen.height);
		    GUI.Label(textArea,textToShow,style);
		}

    }
}
