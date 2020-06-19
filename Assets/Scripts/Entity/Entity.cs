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

	public Entity()
	{

	}

	public Entity recreate(string idName) => this.recreate(EntityId.ByName(idName));

	public Entity recreate(EntityId id)
	{
		this.id = id;
		this.hits = id.hits;
		if (container != null)
			paste(container, location);
		return this;
	}

	public Entity recreate(Entity sample)
	{
		this.id = sample.id;
		this.hits = sample.hits;
		if (container != null)
			paste(container, location);
		return this;
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

	public bool interact(Mob mob, string type)
	{
		EntityId tool = EntityId.ByName(mob.hotbar.items[mob.hotbar.selected]);
		if (typeof(I3dContainer).IsInstanceOfType(container))
		{
			I3dContainer container = (I3dContainer)this.container;
			switch (type)
			{
				case "mine":
					hits -= mob.minemode == "momental" ? hits : Time.fixedDeltaTime * mob.breakSpeed;
					if (hits <= 0)
					{
						if (!mob.perms.canIntoNothing)
						{
							if (mob.touchmode == "silk")
								mob.hotbar.addItem(id.name);
							if (mob.touchmode == "mine")
								mob.hotbar.addItem(id.drop);
						}
						this.recreate(EntityId.ByName("air"));
						return true;
					}
					break;
				default:
					Debug.Log("interact");
					switch (id.name)
					{
						case "tnt":
							if(tool.name=="fire")
								return ignite();
							break;
					}
					break;
			}
		}
		else Debug.Log("re");
		return false;
	}
	public bool ignite()
	{
		if (typeof(I3dContainer).IsInstanceOfType(container))
		{
			I3dContainer container = (I3dContainer)this.container;
			string idName=id.name;
			container.set(location, new Entity().recreate(EntityId.ByName("air")));
			switch (idName)
			{
				case "tnt":
					container.fill(new Vector3(location.x, location.y, location.z), 3, new Entity().recreate(EntityId.ByName("air")),"sphere","ignite;set");
					return true;
			}
		}
		return false;
	}
}
