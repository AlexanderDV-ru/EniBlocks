using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Permissions))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Placement))]
[RequireComponent(typeof(Inventory<>))]
public class Controller : MonoBehaviour
{
	public Chunks chunks;
    public Entity entity;
	public Permissions perms;
	public Movement movement;
	public Placement placement;
    // Start is called before the first frame update
    void Start()
    {
		entity=Mob.create("human");
		entity.inventory.items=new Item[]{Item.create("stone"),Item.create("cobble"),Item.create("wood"),Item.create("dirt"),Item.create("grass"),Item.create("air"),Item.create("air"),Item.create("air"),Item.create("air")};
		placement.hotbar.init(entity.inventory);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
