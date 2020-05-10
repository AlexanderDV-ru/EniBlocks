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
    public Image[] slots;
    public Image select;

    // Start is called before the first frame update
    void Start()
    {
        inventory.items=new string[]{"stone","cobble","wood","dirt","grass","air","air","air","air"};
    }

    // Update is called once per frame
    void Update()
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
        for(var v=0;v<inventory.items.Length;v++)
        {
            bool visible=Ids.BlockById(inventory.items[v])!=null&&Ids.BlockById(inventory.items[v]).textures.Length!=0;
            if(visible)
            {
                Debug.Log(Ids.BlockById(inventory.items[v]).textures[0,0]);
                slots[v].rectTransform.localPosition=new Vector3(-Ids.BlockById(inventory.items[v]).textures[0,0].x*100,-Ids.BlockById(inventory.items[v]).textures[0,0].y*100);
                slots[v].rectTransform.sizeDelta =new Vector2(-Ids.BlockById(inventory.items[v]).textures[0,0].x*100+Ids.BlockById(inventory.items[v]).textures[0,2].x*100, -Ids.BlockById(inventory.items[v]).textures[0,0].y*100+Ids.BlockById(inventory.items[v]).textures[0,2].y*100);
            }
            slots[v].gameObject.SetActive(visible);
        }
        select.rectTransform.localPosition=new Vector3(inventory.selectedSlot*50,0,0);

        Ray ray = cam.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, ray.direction, out hit, editRange);
        selector.position=hit.point+ray.direction*0.1f;

        selector.position=selector.position-chunks.transform.position;

        selector.gameObject.SetActive(chunks.GetBlock(selector.position)!=null?chunks.GetBlock(selector.position).textures.Length!=0:false);
        if(Input.GetMouseButtonDown(0))
            chunks.ChangeBlock(selector.position,Ids.BlockById("air"));
        if(Input.GetMouseButtonDown(1))
            chunks.ChangeBlock(selector.position-ray.direction*0.2f,Ids.BlockById(inventory.items[inventory.selectedSlot]));
        if(Input.GetMouseButtonDown(2))
            inventory.items[inventory.selectedSlot]=chunks.GetBlock(selector.position).id;
        cam.transform.Rotate(Input.mouseScrollDelta.y,0,0);
        selector.position=new Vector3(selector.position.x-selector.position.x%1,selector.position.y-selector.position.y%1,selector.position.z-selector.position.z%1)+chunks.transform.position;
    	transform.position+=Input.GetAxis("Vertical")*0.1f*transform.forward;
    	transform.Rotate(0,Input.GetAxis("Horizontal")*1,0);
    	if(Input.GetKeyDown("space"))
    		GetComponent<Rigidbody>().velocity+=transform.up*4;
    }
}
