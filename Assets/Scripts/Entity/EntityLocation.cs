using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EntityLocation
{
	public int x,y,z;
	public float rotX,rotY,rotZ;
	public bool mirrorX,mirrorY,mirrorZ;

	public EntityLocation(int x,int y,int z,float rotX,float rotY,float rotZ,bool mirrorX,bool mirrorY,bool mirrorZ)
	{
		this.x=x;
		this.y=y;
		this.z=z;

		this.rotX=rotX;
		this.rotY=rotY;
		this.rotZ=rotZ;

		this.mirrorX=mirrorX;
		this.mirrorY=mirrorY;
		this.mirrorZ=mirrorZ;
	}

	public EntityLocation(int x,int y,int z)
	{
		this.x=x;
		this.y=y;
		this.z=z;

		this.rotX=0;
		this.rotY=0;
		this.rotZ=0;

		this.mirrorX=false;
		this.mirrorY=false;
		this.mirrorZ=false;
	}

	public static bool operator ==(EntityLocation a, EntityLocation b)
	{
		return (a.x == b.x && a.y == b.y && a.z == b.z) && (a.rotX == b.rotX && a.rotY == b.rotY && a.rotZ == b.rotZ) && (a.mirrorX == b.mirrorX && a.mirrorY == b.mirrorY && a.mirrorZ == b.mirrorZ);
	}

	public static bool operator !=(EntityLocation a, EntityLocation b)
	{
		return (a.x != b.x || a.y != b.y || a.z != b.z) || (a.rotX != b.rotX || a.rotY != b.rotY || a.rotZ != b.rotZ) || (a.mirrorX != b.mirrorX || a.mirrorY != b.mirrorY || a.mirrorZ != b.mirrorZ);
	}
}
