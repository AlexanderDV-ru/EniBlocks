using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Item
{
    public new BlockType type;

    protected Block(BlockType type):base(type)
    {
        this.type=type!=null?type:BlockType.ById("air");
    }
    private static new Item create(ItemType type){return Item.create(type);}
    public static Block create(BlockType type)
    {
        Block item=new Block(type);
        return item;
    }
    public static new Block create(string id)
    {
        return create(BlockType.ById(id));
    }
}
