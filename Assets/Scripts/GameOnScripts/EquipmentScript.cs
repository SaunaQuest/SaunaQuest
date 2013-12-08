using UnityEngine;
using System.Collections;

public class EquipmentScript : MonoBehaviour {

	Texture2D bootsImage;
	Texture2D jacketImage;
	
	public float bootExpiryTime = 20;
	public float jacketExpiryTime = 20;
	public ArrayList equipmentList = new ArrayList();
	
	private int eqListLocationX = 10;
	private int eqListLocationY = 50;
	private int imageWidth = 50;
	private int imageHeight = 50;
	
	public enum Equipment_Type{
		JACKET, BOOTS
	}
	
	public class Equipment{
		float expiryTime; //how long the item will last
		Equipment_Type eqType;
		
		public Equipment(Equipment_Type eqType, float expiryTime){
			this.eqType = eqType;
			this.expiryTime = expiryTime;
		}
		
		public Equipment_Type getEquipmentType(){ return eqType;}
		public float getExpiryTime(){ return expiryTime;}

	}
	
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
		if(equipmentList!=null && equipmentList.Count>0){
			for(int index =0;index<equipmentList.Count;index++){

		            Equipment obj = (Equipment) equipmentList[index];
		            if(obj.getEquipmentType() == Equipment_Type.JACKET){
						bool clickButton = GUI.Button(new Rect(eqListLocationX+index*(5+imageWidth),eqListLocationY,
						imageWidth,imageHeight), jacketImage);
						if(clickButton){
							startUseEquipment(obj.getEquipmentType(),obj.getExpiryTime());
							obj = null;
							equipmentList.RemoveAt(index);
						}
					}
					else if(obj.getEquipmentType() == Equipment_Type.BOOTS){
						bool clickButton = GUI.Button(new Rect(eqListLocationX+index*(5+imageWidth),eqListLocationY,
						imageWidth,imageHeight), bootsImage);	
						if(clickButton){
							startUseEquipment(obj.getEquipmentType(),obj.getExpiryTime());
							obj = null;
							equipmentList.RemoveAt(index);						
						}					
					}
						
			}
		}
	}
	
	public void addBoots(){
		if(equipmentList.Count<15){
			equipmentList.Add(new Equipment(Equipment_Type.BOOTS,bootExpiryTime));
		}
	}
	
	public void addJacket(){
		if(equipmentList.Count<15){
			equipmentList.Add(new Equipment(Equipment_Type.JACKET,jacketExpiryTime));
		}
	}
	
	public void startUseEquipment(Equipment_Type type, float expiryTime){
		EquipmentUsageScript usage = (EquipmentUsageScript) transform.GetComponent("EquipmentUsageScript");		
		if(type==Equipment_Type.BOOTS){
			usage.useBoots(expiryTime);	
		}
		else if(type==Equipment_Type.JACKET){
			usage.useJacket(expiryTime);	
		}
	}
	
}
