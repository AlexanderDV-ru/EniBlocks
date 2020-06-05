using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	public InputField text;
	public InputField[] craftInput0;
	public InputField[] craftInput1;
	public InputField[] craftInput2;
	public InputField[,] craftInput;
	public Image[,] images;
	public InputField craftResult;
	public Image craftImage;

	public Material slotMat;
	public Vector2 slotSize;

	public InventoryUi inventoryUi,hotbarUi;

	public Camera cam;
	void Start()
	{
		cam.farClipPlane=mob.world.xChunks*mob.world.xBlocks-4;


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
				images[v,v1].material=new Material(mob.world.material);
				images[v,v1].rectTransform.sizeDelta=new Vector2(30,30);
				images[v,v1].rectTransform.localPosition=new Vector3(120/2-slotSize.x/2,0,0);
			}
		}
		craftImage.material=new Material(mob.world.material);
	}
	List<string> prevCmds=new List<string>();

	void CommandFU()
	{
		bool cmd=false;
		if(text.text.Split(' ').Length>4)
			if(text.text.Split(' ')[0]=="/setblock")
			{
				cmd=true;
				mob.world.SetBlock(new EntityLocation(int.Parse(text.text.Split(' ')[1]),int.Parse(text.text.Split(' ')[2]),int.Parse(text.text.Split(' ')[3])),Entity.create(EntityId.ByName(text.text.Split(' ')[4])));
			}
		if(text.text.Split(' ').Length>7)
			if(text.text.Split(' ')[0]=="/fill")
			{
				cmd=true;
				for(int x=int.Parse(text.text.Split(' ')[1]);x<int.Parse(text.text.Split(' ')[5]);x++)
					for(int y=int.Parse(text.text.Split(' ')[2]);y<int.Parse(text.text.Split(' ')[6]);y++)
						for(int z=int.Parse(text.text.Split(' ')[3]);z<int.Parse(text.text.Split(' ')[7]);z++)
							mob.world.SetBlock(new EntityLocation(x,y,z),Entity.create(EntityId.ByName(text.text.Split(' ')[4])));
			}
		if(text.text.Split(' ').Length>7)
			if(text.text.Split(' ')[0]=="/fillarea")
			{
				cmd=true;
				for(int x=int.Parse(text.text.Split(' ')[1]);x<int.Parse(text.text.Split(' ')[1])+int.Parse(text.text.Split(' ')[5]);x++)
					for(int y=int.Parse(text.text.Split(' ')[2]);y<int.Parse(text.text.Split(' ')[2])+int.Parse(text.text.Split(' ')[6]);y++)
						for(int z=int.Parse(text.text.Split(' ')[3]);z<int.Parse(text.text.Split(' ')[3])+int.Parse(text.text.Split(' ')[7]);z++)
							mob.world.SetBlock(new EntityLocation(x,y,z),Entity.create(EntityId.ByName(text.text.Split(' ')[4])));
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
				hotbarUi.inventory.selected=v;
		hotbarUi.InventoryFU();
	}
	void InteractFU()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		mob.InteractFU(Input.GetAxis("Break"),Input.GetAxis("Place"),Input.GetAxis("Clone"),ray);
		breakSelector.position=mob.breakSelector;
		placeSelector.position=mob.placeSelector;
	}
	public Transform breakSelector,placeSelector;
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
	public void ToHotbar()
	{
		hotbarUi.inventory.items[hotbarUi.inventory.selected]=craftResult.text.Split(Craft.spl0)[0];
	}
	public Mob mob;
	// Update is called once per frame
	void FixedUpdate()
	{
		GuiFU();
		if(gui)
		{
			CommandFU();
			CraftingFU();
			inventoryUi.InventoryFU();
		}
		else
		{
			mob.MovementFU(new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Fly"),Input.GetAxis("Vertical")),Input.GetAxis("Jump"),new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"),Input.GetAxis("Mouse Z")));
			InteractFU();
		}
		HotbarFU();
	}
	public GameObject hidingGui;
	public GameObject showingGui;
}
