using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoriesInteractive : MonoBehaviour
{
    [SerializeField]
    private List<InventoryInteractive> interactives;

    void Start()
    {
        interactives=new List<InventoryInteractive>();
    }

    public void openInventory(Inventory inventory)
    {
        InventoryInteractive cur=null;
        int c=0,count=0;
        foreach(InventoryInteractive v in interactives)
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
            cur=go.AddComponent<InventoryInteractive>();
            interactives.Add(cur);
            cur.init(inventory);
        }
        if(inventory.items.Length==0)
            inventory.items=new Item[]{Block.create("dirt")};
        cur.transform.position=new Vector3(0,60*c+60,0);
        cur.init(inventory);
        cur.update();
    }
}
