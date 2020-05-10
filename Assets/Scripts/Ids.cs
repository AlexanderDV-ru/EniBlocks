using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ids : MonoBehaviour
{
	//[visible][prozrachniy][gravity] [texture x][texture y]
	static BlockType[] blocks=new BlockType[]
	{
		new BlockType("air",		"gas",		new Vector2[0,0],	true,	false),
		new BlockType("stone",		"solid",	texture(0,	0),		false,	false),
		new BlockType("cobble",		"solid",	texture(1,	0),		false,	false),
		new BlockType("grass",		"solid",	texture(2,	0),		false,	false),
		new BlockType("_stone",		"solid",	texture(3,	0),		false,	false),
		new BlockType("wood",		"solid",	texture(4,	0),		false,	false),
		new BlockType("dirt",		"solid",	texture(5,	0),		false,	false),
		new BlockType("iron_ore",	"solid",	texture(6,	0),		false,	false),
		new BlockType("coal_ore",	"solid",	texture(7,	0),		false,	false),
		new BlockType("diamond_ore","solid",	texture(8,	0),		false,	false),
		new BlockType("emerald_ore","solid",	texture(9,	0),		false,	false),
		new BlockType("gold_ore",	"solid",	texture(10,	0),		false,	false),
		new BlockType("tin_ore",	"solid",	texture(11,	0),		false,	false),
		new BlockType("copper_ore",	"solid",	texture(12,	0),		false,	false),
		new BlockType("leaves",		"solid",	texture(13,	0),		false,	false),
	};
	static ItemType[] items=merge();
	static ItemType[] merge()
	{
		ItemType[] onlyItems=new ItemType[]
		{
			new ItemType()
		};
		ItemType[] items=new ItemType[onlyItems.Length+blocks.Length];
		for(int v =0; v<onlyItems.Length;v++)
			items[v]=onlyItems[v];
		for(int v =0; v<blocks.Length;v++)
			items[onlyItems.Length+v]=blocks[v];
		return items;
	}
	public static Vector2[,] texture(Vector2 f0,Vector2 f1,Vector2 f2,Vector2 f3,Vector2 f4,Vector2 f5)
	{
		Vector2 v0=new Vector2(0,0);
		Vector2 v1=new Vector2(0,1);
		Vector2 v2=new Vector2(1,1);
		Vector2 v3=new Vector2(1,0);

		return new Vector2[,]{
			{f0+v0,f0+v1,f0+v2,f0+v3},
			{f1+v0,f1+v1,f1+v2,f1+v3},
			{f2+v0,f2+v1,f2+v2,f2+v3},
			{f3+v0,f3+v1,f3+v2,f3+v3},
			{f4+v0,f4+v1,f4+v2,f4+v3},
			{f5+v0,f5+v1,f5+v2,f5+v3},
		};
	}
	public static Vector2[,] texture(Vector2 s,Vector2 u,Vector2 d)
	{
		return texture(s,s,s,s,u,d);
	}
	public static Vector2[,] texture(Vector2 f)
	{
		return texture(f,f,f,f,f,f);
	}
	public static Vector2[,] texture(float x,float y)
	{
		return texture(new Vector2(x,y));
	}

	public static ItemType ItemById(string id)
	{
		for(int v=0;v<items.Length;v++)
			if(items[v].id==id)
				return items[v];
		return null;
	}

	public static BlockType BlockById(string id)
	{
		for(int v=0;v<blocks.Length;v++)
			if(blocks[v].id==id)
				return blocks[v];
		return null;
	}
	public static readonly string defaultId = "air";
}
