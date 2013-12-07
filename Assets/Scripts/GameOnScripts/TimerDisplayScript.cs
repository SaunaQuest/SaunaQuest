using UnityEngine; using System.Collections;

public class TimerDisplayScript : MonoBehaviour
{
	private float currentTime;
	private float restSeconds;
	private int roundedRestSeconds;
	public float displaySeconds;
	public float displayMinutes;
	string timetext;
	public bool runTimer = true;

 
// Use this for initialization
 
	void Start ()
	{


 
	}
 
	void OnGUI ()
	{		
			
		if (runTimer) {
			currentTime = Time.time;
		}

 
		roundedRestSeconds = Mathf.CeilToInt (currentTime);
		displaySeconds = roundedRestSeconds % 60;
		displayMinutes = (roundedRestSeconds / 60) % 60;
	
		timetext = (displayMinutes.ToString () + ":");
		if (displaySeconds > 9) {
			timetext = timetext + displaySeconds.ToString ();
		} else {
			timetext = timetext + "0" + displaySeconds.ToString ();
		}
		
		GUI.Label (new Rect (600.0f, 0.0f, 100.0f, 75.0f), timetext);
	}
	
	void disableTimer ()
	{
		runTimer = false;
	}
}