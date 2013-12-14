using UnityEngine;
using System.Collections;
     
public class HealthBarScript : MonoBehaviour
{
	public enum mode
	{
		beginner,
		normal,
		expert
	};
	
	public static mode playerLevel = mode.normal;
	GUIStyle style = new GUIStyle ();
	GUIStyle style1 = new GUIStyle ();
	Texture2D texture;
	Color redColor = Color.red;
	Color yellowColor = Color.yellow;
	static public float curHealth ;
	static public float maxHealth ;
	public float decreaseRate = 0;
	public float idleDecreaseRate = 0.05f;
	public float walkingDecreaseRate = 0.2f;
	public float trottingDecreaseRate = 0.2f;
	public float runningDecreaseRate = 0.1f;
	public float jumpingDecreaseRate = 0.1f;
	private GameObject playerObject;
	float decreasedHealth = 0;
	bool disableDecreaseRate = false;
	Color myGreen = new Color (0.0f, 0.4f, 0.0f);
     
	void Start ()
	{
		
		switch (playerLevel) {

		case mode.beginner:
			curHealth = 200;
			maxHealth = 200;
			break;
		case mode.normal:
			curHealth = 100;
			maxHealth = 100;
			break;
		case mode.expert:
			curHealth = 50;
			maxHealth = 50;
			break;
		default:
			curHealth = 100;
			maxHealth = 100;
			break;
		}
		decreaseRate = 0;
		idleDecreaseRate = 0.05f;
		walkingDecreaseRate = 0.2f;
		trottingDecreaseRate = 0.2f;
		runningDecreaseRate = 1.0f;
		jumpingDecreaseRate = 1.0f;	

		decreasedHealth = 0;
		disableDecreaseRate = false;
		Color myGreen = new Color (0.0f, 0.4f, 0.0f);
		
		
		texture = new Texture2D (1, 1);
		texture.SetPixel (1, 1, myGreen);
	}
     
	private void Update ()
	{	

		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}	
		
		ThirdPersonController controller = (ThirdPersonController)GetComponent ("ThirdPersonController");
		CharacterState state = controller.getCharacterState ();
		if (state == CharacterState.Dead) {
			decreaseRate = 0;
			disableHealthBar ();
		}
		if (!disableDecreaseRate) {


			
			if (state == CharacterState.Idle) {
				decreaseRate = idleDecreaseRate;
			} else if (state == CharacterState.Walking) {
				decreaseRate = walkingDecreaseRate;
			} else if (state == CharacterState.Trotting) {
				decreaseRate = trottingDecreaseRate;
			} else if (state == CharacterState.Running) {
				decreaseRate = runningDecreaseRate;
			} else if (state == CharacterState.Jumping) {
				decreaseRate = jumpingDecreaseRate;
			}				
			EquipmentUsageScript usage = (EquipmentUsageScript)transform.GetComponent ("EquipmentUsageScript");
			bool hasJacket = usage.jacketUsed;
			if (hasJacket) {
				decreaseRate = 0;	
			}
			decreasedHealth = Time.deltaTime * decreaseRate;
			if (curHealth - decreasedHealth >= 0) {
				curHealth = curHealth - decreasedHealth;
			} else {
				curHealth = 0;
			}
			if (curHealth > 50) {
				texture.SetPixel (1, 1, myGreen);
			}
			if (curHealth < 50) {
				texture.SetPixel (1, 1, yellowColor);
			}
			if (curHealth < 30) {
				texture.SetPixel (1, 1, redColor);
			}
			if (curHealth == 0) {
				controller.setCharacterDead ();
			}
		}
	}
     
	public void OnGUI ()
	{
     
		texture.Apply ();
		style1.fontSize = 36;
     	
		style.normal.background = texture;
		style1.normal.textColor = Color.white;
		if (!disableDecreaseRate) {
			GUI.Box (new Rect (10, 10, 500 * (curHealth / maxHealth), 40), new GUIContent (""), style);
			GUI.Box (new Rect (10, 10, 500, 40), new GUIContent (curHealth.ToString ("F2")), style1);
		}
	}
	
	public void disableHealthBar ()
	{
		disableDecreaseRate = true;	
	}
	
	public void enableHealthBar ()
	{
		disableDecreaseRate = false;	
	}
	
	
}