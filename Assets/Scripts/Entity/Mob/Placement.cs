using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
	public Controller controller;
	public Transform selector,breakSelector,placeSelector;
	public float editRange=7;
	public InventoriesInteractive<Entity> interactive;
	public InventoryInteractive<Entity> hotbar;
	public Image select;
	public void AtFixedUpdate()
	{
		for(int sl=0;sl<9;sl++)
			if(Input.GetAxis("Slot"+sl)>0)
				controller.entity.inventory.selectedSlot=sl;
		Debug.Log(select);
		Debug.Log(controller);
		Debug.Log(controller.entity);
		Debug.Log(controller.entity.inventory);
		select.rectTransform.localPosition=new Vector3(controller.entity.inventory.selectedSlot*hotbar.slotWidth,0,0);

		Ray ray = controller.movement.cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Vector3 castCamPos=controller.chunks.normalize(controller.movement.cam.transform.position+ray.direction*editRange)-ray.direction*editRange;
		Physics.Raycast(castCamPos, ray.direction, out hit, editRange);
		selector.position=hit.point-castCamPos+controller.movement.cam.transform.position;

		selector.position=selector.position-controller.chunks.transform.position;

		breakSelector.position=selector.position+ray.direction*0.1f;

		placeSelector.position=breakSelector.position-ray.direction*0.1f*2;

		Vector3 breakPosition=/*chunks.normalize*/(breakSelector.position);
		Vector3 placePosition=/*chunks.normalize*/(placeSelector.position);
		bool act=controller.chunks.GetBlock(breakPosition)!=null?controller.chunks.GetBlock(breakPosition).type.textures.Length!=0:false;
		selector.gameObject.SetActive(act);
		breakSelector.gameObject.SetActive(act);
		placeSelector.gameObject.SetActive(act);
		if(Input.GetAxis("Break")>0&&controller.perms.breaking)
			controller.chunks.ChangeBlock(breakPosition,"air");
		if(Input.GetAxis("Place")>0&&controller.perms.place)
		{
			Debug.Log(1);
			if(controller.chunks.GetBlock(breakPosition)!=null&&controller.chunks.GetBlock(breakPosition).inventory!=null)
				interactive.openInventory(controller.chunks.GetBlock(breakPosition).inventory);
			else if(controller.entity.inventory.items[controller.entity.inventory.selectedSlot].GetType().Name== "Block")
				controller.chunks.ChangeBlock(placePosition,(Block)controller.entity.inventory.items[controller.entity.inventory.selectedSlot]);
		}
		if(Input.GetAxis("Copy")>0&&controller.perms.copy)
		{
			Debug.Log(controller.chunks.GetBlock(breakPosition).type.id);
			controller.entity.inventory.items[controller.entity.inventory.selectedSlot]=Block.create(controller.chunks.GetBlock(breakPosition).type);
		}

		selector.position=new Vector3(selector.position.x-selector.position.x%1,selector.position.y-selector.position.y%1,selector.position.z-selector.position.z%1)+controller.chunks.transform.position;

		breakSelector.position=new Vector3(breakSelector.position.x-breakSelector.position.x%1,breakSelector.position.y-breakSelector.position.y%1,breakSelector.position.z-breakSelector.position.z%1)+controller.chunks.transform.position;

		placeSelector.position=new Vector3(placeSelector.position.x-placeSelector.position.x%1,placeSelector.position.y-placeSelector.position.y%1,placeSelector.position.z-placeSelector.position.z%1)+controller.chunks.transform.position;

		hotbar.update();
	}
}
