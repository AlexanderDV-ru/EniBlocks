using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	private int xChunks=6,yChunks=6,zChunks=6;
	private int xBlocks=12,yBlocks=12,zBlocks=12;
	private Chunk[,,] chunks;

	public Material material;

	void Start()
	{
		chunks=new Chunk[xChunks,yChunks,zChunks];
		for(int x=0;x<xChunks;x++)
			for(int y=0;y<yChunks;y++)
				for(int z=0;z<zChunks;z++)
				{
					chunks[x,y,z]=new GameObject(x+" "+y+" "+z).AddComponent<Chunk>();
					chunks[x,y,z].transform.parent=transform;
					chunks[x,y,z].transform.position=new Vector3(x*xBlocks,y*yBlocks,z*zBlocks);
					chunks[x,y,z].xBlocks=xBlocks;
					chunks[x,y,z].yBlocks=yBlocks;
					chunks[x,y,z].zBlocks=zBlocks;
					chunks[x,y,z].material=material;
					chunks[x,y,z].init();
				}
		Random gen = new Random();
		for(int x=0;x<xChunks*xBlocks;x++)
			for(int y=0;y<yChunks*yBlocks;y++)
				for(int z=0;z<zChunks*zBlocks;z++)
					SetBlock(new EntityLocation(x,y,z,0,0,0,Random.Range(0,2)>0,Random.Range(0,2)>0,Random.Range(0,2)>0), Entity.create(EntityId.ByName(y==0?"bedrock":(y<32?"stone":(y<36?"dirt":(y<37?"grass":"air"))))));
	}

	public Entity SetBlock(EntityLocation loc, Entity entity)
	{
		//Debug.Log(loc.x+" "+loc.y+" "+loc.z);
		Chunk cur=chunks[(loc.x-loc.x%xBlocks)/xBlocks,(loc.y-loc.y%yBlocks)/yBlocks,(loc.z-loc.z%zBlocks)/zBlocks];
		Entity prev=cur.blocks[loc.x%xBlocks,loc.y%yBlocks,loc.z%zBlocks];
		if(entity!=null)
		{
			cur.blocks[loc.x%xBlocks,loc.y%yBlocks,loc.z%zBlocks]=entity;
			cur.needsUpdateMesh=true;
		}
		return prev;
	}
}
