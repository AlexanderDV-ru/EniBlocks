using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    public ItemClass type;

    public static Item create(ItemClass type)
    {
        Item item=Entity.create<Item>(EntityType.Item);
        item.type=type!=null?type:ItemClass.ById("air");
        return item;
    }
    public static Item create(string id)
    {
        return Item.create(ItemClass.ById(id));
    }
}
