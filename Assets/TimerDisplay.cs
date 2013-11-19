using UnityEngine; using System.Collections;

public class TimerDisplay : MonoBehaviour {
private float startTime;
private float restSeconds;
private int roundedRestSeconds;
private float displaySeconds;
private float displayMinutes;
public int CountDownSeconds=600;
private float Timeleft;
string timetext;
public int initialfontSize = 12;
private bool fontSizeSet = false;
private bool fontEnlarged240 = false;
private bool fontEnlarged180 = false;
private bool fontEnlarged120 = false;
private bool fontEnlarged60 = false;

 
 
// Use this for initialization
 
void Start () 
{
    startTime=Time.deltaTime;

 
}
 
void OnGUI()
{
    if (fontSizeSet == false){
			fontSizeSet = true;
			GUI.skin.label.fontSize = initialfontSize;			
		}
		
		
			
    Timeleft= Time.time-startTime;
 
    restSeconds = CountDownSeconds-(Timeleft);

 
roundedRestSeconds=Mathf.CeilToInt(restSeconds);
displaySeconds = roundedRestSeconds % 60;
displayMinutes = (roundedRestSeconds / 60)%60;
		
if ((fontEnlarged240 == false) && (roundedRestSeconds == 240))
{
			GUI.skin.label.fontSize = GUI.skin.label.fontSize + 4;
		    fontEnlarged240 = true;
}		
		
if ((fontEnlarged180 == false) && (roundedRestSeconds == 180))
{
			GUI.skin.label.fontSize = GUI.skin.label.fontSize + 4;
		    fontEnlarged180 = true;
}		

		
if ((fontEnlarged120 == false) && (roundedRestSeconds == 120))
{
			GUI.skin.label.fontSize = GUI.skin.label.fontSize + 4;
		    fontEnlarged120 = true;
}		
		
if ((fontEnlarged60 == false) && (roundedRestSeconds == 60))
{
			GUI.skin.label.fontSize = GUI.skin.label.fontSize + 8;
		    fontEnlarged60 = true;
}		
		
timetext = (displayMinutes.ToString()+":");
if (displaySeconds > 9)
{
    timetext = timetext + displaySeconds.ToString();
}
else 
{
    timetext = timetext + "0" + displaySeconds.ToString();
}
		
GUI.Label(new Rect(560.0f, 0.0f, 100.0f, 75.0f), timetext);
    }}