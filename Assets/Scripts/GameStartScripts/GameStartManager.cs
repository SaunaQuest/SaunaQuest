using UnityEngine;
using System.Collections;

public class GameStartManager : MonoBehaviour
{
// Update is called once per frame
	bool _keyPressed;

	void Update ()

	{   
		if (Input.GetKeyDown ("e"))
		{
			HealthBarScript.playerLevel = HealthBarScript.mode.expert;

		};
		  if (Input.GetKeyDown ("n"))
		{
			HealthBarScript.playerLevel = HealthBarScript.mode.normal;

			
		};
		  if (Input.GetKeyDown ("b"))
		{
			HealthBarScript.playerLevel = HealthBarScript.mode.beginner;

		};
		if (!_keyPressed) {
			if (Input.GetButtonDown ("Jump")) {
				StartGame ();
			}
			
			}
		}

	

	void StartGame ()
	{
		_keyPressed = true;
		Application.LoadLevel("gameOnScene");
	
	}
	
}
   