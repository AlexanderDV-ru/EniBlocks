using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunks : MonoBehaviour
{
	public int chunksWidth=6,chunksHeight=4,chunksDepth=6;
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
					g.GetComponent<Chunk>().width=chunkWidth;
					g.GetComponent<Chunk>().height=chunkHeight;
					g.GetComponent<Chunk>().depth=chunkDepth;
					g.GetComponent<Chunk>().chunks=this;
					g.GetComponent<Chunk>().defBlock=y<2?new string[]{"stone"}:(y==2?new string[]{"dirt"}:new string[]{"grass","air","air","air","air","air","air","air","air","air","air","air","air","air"});
					g.name=x+" "+y+" "+z;
					chunks[x,y,z]=g.GetComponent<Chunk>();
				}
	}
	public Location normalize(Location loc)
	{
		int x=(loc.x%(chunksWidth*chunkWidth)+(chunksWidth*chunkWidth))%(chunksWidth*chunkWidth);
		int y=loc.y;//(loc.y%(chunksHeight*chunkHeight)+(chunksHeight*chunkHeight))%(chunksHeight*chunkHeight);
		int z=(loc.z%(chunksDepth*chunkDepth)+(chunksDepth*chunkDepth))%(chunksDepth*chunkDepth);
		return new Location(x,y,z);
	}
	public Vector3 normalize(Vector3 pos)
	{
		float x=(pos.x%(chunksWidth*chunkWidth)+(chunksWidth*chunkWidth))%(chunksWidth*chunkWidth);
		float y=pos.y;//(pos.y%(chunksHeight*chunkHeight)+(chunksHeight*chunkHeight))%(chunksHeight*chunkHeight);
		float z=(pos.z%(chunksDepth*chunkDepth)+(chunksDepth*chunkDepth))%(chunksDepth*chunkDepth);
		return new Vector3(x,y,z);
	}
	public Block ChangeBlock(Location loc,Block block)
	{
		loc=normalize(loc);
		Chunk c =GetChunk((loc.x-loc.x%chunkWidth)/chunkWidth,(loc.y-loc.y%chunkHeight)/chunkHeight,(loc.z-loc.z%chunkDepth)/chunkDepth);
		if(c)
		{
			c.NearBlockRecalc(new Location(loc.x%chunkWidth,loc.y%chunkHeight,loc.z%chunkDepth));
			c.doUpd=true;
		}
		return c?c.SetBlock(new Location(loc.x%chunkWidth,loc.y%chunkHeight,loc.z%chunkDepth),block):null;
	}
	public Block ChangeBlock(Location loc,ItemClass type)
	{
		return ChangeBlock(loc,Block.create((BlockClass)type));
	}
	public Block ChangeBlock(Location loc,string id)
	{
		return ChangeBlock(loc,ItemClass.ById(id));
	}
	public Block ChangeBlock(Vector3 pos,Block block)
	{
		return ChangeBlock(new Location((int)pos.x,(int)pos.y,(int)pos.z),block);
	}
	public Block ChangeBlock(Vector3 pos,ItemClass type)
	{
		return ChangeBlock(new Location((int)pos.x,(int)pos.y,(int)pos.z),type);
	}
	public Block ChangeBlock(Vector3 pos,string id)
	{
		return ChangeBlock(new Location((int)pos.x,(int)pos.y,(int)pos.z),id);
	}
	public Block ChangeBlock(int x,int y,int z,Block block)
	{
		return ChangeBlock(new Location(x,y,z),block);
	}
	public Block ChangeBlock(int x,int y,int z,ItemClass type)
	{
		return ChangeBlock(new Location(x,y,z),type);
	}
	public Block ChangeBlock(int x,int y,int z,string id)
	{
		return ChangeBlock(new Location(x,y,z),id);
	}

	public Chunk GetChunk(int x,int y,int z)
	{
		if(!testOnIn(new Location(x*chunkWidth,y*chunkHeight,z*chunkDepth)))
			return null;
		return chunks[x,y,z];
	}
	public bool testOnIn(Location loc)
	{
		if(loc.x<0||loc.x>=chunksWidth*chunkWidth)
			return false;
		if(loc.y<0||loc.y>=chunksHeight*chunkHeight)
			return false;
		if(loc.z<0||loc.z>=chunksDepth*chunkDepth)
			return false;
		return true;
	}
	public Block GetBlock(Location loc)
	{
		return ChangeBlock(loc,(Block)null);
	}
	public Block GetBlock(Vector3 pos)
	{
		return ChangeBlock(pos,(Block)null);
	}
	public Block GetBlock(int x,int y,int z)
	{
		return ChangeBlock(x,y,z,(Block)null);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
