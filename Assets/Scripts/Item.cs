using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemType type;
    public int durability=0;
    public Inventory inventory;

    protected Item(ItemType type)
    {
        this.type=type!=null?type:ItemType.ById("air");
    }
    public static Item create(ItemType type)
    {
        if(type.GetType().Name=="BlockType")
            return Block.create((BlockType)type);
        Item item=new Item(type);
        return item;
    }
    public static Item create(string id)
    {
        return create(ItemType.ById(id));
    }
}
