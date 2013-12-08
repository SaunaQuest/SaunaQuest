using UnityEngine;
using System.Collections;
     
public class HealthBarScript : MonoBehaviour
{
     
	GUIStyle style = new GUIStyle ();
	Texture2D texture;
	Color redColor = Color.red;
	Color greenColor = Color.green;
	Color yellowColor = Color.yellow;
	public float curHealth = 100;
	public float maxHealth = 100;
	public float decreaseRate = 0;
	public float idleDecreaseRate = 0.005f;
	public float walkingDecreaseRate = 0.02f;
	public float trottingDecreaseRate = 0.02f;
	public float runningDecreaseRate = 0.1f;
	public float jumpingDecreaseRate = 0.1f;	
	private GameObject playerObject;
	float decreasedHealth = 0;
	bool disableDecreaseRate = false;
     
	void Start ()
	{
		texture = new Texture2D (1, 1);
		texture.SetPixel (1, 1, greenColor);
	}
     
	private void Update ()
	{
     
		if (!disableDecreaseRate) {
			ThirdPersonController controller = (ThirdPersonController)GetComponent ("ThirdPersonController");
			CharacterState state = controller.getCharacterState ();
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
			EquipmentUsageScript usage = (EquipmentUsageScript) transform.GetComponent("EquipmentUsageScript");
			bool hasJacket = usage.jacketUsed;
			if(hasJacket){
				decreaseRate =0;	
			}
			decreasedHealth = Time.deltaTime * decreaseRate;
			if (curHealth - decreasedHealth >= 0) {
				curHealth = curHealth - decreasedHealth;
			} else {
				curHealth = 0;
			}
			if (curHealth > 50) {
				texture.SetPixel (1, 1, greenColor);
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
     	
		style.normal.background = texture;
		GUI.Box (new Rect (10, 10, 500 * (curHealth / maxHealth), 20), new GUIContent (""), style);
		GUI.Box (new Rect (10, 10, 500, 20), new GUIContent (curHealth.ToString ("F2")));
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