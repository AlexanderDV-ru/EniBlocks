using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClass : ItemClass
{
	public static readonly Vector3[] vertices=new Vector3[]
	{
		new Vector3(0,0,0),
		new Vector3(0,1,0),
		new Vector3(1,1,0),
		new Vector3(1,0,0),
		new Vector3(0,0,1),
		new Vector3(0,1,1),
		new Vector3(1,1,1),
		new Vector3(1,0,1)
	};
	public const int dimensions=3, facesCount=6, indexesInFace=6, verticesInFace=4;
	public static readonly int[,] verticesIndexesOfFaces=new int[facesCount,verticesInFace]
	{
		{0,1,2,3}, // z-
		{7,6,5,4}, // z+
		{3,7,4,0}, // y-
		{1,5,6,2}, // y+
		{4,5,1,0}, // x-
		{3,2,6,7}, // x+
	};
	public static readonly int[,] directionsOfFaces=new int[facesCount,dimensions]
	{
		{ 0, 0,-1}, // z-
		{ 0, 0,+1}, // z+
		{ 0,-1, 0}, // y-
		{ 0,+1, 0}, // y+
		{-1, 0, 0}, // x-
		{+1, 0, 0}, // x+
	};
	private static BlockClass[] bts;
	public static BlockClass[] blockTypes{
		get{
			return bts==null?bts=new BlockClass[]
			{
				new BlockClass("air",		Subtype.Gas,	new Vector2[0,0],	true,	false),
				new BlockClass("stone",		Subtype.Solid,	texture(0,	0),		false,	false),
				new BlockClass("cobble",		Subtype.Solid,	texture(1,	0),		false,	false),
				new BlockClass("grass",		Subtype.Solid,	texture(2,	0),		false,	false),
				new BlockClass("_stone",		Subtype.Solid,	texture(3,	0),		false,	false),
				new BlockClass("wood",		Subtype.Solid,	texture(4,	0),		false,	false),
				new BlockClass("dirt",		Subtype.Solid,	texture(5,	0),		false,	false),
				new BlockClass("iron_ore",	Subtype.Solid,	texture(6,	0),		false,	false),
				new BlockClass("coal_ore",	Subtype.Solid,	texture(7,	0),		false,	false),
				new BlockClass("diamond_ore",Subtype.Solid,	texture(8,	0),		false,	false),
				new BlockClass("emerald_ore",Subtype.Solid,	texture(9,	0),		false,	false),
				new BlockClass("gold_ore",	Subtype.Solid,	texture(10,	0),		false,	false),
				new BlockClass("tin_ore",	Subtype.Solid,	texture(11,	0),		false,	false),
				new BlockClass("copper_ore",	Subtype.Solid,	texture(12,	0),		false,	false),
				new BlockClass("leaves",		Subtype.Solid,	texture(13,	0),		false,	false),
			}:bts;
		}
	}
	public static new BlockClass ById(string id)
	{
		BlockClass type=null;
		for(int v=0;v<blockTypes.Length;v++)
			if(blockTypes[v].id==id)
				type= blockTypes[v];
		return type;
	}

	public bool hasGravity;

	//protected BlockClass()  :   base()
	//{
	//    this.hasGravity=false;
	//}

	protected BlockClass(string id, Subtype subtype, Vector2[,] textures, bool transparent, bool hasGravity)  :   base(id,subtype,textures,transparent)
	{
		this.hasGravity = hasGravity;
	}
}
