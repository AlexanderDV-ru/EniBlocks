using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I3dContainer : IVoxelContainer
{
	int xChunks { get; }
	int yChunks { get; }
	int zChunks { get; }

	int xChunkBlocks { get; }
	int yChunkBlocks { get; }
	int zChunkBlocks { get; }

	int xBlocks { get; }
	int yBlocks { get; }
	int zBlocks { get; }

	int xBlockSize { get; }
	int yBlockSize { get; }
	int zBlockSize { get; }

	int xSize { get; }
	int ySize { get; }
	int zSize { get; }

	bool fill(Vector3 point, Entity sample);

	bool fill(Vector3 center, float radius, Entity sample, string shape = "cube");

	bool fill(Vector3 first, Vector3 second, Entity sample, string shape = "cube");

	bool fillInside(Vector3 first, Vector3 second, Entity sample, string shape = "cube");

	bool fillArea(Vector3 first, Vector3 second, Entity sample, string shape = "cube");
}
