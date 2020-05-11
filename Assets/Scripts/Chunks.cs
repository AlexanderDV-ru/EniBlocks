using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunks : MonoBehaviour
{
	public int chunksWidth=4,chunksHeight=4,chunksDepth=4;
	Chunk[,,] chunks;
	public int chunkWidth=12,chunkHeight=12,chunkDepth=12;
	public Material mat;
	// Start is called before the first frame update
	void Start()
	{
		chunks=new Chunk[chunksWidth,chunksHeight,chunksDepth];
		Debug.Log(chunks);
		for(int x=0;x<chunksWidth;x++)
			for(int y=0;y<chunksHeight;y++)
				for(int z=0;z<chunksDepth;z++)
				{
					GameObject g=new GameObject();
					g.AddComponent<MeshFilter>();
					g.AddComponent<MeshRenderer>();
					g.AddComponent<MeshCollider>();
					g.AddComponent<Chunk>();
					g.GetComponent<MeshRenderer>().material=mat;
					g.transform.parent=transform;
					g.transform.position=new Vector3(x*chunkWidth,y*chunkHeight,z*chunkDepth);
					g.GetComponent<Chunk>().w=chunkWidth;
					g.GetComponent<Chunk>().h=chunkHeight;
					g.GetComponent<Chunk>().d=chunkDepth;
					g.GetComponent<Chunk>().defBlock=y<2?new string[]{"stone"}:(y==2?new string[]{"dirt"}:new string[]{"grass","air","air","air","air","air","air","air","air","air","air","air","air","air"});
					g.name=x+" "+y+" "+z;
					chunks[x,y,z]=g.GetComponent<Chunk>();
				}
	}
	public Block ChangeBlock(int x,int y,int z,Block block)
	{
		Chunk c =GetChunk((x-x%chunkWidth)/chunkWidth,(y-y%chunkHeight)/chunkHeight,(z-z%chunkDepth)/chunkDepth);
		if(c)
		{
			c.NearBlockRecalc(x%chunkWidth,y%chunkHeight,z%chunkDepth);
			c.doUpd=true;
		}
		return c?c.SetBlock(x%chunkWidth,y%chunkHeight,z%chunkDepth,block):null;
	}
	public Block ChangeBlock(int x,int y,int z,ItemType type)
	{
		return ChangeBlock(x,y,z,Block.create((BlockType)type));
	}
	public Block ChangeBlock(int x,int y,int z,string id)
	{
		return ChangeBlock(x,y,z,ItemType.ById(id));
	}
	public Block ChangeBlock(Vector3 pos,Block block)
	{
		return ChangeBlock((int)pos.x,(int)pos.y,(int)pos.z,block);
	}
	public Block ChangeBlock(Vector3 pos,ItemType type)
	{
		return ChangeBlock((int)pos.x,(int)pos.y,(int)pos.z,type);
	}
	public Block ChangeBlock(Vector3 pos,string id)
	{
		return ChangeBlock((int)pos.x,(int)pos.y,(int)pos.z,id);
	}

	public Chunk GetChunk(int x,int y,int z)
	{
		if(!testOnIn(x,y,z))
			return null;
		return chunks[x,y,z];
	}
	public bool testOnIn(int x,int y,int z)
	{
		if(x<0||x>=chunksWidth)
			return false;
		if(y<0||y>=chunksHeight)
			return false;
		if(z<0||z>=chunksDepth)
			return false;
		return true;
	}
	public Block GetBlock(int x,int y,int z)
	{
		Chunk c =GetChunk((x-x%chunkWidth)/chunkWidth,(y-y%chunkHeight)/chunkHeight,(z-z%chunkDepth)/chunkDepth);
		if(c)
		{
			c.NearBlockRecalc(x%chunkWidth,y%chunkHeight,z%chunkDepth);
			c.doUpd=true;
		}
		return c?c.GetBlock(x%chunkWidth,y%chunkHeight,z%chunkDepth):null;
	}
	public Block GetBlock(Vector3 pos)
	{
		return GetBlock((int)pos.x,(int)pos.y,(int)pos.z);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
