using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
	public IEntityContainer container
	{
		get;
		private set;
	}
	public EntityLocation location
	{
		get;
		private set;
	}
	public EntityId id;
	public float hits;

	protected Entity()
	{

	}

	public static Entity create(EntityId id)
	{
		Entity e = new Entity();
		e.recreate(id);
		return e;
	}

	public void recreate(EntityId id)
	{
		this.id = id;
		this.hits = id.hits;
		if (container != null)
			paste(container, location);
	}

	public void paste(IEntityContainer container, EntityLocation location)
	{
		this.container = container;
		this.location = location;
		container.set(location, this);
	}

	public void paste(IEntityContainer container)
	{
		this.container = container;
		container.set(location, this);
	}

	public void paste(EntityLocation location)
	{
		this.location = location;
		container.set(location, this);
	}

	public bool interact(EntityId tool, string touchmode, string minemode, float breakSpeed, Inventory hotbar, Permissions perms)
	{
		hits -= minemode == "momental" ? hits : Time.fixedDeltaTime * breakSpeed;
		if (hits <= 0)
		{
			if (!perms.canIntoNothing)
			{
				if (touchmode == "silk")
					hotbar.addItem(id.name);
				if (touchmode == "mine")
					hotbar.addItem(id.drop);
			}
			this.recreate(EntityId.ByName("air"));
			return true;
		}
		return false;
	}

	public bool interact(EntityId tool)
	{
		return false;
	}
}
