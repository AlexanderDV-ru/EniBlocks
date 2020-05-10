using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType
{
    public string id, type;

    public Vector2[,] textures;

    public bool transparent;

    public ItemType()
    {
        this.id = Ids.defaultId;
        this.textures = new Vector2[0,0];
        this.transparent=false;
    }

    public ItemType(string id, string type, Vector2[,] textures, bool transparent)
    {
        this.id = id;
        this.type = type;
        this.textures = textures;
        this.transparent = transparent;
    }
}
