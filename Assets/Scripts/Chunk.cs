using System;
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

	public bool needsUpdateMesh = true;
	public Entity[,,] blocks;

	public Material material;

	public void init()
	{
		blocks = new Entity[xBlocks, yBlocks, zBlocks];
		for (int x = 0; x < xBlocks; x++)
			for (int y = 0; y < yBlocks; y++)
				for (int z = 0; z < zBlocks; z++)
					blocks[x, y, z] = Entity.create(EntityId.ByName("air"));
	}

	void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();

		meshRenderer.materials = new Material[] { material };

		mesh = new Mesh();
	}

	void FixedUpdate()
	{
		if (needsUpdateMesh)
		{
			UpdateMesh();
			needsUpdateMesh = false;
		}
	}
	Vector3[] vs = new Vector3[]
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
	int[,] ts = new int[,]
	{
		{0,1,2,3}, // z-
		{7,6,5,4}, // z+
		{3,7,4,0}, // y-
		{1,5,6,2}, // y+
		{4,5,1,0}, // x-
		{3,2,6,7}, // x+
	};
	Vector3[] dirs = new Vector3[]
	{
		new Vector3(0,0,-1), // z-
		new Vector3(0,0,+1), // z+
		new Vector3(0,-1,0), // y-
		new Vector3(0,+1,0), // y+
		new Vector3(-1,0,0), // x-
		new Vector3(+1,0,0), // x+
	};
	Vector3[] dirs2 = new Vector3[]
	{
		new Vector3(0,0,0), // z-
		new Vector3(0,0,+1), // z+
		new Vector3(0,0,0), // y-
		new Vector3(0,+1,0), // y+
		new Vector3(0,0,0), // x-
		new Vector3(+1,0,0), // x+
	};
	Vector2[] us = new Vector2[]
	{
		new Vector2(0,0),
		new Vector2(0,1),
		new Vector2(1,1),
		new Vector2(1,0),
	};
	public int[] vertis = new int[] { 0, 1, 2, 3 }, uvis = new int[] { 0, 1, 2, 3 }, faces = new int[] { 0, 1, 2, 3, 4, 5 }, trs = new int[] { 0, 1, 2, 0, 2, 3 };

	bool testBlockFace(int x, int y, int z, int f)
	{
		x += f == 5 ? 1 : (f == 4 ? -1 : 0);
		y += f == 3 ? 1 : (f == 2 ? -1 : 0);
		z += f == 1 ? 1 : (f == 0 ? -1 : 0);
		if (x < 0 || x >= xBlocks)
			return true;
		if (y < 0 || y >= yBlocks)
			return true;
		if (z < 0 || z >= zBlocks)
			return true;
		return blocks[x, y, z].id.transparent;
	}
	void UpdateMesh()
	{
		Vector3[] vertices = new Vector3[xBlocks * yBlocks * zBlocks * faces.Length * vertis.Length];
		int[] triangles = new int[xBlocks * yBlocks * zBlocks * faces.Length * trs.Length];
		Vector2[] uv = new Vector2[xBlocks * yBlocks * zBlocks * faces.Length * uvis.Length];
		int count = 0;
		for (int x = 0; x < xBlocks; x++)
			for (int y = 0; y < yBlocks; y++)
				for (int z = 0; z < zBlocks; z++)
				{
					Vector3[] vs = blocks[x, y, z].id.verts != null ? blocks[x, y, z].id.verts : this.vs;
					int[,] ts = blocks[x, y, z].id.tris != null ? blocks[x, y, z].id.tris : this.ts;
					Vector2[] us = blocks[x, y, z].id.uvs != null ? blocks[x, y, z].id.uvs : this.us;
					if (vs.Length == 0 || ts.Length == 0 || us.Length == 0)
						continue;
					for (int fi = 0; fi < faces.Length; fi++)
						if (testBlockFace(x, y, z, fi))
						{
							int addr = true ? count : ((x * yBlocks + y) * zBlocks + z) * faces.Length + fi;
							int f = faces[fi];
							for (int vi = 0; vi < vertis.Length; vi++)
								vertices[addr * vertis.Length + vi] = new Vector3(x, y, z) + vs[ts[f, vertis[vi]]];
							for (int ti = 0; ti < trs.Length; ti++)
								triangles[addr * trs.Length + ti] = addr * vertis.Length + trs[ti];
							for (int ui = 0; ui < uvis.Length; ui++)
								uv[addr * uvis.Length + ui] = blocks[x, y, z].id.textures[blocks[x, y, z].id.faces[f]] + us[ui];
							count++;
						}
				}
		Array.Resize(ref vertices, count * vertis.Length);
		Array.Resize(ref triangles, count * trs.Length);
		Array.Resize(ref uv, count * uvis.Length);
		//Vector3[] zipedVertices=new Vector3[count*vertis.Length];
		//int[] zipedTriangles=new int[count*trs.Length];
		//Vector2[] zipedUv=new Vector2[count*uvis.Length];
		//for(var v=0;v<count;v++)
		//{
		//	for(int vi=0;vi<vertis.Length;vi++)
		//		zipedVertices[v*vertis.Length+vi]=vertices[v*vertis.Length+vi];
		//	for(int ti=0;ti<trs.Length;ti++)
		//		zipedTriangles[v*trs.Length+ti]	=triangles[v*trs.Length+ti];
		//	for(int ui=0;ui<uvis.Length;ui++)
		//		zipedUv[v*uvis.Length+ui]	=uv[v*uvis.Length+ui]
		//}
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.RecalculateNormals();

		meshFilter.mesh = mesh;
		meshCollider.sharedMesh = mesh;
	}
}
