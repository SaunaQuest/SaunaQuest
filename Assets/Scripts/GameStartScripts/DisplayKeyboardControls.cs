using UnityEngine;
using System.Collections;

public class DisplayKeyboardControls : MonoBehaviour {
	

	
	public Color splashScreenFontColor  = new Color(1.0f,1.0f,1.0f);
	public Color splashScreenTitleColor  = new Color(1.0f,1.0f,1.0f);
    public void OnGUI()
    {

     	var textTitle = "Welcome to Sauna Quest \n";
		
     	var text = "You are travelling in freezing cold weather.\n";
     	text += "Hypothermia is a harsh foe so you need to run to survive.\n";
     	text += "Reach the sauna before you die of cold!\n";     	
     	text += "\n";     	
     	text += "Game Controls:\n";
     	text += "W or Up arrow : move forward\n";
     	text += "S  or Down arrow: move brackward\n";
     	text += "A or Left arrow: turn left\n";
     	text += "D or Right arrow: turn right\n";
     	text += "Shift: run\n";
     	text += "Space: jump\n";
		text += "To use items: left click the item with mouse\n";
		text += "Boots make you able to run normally across mud\n";
		text += "Jacket prevent you from getting cold\n";
     	text += "\n";
		text += "B : Beginner Mode\n";
		text += "I : Intermediate Mode\n";
		text += "E : Expert Mode\n";
		text += "\n";	
     	text += "Press Space to start game\n";
		text += "Press escape to quit at any time \n";
		
		GUIStyle styleTitle = new GUIStyle();
    	styleTitle.fontSize=Screen.width/15;
		styleTitle.normal.textColor = Color.white;
    	var textAreaTitle = new Rect(Screen.width/7,0,Screen.width, Screen.height);
	    GUI.Label(textAreaTitle,textTitle,styleTitle);
		

     	GUIStyle style = new GUIStyle();
     	style.fontSize=Screen.width/50;
		style.normal.textColor = Color.yellow;
    	var textArea = new Rect(Screen.width/3.5f,Screen.height/9,Screen.width, Screen.height);
	    GUI.Label(textArea,text,style);

    }
}
