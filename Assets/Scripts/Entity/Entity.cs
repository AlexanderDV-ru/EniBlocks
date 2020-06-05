using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
	public EntityLocation loc;
    public EntityId id;
	public float hits;

	protected Entity()
	{

	}

    public static Entity create(EntityId id)
    {
        Entity e=new Entity();
        e.id=id;
		e.hits=id.hits;
        return e;
    }
}
