using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteractive<E> : MonoBehaviour where E:Entity
{
	public Inventory<E> inventory;
	public Slot[] slots;
	public float slotWidth=50,slotHeight=50;

	void Start()
	{
		if(inventory!=null)
		{
			inventory.init();
			init(inventory);
		}
	}

	public void init(Inventory<E> inventory)
	{
		this.inventory=inventory;
		slots=new Slot[inventory.items.Length];
		for(int v=0;v<slots.Length;v++)
		{
			GameObject go=new GameObject();
			go.name="Slot "+v;
			go.transform.parent=transform;
			go.transform.localPosition=new Vector3(slotWidth*v,0,0);
			slots[v]=go.AddComponent<Slot>();
			slots[v].width=slotWidth;
			slots[v].height=slotHeight;
			slots[v].init();
			slots[v].update(inventory.items[v]);
		}
	}
	public void update()
	{
		//Debug.Log(inventory.items.Length);
		//Debug.Log(slots.Length);
		if(inventory!=null)
			for(int v=0;v<slots.Length;v++)
				slots[v].update(inventory.items[v]);
	}
}
