using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	public InputField text;
	public World world;
	public InputField[] craftInput0;
	public InputField[] craftInput1;
	public InputField[] craftInput2;
	public InputField[,] craftInput;
	public Image[,] images;
	public InputField craftResult;
	public Image craftImage;

	public RectTransform inventory;
	public string[] inventoryItems;
	private Image[] inventorySlots;
	public int inventorySelectedSlot;
	private Image inventorySelector;

	public RectTransform hotbar;
	public string[] hotbarItems;
	private Image[] hotbarSlots;
	public int hotbarSelectedSlot;
	private Image hotbarSelector;

	public Material hotbarSelectorMat,slotMat;
	public Vector2 slotSize,hotbarSelectorOffset;

	public Material inventorySelectorMat;
	public Vector2 inventorySelectorOffset;

	public Camera cam;
	public bool isRunning=false;
	void Start()
	{
		cam.farClipPlane=world.xChunks*world.xBlocks-4;


		craftInput=new InputField[3,3];
		images=new Image[3,3];
		for(int v=0;v<3;v++)
		{
			InputField[] f=v==0?craftInput0:(v==1?craftInput1:craftInput2);
			for(int v1=0;v1<3;v1++)
			{
				craftInput[v,v1]=f[v1];
				images[v,v1]=new GameObject().AddComponent<Image>();
				images[v,v1].rectTransform.SetParent(f[v1].gameObject.transform);
				images[v,v1].material=new Material(world.material);
				images[v,v1].rectTransform.sizeDelta=new Vector2(30,30);
				images[v,v1].rectTransform.localPosition=new Vector3(120/2-slotSize.x/2,0,0);
			}
		}
		craftImage.material=new Material(world.material);

		hotbar.sizeDelta=new Vector2(slotSize.x*hotbarItems.Length,slotSize.y*2);
		hotbar.position=new Vector2(hotbar.position.x,slotSize.y*downOffset);
		hotbarSlots=new Image[hotbarItems.Length];
		for(int v=0;v<hotbarItems.Length;v++)
		{
			Image slot=new GameObject().AddComponent<Image>();
			slot.gameObject.name="Slot "+v;
			slot.rectTransform.SetParent(hotbar);
			slot.material=slotMat;
			slot.rectTransform.sizeDelta=slotSize;
			slot.rectTransform.localPosition=new Vector3((v-hotbarItems.Length/2)*slotSize.x,0,0);
			hotbarSlots[v]=new GameObject().AddComponent<Image>();
			hotbarSlots[v].rectTransform.SetParent(slot.rectTransform);
			hotbarSlots[v].material=new Material(world.material);
		}
		hotbarSelector=new GameObject().AddComponent<Image>();
		hotbarSelector.gameObject.name="Slot Selector";
		hotbarSelector.rectTransform.SetParent(hotbar);
		hotbarSelector.material=hotbarSelectorMat;
		hotbarSelector.rectTransform.sizeDelta=slotSize+hotbarSelectorOffset;



		inventory.sizeDelta=new Vector2(slotSize.x*inventoryItems.Length,slotSize.y*2);
		inventory.position=new Vector2(inventory.position.x,slotSize.y*downOffset*3);
		inventorySlots=new Image[inventoryItems.Length];
		for(int v=0;v<inventoryItems.Length;v++)
		{
			Image slot=new GameObject().AddComponent<Image>();
			slot.gameObject.name="Slot "+v;
			slot.rectTransform.SetParent(inventory);
			slot.material=slotMat;
			slot.rectTransform.sizeDelta=slotSize;
			slot.rectTransform.localPosition=new Vector3((v-inventoryItems.Length/2)*slotSize.x,0,0);
			inventorySlots[v]=new GameObject().AddComponent<Image>();
			inventorySlots[v].rectTransform.SetParent(slot.rectTransform);
			inventorySlots[v].material=new Material(world.material);
		}
		inventorySelector=new GameObject().AddComponent<Image>();
		inventorySelector.gameObject.name="Slot Selector";
		inventorySelector.rectTransform.SetParent(inventory);
		inventorySelector.material=inventorySelectorMat;
		inventorySelector.rectTransform.sizeDelta=slotSize+inventorySelectorOffset;
	}
	List<string> prevCmds=new List<string>();
	public Vector2 mouseSenvisity=new Vector2(2,2);
	public Vector3 speeds=new Vector3(0.3f,0.3f,0.3f);
	public Vector3 jump=new Vector3(0,2,0);
	public Vector3 runModifiers=new Vector3(0,3,0);
	public Vector3 exMinEulerAngles=new Vector3(0,0,0);
	public Vector3 minEulerAngles=new Vector3(-90,0,0);
	public Vector3 exMaxEulerAngles=new Vector3(0,0,0);
	public Vector3 maxEulerAngles=new Vector3(90,0,0);

	void CommandFU()
	{
		bool cmd=false;
		if(text.text.Split(' ').Length>4)
			if(text.text.Split(' ')[0]=="/setblock")
			{
				cmd=true;
				world.SetBlock(new EntityLocation(int.Parse(text.text.Split(' ')[1]),int.Parse(text.text.Split(' ')[2]),int.Parse(text.text.Split(' ')[3])),Entity.create(EntityId.ByName(text.text.Split(' ')[4])));
			}
		if(text.text.Split(' ').Length>7)
			if(text.text.Split(' ')[0]=="/fill")
			{
				cmd=true;
				for(int x=int.Parse(text.text.Split(' ')[1]);x<int.Parse(text.text.Split(' ')[5]);x++)
					for(int y=int.Parse(text.text.Split(' ')[2]);y<int.Parse(text.text.Split(' ')[6]);y++)
						for(int z=int.Parse(text.text.Split(' ')[3]);z<int.Parse(text.text.Split(' ')[7]);z++)
							world.SetBlock(new EntityLocation(x,y,z),Entity.create(EntityId.ByName(text.text.Split(' ')[4])));
			}
		if(text.text.Split(' ').Length>7)
			if(text.text.Split(' ')[0]=="/fillarea")
			{
				cmd=true;
				for(int x=int.Parse(text.text.Split(' ')[1]);x<int.Parse(text.text.Split(' ')[1])+int.Parse(text.text.Split(' ')[5]);x++)
					for(int y=int.Parse(text.text.Split(' ')[2]);y<int.Parse(text.text.Split(' ')[2])+int.Parse(text.text.Split(' ')[6]);y++)
						for(int z=int.Parse(text.text.Split(' ')[3]);z<int.Parse(text.text.Split(' ')[3])+int.Parse(text.text.Split(' ')[7]);z++)
							world.SetBlock(new EntityLocation(x,y,z),Entity.create(EntityId.ByName(text.text.Split(' ')[4])));
			}
		if(cmd)
		{
			text.text="$"+text.text;
			prevCmds.Add(text.text.Substring(1));
		}
	}
	void CraftingFU()
	{
		string[,] cells=new string[3,3];
		for(int v=0;v<3;v++)
			for(int v1=0;v1<3;v1++)
			{
				cells[v,v1]=craftInput[v,v1].text;
				images[v,v1].material.SetTextureOffset("_MainTex",EntityId.ByName(craftInput[v,v1].text).textures[0]*images[v,v1].material.GetTextureScale("_MainTex"));
			}

		craftResult.text=string.Join(Craft.spl0+"",Craft.Do(cells));
		//Debug.Log(craftResult.text.Split(Craft.spl0)[0]);
		craftImage.material.SetTextureOffset("_MainTex",EntityId.ByName(craftResult.text.Split(Craft.spl0)[0]).textures[0]*craftImage.material.GetTextureScale("_MainTex"));
	}
	void HotbarFU()
	{
		for(int v=0;v<12;v++)
			if(Input.GetAxis("Slot"+v)>0)
				hotbarSelectedSlot=v;
		for(int v=0;v<hotbarItems.Length;v++)
		{
			hotbarSlots[v].material.SetTextureOffset("_MainTex",EntityId.ByName(hotbarItems[v]).textures[0]*hotbarSlots[v].material.GetTextureScale("_MainTex"));
			hotbarSlots[v].rectTransform.localPosition=new Vector3(0,0,0);
			hotbarSlots[v].rectTransform.sizeDelta=slotSize*(EntityId.ByName(hotbarItems[v]).type=="block"?blockMultiplier:otherMultiplier);
		}

		hotbarSelector.rectTransform.localPosition=new Vector3((hotbarSelectedSlot-hotbarItems.Length/2)*slotSize.x,0,0);
	}
	public bool gui;
	public float downOffset=0.65f,blockMultiplier=0.8f,otherMultiplier=0.6f;

	bool lastInv;
	void GuiFU()
	{
		if(Input.GetAxis("Inventory")>0&&!lastInv)
			gui=!gui;
		lastInv=Input.GetAxis("Inventory")>0;
		Cursor.visible=gui;
		Cursor.lockState=gui?CursorLockMode.None:CursorLockMode.Locked;
		hidingGui.SetActive(gui);
		showingGui.SetActive(!gui);
	}
	void InteractFU()
	{

	}
	public Permissions perms;
	bool onGround()
	{
		RaycastHit hit;
		return Physics.Raycast(transform.position, -transform.up, out hit, 0.1f);
	}
	public void ToHotbar()
	{
		hotbarItems[hotbarSelectedSlot]=craftResult.text.Split(Craft.spl0)[0];
	}
	void MovementFU()
	{
		Vector3 nev=new Vector3();
		nev.x+= speeds.x*(perms.moveX	?1:0)*Input.GetAxis("Horizontal")*(	isRunning?runModifiers.x:1);
		nev.y+= speeds.y*(perms.moveY	?1:0)*Input.GetAxis("Fly")	*	(	isRunning?runModifiers.y:1);
		nev.z+= speeds.z*(perms.moveZ	?1:0)*Input.GetAxis("Vertical")*(	isRunning?runModifiers.z:1);
		nev	+=	jump	*(perms.jump	?1:0)*(Input.GetAxis("Jump")>0?1:0)*(onGround()?1:0);
		Debug.Log(onGround());

		Vector3 movement=rel(nev);
		GetComponent<Rigidbody>().velocity+=movement;

		cam.transform.Rotate(-Input.GetAxis("Mouse Y")	*	mouseSenvisity.y*(perms.rotateX?1:0),0,0);
		transform.Rotate(0,	Input.GetAxis("Mouse X")	*	mouseSenvisity.x*(perms.rotateY?1:0),	0);
		cam.transform.Rotate(0,	0,Input.GetAxis("Mouse Z")*	mouseSenvisity.y*(perms.rotateZ?1:0));


		//TODO transform.position=chunks.normalize(transform.position);
		float x=ogr(minEulerAngles.x,cam.transform.eulerAngles.x,maxEulerAngles.x,exMinEulerAngles.x!=0,exMaxEulerAngles.x!=0);
		float y=ogr(minEulerAngles.y,cam.transform.eulerAngles.y,maxEulerAngles.y,exMinEulerAngles.y!=0,exMaxEulerAngles.y!=0);
		float z=ogr(minEulerAngles.z,cam.transform.eulerAngles.z,maxEulerAngles.z,exMinEulerAngles.z!=0,exMaxEulerAngles.z!=0);
		cam.transform.eulerAngles=new Vector3(x,y,z);
	}
	float ogr(float min,float cur,float max,bool hasMin,bool hasMax)
	{
		float ogr0=hasMin?(min>cur?min:cur):cur;
		ogr0=hasMax?(max<cur?max:cur):cur;
		return ogr0;
	}
	float ogr(float min,float cur,float max)
	{
		return ogr(min,cur,max,true,true);
	}
	Vector3 rel(Vector3 v)
	{
		return transform.right*v.x+transform.up*v.y+transform.forward*v.z;
	}
	void InventoryFU()
	{
		for(int v=0;v<inventoryItems.Length;v++)
		{
			inventorySlots[v].material.SetTextureOffset("_MainTex",EntityId.ByName(inventoryItems[v]).textures[0]*inventorySlots[v].material.GetTextureScale("_MainTex"));
			inventorySlots[v].rectTransform.localPosition=new Vector3(0,0,0);
			inventorySlots[v].rectTransform.sizeDelta=slotSize*(EntityId.ByName(inventoryItems[v]).type=="block"?blockMultiplier:otherMultiplier);
		}

		inventorySelector.rectTransform.localPosition=new Vector3((inventorySelectedSlot-inventoryItems.Length/2)*slotSize.x,0,0);
	}
	// Update is called once per frame
	void FixedUpdate()
	{
		GuiFU();
		if(gui)
		{
			CommandFU();
			CraftingFU();
			InventoryFU();
		}
		else
		{
			MovementFU();
			InteractFU();
		}
		HotbarFU();
	}
	public GameObject hidingGui;
	public GameObject showingGui;
}
