using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Limiter
{
	public float value;
	public bool more = true;
	public bool containing=true;

	public static bool check(Limiter limiter, float checking)
    {
		return limiter != null ? ((limiter.containing ? checking == limiter.value : false)||(limiter.more?checking > limiter.value:checking<limiter.value)) : true;
	}
}
[System.Serializable]
public class GenerationElement
{
	public Limiter xMin,xMax,yMin,yMax,zMin,zMax;
	public bool endFor = false;
	public bool rawX = false, rawY = false, rawZ = false;
	public string idName;
}
[System.Serializable]
public class Generation
{
	public float coef = 1, perlinCoefX = 1, perlinCoefY = 1, perlinCoefZ = 1, perlinPlusX = 0, perlinPlusY = 0, perlinPlusZ = 0;
	public bool perlinNoise = false, perlinPlus = false;
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
		for (float xl = 0; xl < world.xChunks * world.xBlocks; xl++)
			for (float yl = 0; yl < world.yChunks * world.yBlocks; yl++)
				for (float zl = 0; zl < world.zChunks * world.zBlocks; zl++)
				{
					float perlinX = xl / (world.xChunks * world.xBlocks) / perlinCoefX - perlinPlusX, perlinZ = zl / (world.zChunks * world.zBlocks) / perlinCoefZ - perlinPlusZ;
					float perlinY = Mathf.PerlinNoise(perlinX, perlinZ) * perlinCoefY + perlinPlusY;
					//Debug.Log(perlinX+" "+perlinZ+" "+perlinY);
					float x = xl, y = yl / coef * (perlinNoise && !perlinPlus ? perlinY : 1) + (perlinNoise && perlinPlus ? perlinY : 0), z = zl;
					foreach (GenerationElement gElem in levels)
					{
						float xc = gElem.rawX ? xl : x, yc = gElem.rawY ? yl : y, zc = gElem.rawZ ? zl : z;
						if (Limiter.check(gElem.xMin, xc) && Limiter.check(gElem.xMax, xc) && Limiter.check(gElem.yMin, yc) && Limiter.check(gElem.yMax, yc) && Limiter.check(gElem.zMin, zc) && Limiter.check(gElem.zMax, zc))
						{
							last = gElem.idName;
							if (gElem.endFor)
								break;
						}
					}
					world.set(new EntityLocation((int)xl, (int)yl, (int)zl, 0, 0, 0, Random.Range(0, 2) > 0, Random.Range(0, 2) > 0, Random.Range(0, 2) > 0), new Entity().recreate(EntityId.ByName(last)));
				}
	}
}
