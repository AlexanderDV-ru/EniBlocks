using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public BlockClass type;

    public static Block create(BlockClass type)
    {
        Block e=Entity.create<Block>(EntityType.Block);
        e.type=type!=null?type:BlockClass.ById("air");
        return e;
    }
    public static Block create(string id)
    {
        return Block.create(BlockClass.ById(id));
    }
}
