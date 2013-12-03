using UnityEngine;
using System.Collections;

public class GameStartManager : MonoBehaviour
{
// Update is called once per frame
	bool _keyPressed;
	void Update ()
	{
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
   