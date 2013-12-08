using UnityEngine;
using System.Collections;

public class EquipmentUsageScript : MonoBehaviour {
	
	public bool bootsUsed=false;	
	public float bootsTimeLeft=0;
	public bool jacketUsed=false;
	public float jacketTimeLeft = 0;
	
	Texture2D bootsImage;
	Texture2D jacketImage;	
	
	private int jacketUsageLocationX = 10;
	private int jacketUsageLocationY = 300;
	private int bootsUsageLocationX = 10;
	private int bootsUsageLocationY = 380;
	
	private int imageWidth = 50;
	private int imageHeight = 50;	
	
	// Use this for initialization
	void Start () {
		bootsImage= (Texture2D) Resources.Load("boots_picture",typeof(Texture2D));
		jacketImage = (Texture2D) Resources.Load("jacket_picture",typeof(Texture2D));	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnGUI ()
	{	
		if(bootsUsed || jacketUsed){
			if(bootsUsed){
			    if (bootsImage!=null) {
					bootsTimeLeft = bootsTimeLeft - Time.deltaTime;
					if(bootsTimeLeft>5){
						GUI.Button(new Rect(bootsUsageLocationX,bootsUsageLocationY,imageWidth,imageHeight), bootsImage);
						GUI.Label (new Rect(bootsUsageLocationX+imageWidth+ 10,bootsUsageLocationY+15,imageWidth,imageHeight),bootsTimeLeft.ToString("F2") );
					}
					else if (bootsTimeLeft >0){
						//make it blinking
						if(Time.time%0.5<0.25){ //blink every half second (with 1/4second showing picture and 1/4 second empty)
			    			GUI.Button(new Rect(bootsUsageLocationX,bootsUsageLocationY,imageWidth,imageHeight), bootsImage);
							GUI.Label (new Rect(bootsUsageLocationX+imageWidth+ 10,bootsUsageLocationY+15,imageWidth,imageHeight),bootsTimeLeft.ToString("F2") );
						}	
					}
					else if (bootsTimeLeft < 0){
						bootsUsed = false;
						bootsTimeLeft = 0;
					}
			    } else {
			    	Debug.Log("Boots image not found");
			    }
			}
			if(jacketUsed){
				if (jacketImage!=null) {
					jacketTimeLeft = jacketTimeLeft - Time.deltaTime;
					if(jacketTimeLeft>5){
						GUI.Button(new Rect(jacketUsageLocationX,jacketUsageLocationY,imageWidth,imageHeight), jacketImage);
						GUI.Label (new Rect(jacketUsageLocationX+imageWidth+ 10,jacketUsageLocationY+15,imageWidth,imageHeight),jacketTimeLeft.ToString("F2") );
						
					}
					else if (jacketTimeLeft >0){
						//make it blinking
						if(Time.time%0.5<0.25){
			    			GUI.Button(new Rect(jacketUsageLocationX,jacketUsageLocationY,imageWidth,imageHeight), jacketImage);
							GUI.Label (new Rect(jacketUsageLocationX+imageWidth+ 10,jacketUsageLocationY+15,imageWidth,imageHeight),jacketTimeLeft.ToString("F2") );							
						}	
					}
					else if (jacketTimeLeft < 0){
						jacketUsed = false;
						jacketTimeLeft = 0;
					}
			    } else {
			    	Debug.Log("Jacket image not found");
			    }				
			}
		}
	}
	
	public void useBoots(float expiryTime){
		bootsUsed = true;
		bootsTimeLeft+=expiryTime;
	}
	
	public void useJacket(float expiryTime){
		jacketUsed = true;
		jacketTimeLeft+=expiryTime;
	}
}
