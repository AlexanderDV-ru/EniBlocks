using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Location : EntityLocation
{
    public int x,y,z;

    public Location(int x,int y,int z)
    {
        this.x=x;
        this.y=y;
        this.z=z;
    }
}
