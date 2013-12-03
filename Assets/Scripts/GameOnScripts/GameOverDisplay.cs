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
	    	var textArea = new Rect(Screen.width/10,Screen.height/10,Screen.width, Screen.height);
		    GUI.Label(textArea,textToShow,style);
		}

    }
}
