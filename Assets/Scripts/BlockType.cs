using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockType : ItemType
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
	public static BlockType[] blockTypes=new BlockType[]
	{
		new BlockType("air",		Subtype.Gas,	new Vector2[0,0],	true,	false),
		new BlockType("stone",		Subtype.Solid,	texture(0,	0),		false,	false),
		new BlockType("cobble",		Subtype.Solid,	texture(1,	0),		false,	false),
		new BlockType("grass",		Subtype.Solid,	texture(2,	0),		false,	false),
		new BlockType("_stone",		Subtype.Solid,	texture(3,	0),		false,	false),
		new BlockType("wood",		Subtype.Solid,	texture(4,	0),		false,	false),
		new BlockType("dirt",		Subtype.Solid,	texture(5,	0),		false,	false),
		new BlockType("iron_ore",	Subtype.Solid,	texture(6,	0),		false,	false),
		new BlockType("coal_ore",	Subtype.Solid,	texture(7,	0),		false,	false),
		new BlockType("diamond_ore",Subtype.Solid,	texture(8,	0),		false,	false),
		new BlockType("emerald_ore",Subtype.Solid,	texture(9,	0),		false,	false),
		new BlockType("gold_ore",	Subtype.Solid,	texture(10,	0),		false,	false),
		new BlockType("tin_ore",	Subtype.Solid,	texture(11,	0),		false,	false),
		new BlockType("copper_ore",	Subtype.Solid,	texture(12,	0),		false,	false),
		new BlockType("leaves",		Subtype.Solid,	texture(13,	0),		false,	false),
	};
	public static new BlockType ById(string id)
	{
		BlockType type=null;
		for(int v=0;v<blockTypes.Length;v++)
			if(blockTypes[v].id==id)
				type= blockTypes[v];
		return type;
	}

	public bool hasGravity;

	//protected BlockType()  :   base()
	//{
	//    this.hasGravity=false;
	//}

	protected BlockType(string id, Subtype subtype, Vector2[,] textures, bool transparent, bool hasGravity)  :   base(id,Type.Placeble,subtype,textures,transparent)
	{
		this.hasGravity = hasGravity;
	}
}
