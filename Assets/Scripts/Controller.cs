using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Transform selector;
    public Chunks chunks;
    public float editRange=7;
    public Camera cam;
    public PlayerInventory inventory;
    public Image select;
    public InventoriesInteractive interactive;
    public InventoryInteractive hotbar;
    public Ids ids;

    // Start is called before the first frame update
    void Start()
    {
        Ids.ins=ids;
        inventory.items=new Item[]{Item.create("stone"),Item.create("cobble"),Item.create("wood"),Item.create("dirt"),Item.create("grass"),Item.create("air"),Item.create("air"),Item.create("air"),Item.create("air")};
        hotbar.init(inventory);
    }
    void InventoryUpdate()
    {
        //Inventory
        if(Input.GetKeyDown("1")) inventory.selectedSlot=0;
        if(Input.GetKeyDown("2")) inventory.selectedSlot=1;
        if(Input.GetKeyDown("3")) inventory.selectedSlot=2;
        if(Input.GetKeyDown("4")) inventory.selectedSlot=3;
        if(Input.GetKeyDown("5")) inventory.selectedSlot=4;
        if(Input.GetKeyDown("6")) inventory.selectedSlot=5;
        if(Input.GetKeyDown("7")) inventory.selectedSlot=6;
        if(Input.GetKeyDown("8")) inventory.selectedSlot=7;
        if(Input.GetKeyDown("9")) inventory.selectedSlot=8;
        select.rectTransform.localPosition=new Vector3(inventory.selectedSlot*hotbar.slotWidth,0,0);
    }
    void PositionUpdate()
    {
        transform.position+=Input.GetAxis("Vertical")*0.1f*transform.forward;
        transform.Rotate(0,Input.GetAxis("Horizontal")*1,0);
        if(Input.GetKeyDown("space"))
            GetComponent<Rigidbody>().velocity+=transform.up*4;
    }
    // Update is called once per frame
    void Update()
    {
        PositionUpdate();
        InventoryUpdate();
        Ray ray = cam.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, ray.direction, out hit, editRange);
        selector.position=hit.point+ray.direction*0.1f;

        selector.position=selector.position-chunks.transform.position;

        selector.gameObject.SetActive(chunks.GetBlock(selector.position)!=null?chunks.GetBlock(selector.position).type.textures.Length!=0:false);
        if(Input.GetMouseButtonDown(0))
            chunks.ChangeBlock(selector.position,"air");
        if(Input.GetMouseButtonDown(1))
        {
            if(chunks.GetBlock(selector.position)!=null&&chunks.GetBlock(selector.position).inventory!=null)
                interactive.openInventory(chunks.GetBlock(selector.position).inventory);
            else if(inventory.items[inventory.selectedSlot].GetType().Name== "Block")
                chunks.ChangeBlock(selector.position-ray.direction*0.2f,(Block)inventory.items[inventory.selectedSlot]);
        }
        if(Input.GetMouseButtonDown(2))
            if(canCopyBlock)
            {
                Debug.Log(chunks.GetBlock(selector.position).type.id);
                inventory.items[inventory.selectedSlot]=Block.create(chunks.GetBlock(selector.position).type);
            }
        cam.transform.Rotate(Input.mouseScrollDelta.y,0,0);
        selector.position=new Vector3(selector.position.x-selector.position.x%1,selector.position.y-selector.position.y%1,selector.position.z-selector.position.z%1)+chunks.transform.position;

        hotbar.update();
    }
    public bool canCopyBlock=true;
}
