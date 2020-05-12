using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoriesInteractive<E> : MonoBehaviour where E:Entity
{
    [SerializeField]
    private List<InventoryInteractive<E>> interactives;

    void Start()
    {
        interactives=new List<InventoryInteractive<E>>();
    }

    public void openInventory(Inventory<E> inventory)
    {
        InventoryInteractive<E> cur=null;
        int c=0,count=0;
        foreach(InventoryInteractive<E> v in interactives)
        {
            if(v.inventory==inventory)
            {
                c=count;
                cur=v;
            }
            count++;
        }
        if(cur==null)
        {
            c=interactives.Count;
            GameObject go=new GameObject();
            go.transform.parent=transform;
            cur=go.AddComponent<InventoryInteractive<E>>();
            interactives.Add(cur);
            cur.init(inventory);
        }
        cur.transform.position=new Vector3(0,60*c+60,0);
        cur.init(inventory);
        cur.update();
    }
}
