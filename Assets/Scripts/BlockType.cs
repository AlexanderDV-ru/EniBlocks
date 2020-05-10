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

    public bool hasGravity;

    public BlockType()  :   base()
    {
        this.hasGravity=false;
    }

    public BlockType(string id, string type, Vector2[,] textures, bool transparent, bool hasGravity)  :   base(id,type,textures,transparent)
    {
        this.hasGravity = hasGravity;
    }
}
