using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ids : MonoBehaviour
{
	public static Ids ins;

	public Material terrainMat;
	public Material inventoryMat;
	public Sprite slotMaskSprite;
	public int xTexturesCount=16,yTexturesCount=16,xTextureResolution=32,yTextureResolution=32;
	void Start()
	{
		ins=this;
	}
}
