using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
	public EntityLocation loc;
    public EntityId id;

	protected Entity()
	{

	}

    public static Entity create(EntityId id)
    {
        Entity e=new Entity();
        e.id=id;
        return e;
    }
}
