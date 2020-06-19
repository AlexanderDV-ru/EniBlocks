using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class WorldSave
{
	public string[] blocks;
}
public class World : MonoBehaviour, I3dContainer
{
	public int xChunks { get => xChunksValue; }
	public int yChunks { get => yChunksValue; }
	public int zChunks { get => zChunksValue; }

	public int xChunkBlocks { get; }
	public int yChunkBlocks { get; }
	public int zChunkBlocks { get; }

	public int xBlocks { get => xBlocksValue; }
	public int yBlocks { get => yBlocksValue; }
	public int zBlocks { get => zBlocksValue; }

	public int xBlockSize { get => _xBlockSize; }
	public int yBlockSize { get => _yBlockSize; }
	public int zBlockSize { get => _zBlockSize; }

	public int xSize { get => xBlocks * xChunks; }
	public int ySize { get => yBlocks * yChunks; }
	public int zSize { get => zBlocks * zChunks; }

	private int _xBlockSize = 1, _yBlockSize = 1, _zBlockSize = 1;
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

	public void save(string dir, string file)
	{
		WorldSave ws = new WorldSave();
		ws.blocks = new string[xSize * ySize * zSize];
		for (int x = 0; x < xSize; x++)
			for (int y = 0; y < ySize; y++)
				for (int z = 0; z < zSize; z++)
					ws.blocks[(x * ySize + y) * zSize + z] = set(new EntityLocation(x, y, z)).id.name;
		string json = JsonUtility.ToJson(ws);
		try
		{
			if (!Directory.Exists(dir))
				new DirectoryInfo(dir).Create();
			if (!File.Exists(dir + "/" + file))
				new FileInfo(dir + "/" + file).Create();

			StreamWriter wr = new StreamWriter(dir + "/" + file);
			wr.Write(json);
			wr.Close();
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
	}

	public void load(string dir, string file)
	{
		string json = "{}";
		try
		{
			if (!Directory.Exists(dir))
				new DirectoryInfo(dir).Create();
			if (!File.Exists(dir + "/" + file))
				new FileInfo(dir + "/" + file).Create();

			StreamReader wr = new StreamReader(dir + "/" + file);
			json = wr.ReadToEnd();
			wr.Close();
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
		WorldSave ws = JsonUtility.FromJson<WorldSave>(json);
		for (int x = 0; x < xSize; x++)
			for (int y = 0; y < ySize; y++)
				for (int z = 0; z < zSize; z++)
					set(new EntityLocation(x, y, z), new Entity().recreate(EntityId.ByName(ws.blocks[(x * ySize + y) * zSize + z])));
	}

	public bool fill(Vector3 point, Entity sample)
	{
		set(new EntityLocation((int)point.x, (int)point.y, (int)point.z), new Entity().recreate(sample));
		return true;
	}

	public bool fill(Vector3 center, float radius, Entity sample, string shape = "cube")
	{
		for (float x = center.x - radius; x <= center.x + radius; x++)
			for (float y = center.y - radius; y <= center.y + radius; y++)
				for (float z = center.z - radius; z <= center.z + radius; z++)
				{
					bool isInFormula = false;
					switch (shape)
					{
						case "sphere/hollow":
							isInFormula = (Mathf.Abs(Mathf.Pow(x - center.x, 2) + Mathf.Pow(y - center.y, 2) + Mathf.Pow(z - center.z, 2)) - (Mathf.Pow(radius, 2)) < 1f);
							break;
						case "sphere":
							isInFormula = (Mathf.Abs(Mathf.Pow(x - center.x, 2) + Mathf.Pow(y - center.y, 2) + Mathf.Pow(z - center.z, 2)) <= (Mathf.Pow(radius, 2)));
							break;
						default:
							isInFormula = true;
							break;
					}
					if (isInFormula)
						set(new EntityLocation((int)x, (int)y, (int)z), new Entity().recreate(sample));
				}
		return true;
	}

	public bool fill(Vector3 first, Vector3 second, Entity sample, string shape = "cube")
	{
		for (float x = first.x; x <= second.x; x++)
			for (float y = first.y; y <= second.y; y++)
				for (float z = first.z; z <= second.z; z++)
					set(new EntityLocation((int)x, (int)y, (int)z), new Entity().recreate(sample));
		return true;
	}



	public bool fillInside(Vector3 first, Vector3 second, Entity sample, string shape = "cube") => fill(first + Vector3.one, second - Vector3.one, sample, shape);

	public bool fillArea(Vector3 first, Vector3 second, Entity sample, string shape = "cube") => fill(first, first + second, sample, shape);
}
