﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType
{
	//[visible][prozrachniy][gravity] [texture x][texture y]
	static ItemType[] itemTypes=merge();
	static ItemType[] merge()
	{
		ItemType[] onlyitemTypes=new ItemType[]
		{
			new ItemType("debug_tool",Type.Tool,Subtype.Other,texture(15,0),true)
		};
		ItemType[] itemTypes=new ItemType[onlyitemTypes.Length+BlockType.blockTypes.Length];
		for(int v =0; v<onlyitemTypes.Length;v++)
			itemTypes[v]=onlyitemTypes[v];
		for(int v =0; v<BlockType.blockTypes.Length;v++)
			itemTypes[onlyitemTypes.Length+v]=BlockType.blockTypes[v];
		return itemTypes;
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

	public static ItemType ById(string id)
	{
        ItemType type=null;
		for(int v=0;v<itemTypes.Length;v++)
			if(itemTypes[v].id==id)
				type= itemTypes[v];
		return type;
	}
	public static readonly string defaultId = "air";




    public string id;
    public Type type;
    public Subtype subtype;

    public Vector2[,] textures;

    public bool transparent;

    //protected ItemType()
    //{
    //    this.id = ItemType.defaultId;
    //    this.textures = new Vector2[0,0];
    //    this.transparent=false;
    //}

    protected ItemType(string id, Type type, Subtype subtype, Vector2[,] textures, bool transparent)
    {
        this.id = id;
        this.type = type;
        this.subtype = subtype;
        this.textures = textures;
        this.transparent = transparent;
    }
}
