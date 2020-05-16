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
    // Start is called before the first frame update
    void Start()
    {
		craftInput=new InputField[3,3];
		images=new Image[3,3];
		for(int v=0;v<3;v++)
		{
			InputField[] f=v==0?craftInput0:(v==1?craftInput1:craftInput2);
			for(int v1=0;v1<3;v1++)
			{
				craftInput[v,v1]=f[v1];
				images[v,v1]=new GameObject().AddComponent<Image>();
				images[v,v1].transform.parent=f[v1].gameObject.transform;
				images[v,v1].material=new Material(world.material);
				images[v,v1].rectTransform.sizeDelta=new Vector2(30,30);
				images[v,v1].rectTransform.localPosition=new Vector3(120/2-images[v,v1].rectTransform.sizeDelta.x/2,0,0);
			}
		}
		craftImage.material=new Material(world.material);
    }
	List<string> prevCmds=new List<string>();

	void Command()
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
	void Crafting()
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
    // Update is called once per frame
    void FixedUpdate()
    {
		Command();
		Crafting();
    }
}
