using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class World : MonoBehaviour, IEntityContainer
{
	public int xChunks { get => xChunksValue; }
	public int yChunks { get => yChunksValue; }
	public int zChunks { get => zChunksValue; }
	public int xBlocks { get => xBlocksValue; }
	public int yBlocks { get => yBlocksValue; }
	public int zBlocks { get => zBlocksValue; }
	private int xChunksValue = 6, yChunksValue = 6, zChunksValue = 6;
	private int xBlocksValue = 12, yBlocksValue = 12, zBlocksValue = 12;
	private Chunk[,,] chunks;

	public Material blocksStandard, blocksSprite;

	void Start()
	{
		chunks = new Chunk[xChunks, yChunks, zChunks];
		for (int x = 0; x < xChunks; x++)
			for (int y = 0; y < yChunks; y++)
				for (int z = 0; z < zChunks; z++)
				{
					chunks[x, y, z] = new GameObject(x + " " + y + " " + z).AddComponent<Chunk>();
					chunks[x, y, z].transform.parent = transform;
					chunks[x, y, z].transform.position = new Vector3(x * xBlocks, y * yBlocks, z * zBlocks);
					chunks[x, y, z].xBlocks = xBlocks;
					chunks[x, y, z].yBlocks = yBlocks;
					chunks[x, y, z].zBlocks = zBlocks;
					chunks[x, y, z].material = blocksStandard;
					chunks[x, y, z].init();
				}
		Generation.generation.perform(this);
	}

	public Entity set(EntityLocation loc, Entity entity = null)
	{
		//Debug.Log(loc.x+" "+loc.y+" "+loc.z);
		Chunk cur = chunks[(loc.x - loc.x % xBlocks) / xBlocks, (loc.y - loc.y % yBlocks) / yBlocks, (loc.z - loc.z % zBlocks) / zBlocks];
		Entity prev = cur.blocks[loc.x % xBlocks, loc.y % yBlocks, loc.z % zBlocks];
		if (entity != null)
		{
			cur.blocks[loc.x % xBlocks, loc.y % yBlocks, loc.z % zBlocks] = entity;
			cur.needsUpdateMesh = true;
		}
		if (cur.blocks[loc.x % xBlocks, loc.y % yBlocks, loc.z % zBlocks].location != loc || cur.blocks[loc.x % xBlocks, loc.y % yBlocks, loc.z % zBlocks].container != this)
			cur.blocks[loc.x % xBlocks, loc.y % yBlocks, loc.z % zBlocks].paste(this, loc);
		return prev;
	}
}
