using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Inventoried<Entity>
{
    public int durability=0;
    public EntityType entityType;
    public string name;
    public virtual EntityClass type{get;set;}

    public static E create<E>(EntityType entityType, string name) where E:Entity
    {
        GameObject go=new GameObject();
        go.name=name;
        go.AddComponent<Inventory<Entity>>();
        E e=go.AddComponent<E>();
        e.entityType=entityType;
        e.name=name;
        return e;
    }
    public static E create<E>(EntityType entityType) where E:Entity
    {
        return Entity.create<E>(entityType,"New Unnamed Entity");
    }
}
