using UnityEngine;
using System.Collections;

public class DisplayKeyboardControls : MonoBehaviour {

    public void OnGUI()
    {
     	var text = "Welcome to Sauna Quest Game...\n";
     	text += "You are a man travelling in a freezing cold weather.\n";
     	text += "It is really cold and you need to run and survive.\n";
     	text += "Reach the sauna before you die of cold!\n";     	
     	text += "\n";     	
     	text += "Game Controls:\n";
     	text += "W : move forward\n";
     	text += "S : move backward\n";
     	text += "A : turn left\n";
     	text += "D : turn right\n";
     	text += "Shift: run\n";
     	text += "Space: jump\n";
		text += "To use items: left click the item with mouse\n";
		text += "Boots make you able to run normally across mud\n";
		text += "Jacket prevent you from getting cold\n";
     	text += "\n";
     	text += "Press Space to start game\n";
     	
     	GUIStyle style = new GUIStyle();
     	style.fontSize=Screen.width/50;
    	var textArea = new Rect(Screen.width/10,Screen.height/10,Screen.width, Screen.height);
	    GUI.Label(textArea,text,style);

    }
}
