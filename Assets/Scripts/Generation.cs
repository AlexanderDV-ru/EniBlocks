using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Limiter
{
	public int value;
	public bool containing=true;

	public static bool check(Limiter limiter, int checking)
    {
		return limiter != null ? (limiter.containing ? checking >= limiter.value : checking > limiter.value) : true;
	}
}
[System.Serializable]
public class GenerationElement
{
	public Limiter xMin,xMax,yMin,yMax,zMin,zMax;
	public string idName;
}
[System.Serializable]
public class Generation
{
    public float coef = 1;
    public GenerationElement[] levels;

	private static Generation generationFromConfig(string path)
	{
		StreamReader reader = new StreamReader(path);
		string cfg = reader.ReadToEnd();
		reader.Close();
		
		return JsonUtility.FromJson<Generation>(cfg);
	}
	public static readonly Generation generation = generationFromConfig("Assets/Properties/generation.json");

	public void perform(World world)
    {
		string last = "air";
		for (int x = 0; x < world.xChunks * world.xBlocks; x++)
			for (int y = 0; y < world.yChunks * world.yBlocks; y++)
				for (int z = 0; z < world.zChunks * world.zBlocks; z++)
				{
					foreach (GenerationElement gElem in levels)
						if (Limiter.check(gElem.xMin, x) && Limiter.check(gElem.xMax, x) && Limiter.check(gElem.yMin, y) && Limiter.check(gElem.yMax, y) && Limiter.check(gElem.zMin, z) && Limiter.check(gElem.zMax, z))
							last = gElem.idName;
					world.set(new EntityLocation(x, y, z, 0, 0, 0, Random.Range(0, 2) > 0, Random.Range(0, 2) > 0, Random.Range(0, 2) > 0), Entity.create(EntityId.ByName(last)));
				}
	}
}
