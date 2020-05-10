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
					g.GetComponent<Chunk>().defBlock=y<2?new BlockType[]{Ids.BlockById("stone")}:(y==2?new BlockType[]{Ids.BlockById("dirt")}:new BlockType[]{Ids.BlockById("grass"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air"),Ids.BlockById("air")});
					g.name=x+" "+y+" "+z;
					chunks[x,y,z]=g.GetComponent<Chunk>();
				}
	}
	public BlockType ChangeBlock(int x,int y,int z,BlockType block)
	{
		Chunk c =GetChunk((x-x%chunkWidth)/chunkWidth,(y-y%chunkHeight)/chunkHeight,(z-z%chunkDepth)/chunkDepth);
		if(c)
		{
			c.NearBlockRecalc(x%chunkWidth,y%chunkHeight,z%chunkDepth);
			c.doUpd=true;
		}
		return c?c.SetBlock(x%chunkWidth,y%chunkHeight,z%chunkDepth,block):null;
	}
	public BlockType ChangeBlock(Vector3 pos,BlockType block)
	{
		return ChangeBlock((int)pos.x,(int)pos.y,(int)pos.z,block);
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
	public BlockType GetBlock(int x,int y,int z)
	{
		Chunk c =GetChunk((x-x%chunkWidth)/chunkWidth,(y-y%chunkHeight)/chunkHeight,(z-z%chunkDepth)/chunkDepth);
		if(c)
		{
			c.NearBlockRecalc(x%chunkWidth,y%chunkHeight,z%chunkDepth);
			c.doUpd=true;
		}
		return c?c.GetBlock(x%chunkWidth,y%chunkHeight,z%chunkDepth):null;
	}
	public BlockType GetBlock(Vector3 pos)
	{
		return GetBlock((int)pos.x,(int)pos.y,(int)pos.z);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
