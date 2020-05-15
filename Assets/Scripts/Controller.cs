using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	public InputField text;
	public World world;
    // Start is called before the first frame update
    void Start()
    {

    }
	List<string> prevCmds=new List<string>();
    // Update is called once per frame
    void FixedUpdate()
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
}
