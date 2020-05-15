using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshCollider meshCollider;

	private Mesh mesh;

	public int xBlocks, yBlocks, zBlocks;

	public bool needsUpdateMesh=true;
	public Entity[,,] blocks;

	public Material material;

	public void init()
	{
		blocks=new Entity[xBlocks,yBlocks,zBlocks];
		for(int x=0;x<xBlocks;x++)
			for(int y=0;y<yBlocks;y++)
				for(int z=0;z<zBlocks;z++)
					blocks[x,y,z]=Entity.create(EntityId.ByName("stone"));//y==0?EntityId.bedrock:(y==8?EntityId.grass:(y<8?EntityId.stone:EntityId.air)));
	}

	void Start()
	{
		meshFilter=GetComponent<MeshFilter>();
		meshRenderer=GetComponent<MeshRenderer>();
		meshCollider=GetComponent<MeshCollider>();

		meshRenderer.materials=new Material[]{material};

		mesh=new Mesh();
	}

	void FixedUpdate()
	{
		if(needsUpdateMesh)
		{
			UpdateMesh();
			needsUpdateMesh=false;
		}
	}
	Vector3[] vs=new Vector3[]
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
	int[,] ts=new int[,]
	{
		{0,1,2,3}, // z-
		{7,6,5,4}, // z+
		{3,7,4,0}, // y-
		{1,5,6,2}, // y+
		{4,5,1,0}, // x-
		{3,2,6,7}, // x+
	};
	Vector3[] dirs=new Vector3[]
	{
		new Vector3(0,0,-1), // z-
		new Vector3(0,0,+1), // z+
		new Vector3(0,-1,0), // y-
		new Vector3(0,+1,0), // y+
		new Vector3(-1,0,0), // x-
		new Vector3(+1,0,0), // x+
	};
	Vector3[] dirs2=new Vector3[]
	{
		new Vector3(0,0,0), // z-
		new Vector3(0,0,+1), // z+
		new Vector3(0,0,0), // y-
		new Vector3(0,+1,0), // y+
		new Vector3(0,0,0), // x-
		new Vector3(+1,0,0), // x+
	};
	Vector2[] us=new Vector2[]
	{
		new Vector2(0,0),
		new Vector2(0,1),
		new Vector2(1,1),
		new Vector2(1,0),
	};
	public int[] vertis=new int[]{0,1,2,3},uvis=new int[]{0,1,2,3},faces=new int[]{0,1,2,3,4,5},trs =new int[]{0,1,2,0,2,3};

	bool testBlockFace(int x,int y,int z,int f)
	{
		x+=f==5?1:(f==4?-1:0);
		y+=f==3?1:(f==2?-1:0);
		z+=f==1?1:(f==0?-1:0);
		if(x<0||x>=xBlocks)
			return true;
		if(y<0||y>=yBlocks)
			return true;
		if(z<0||z>=zBlocks)
			return true;
		return blocks[x,y,z].id.transparent;
	}
	void UpdateMesh()
	{
		Vector3[] verts =   new Vector3[xBlocks*yBlocks*zBlocks*faces.Length*vertis.Length];
		int[]   tris    =   new int[	xBlocks*yBlocks*zBlocks*faces.Length*trs.Length];
		Vector2[] uvs   =   new Vector2[xBlocks*yBlocks*zBlocks*faces.Length*uvis.Length];
		for(int x=0;x<xBlocks;x++)
			for(int y=0;y<yBlocks;y++)
				for(int z=0;z<zBlocks;z++)
					for(int fi=0;fi<faces.Length;fi++)
						if(testBlockFace(x,y,z,fi))
						{
							int addr=((x*yBlocks+y)*zBlocks+z)*faces.Length+fi;
							int f=faces[fi];
							for(int vi=0;vi<vertis.Length;vi++)
								verts[addr*vertis.Length+vi]=new Vector3(x,y,z)+vs[ts[f,vertis[vi]]];
							for(int ti=0;ti<trs.Length;ti++)
								tris[addr*trs.Length+ti]	=addr*vertis.Length+trs[ti];
							for(int ui=0;ui<uvis.Length;ui++)
								uvs[addr*uvis.Length+ui]	=blocks[x,y,z].id.textures[blocks[x,y,z].id.faces[f]]+us[ui];
						}
		mesh.vertices=verts;
		mesh.triangles=tris;
		mesh.uv=uvs;
		mesh.RecalculateNormals();

		meshFilter.mesh=mesh;
		meshCollider.sharedMesh=mesh;
	}
}
