using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
	public World world;

	public RectTransform inventoryUiTransform;
	public Inventory inventory;
	private Image[] slots;
	private Image inventorySelector;

	public Material slotMat;
	public Vector2 slotSize;

	public Material inventorySelectorMat;
	public Vector2 inventorySelectorOffset;
	public float downOffset = 0.65f, blockMultiplier = 0.8f, otherMultiplier = 0.6f;
	// Start is called before the first frame update
	void Start()
	{
		inventoryUiTransform.sizeDelta = new Vector2(slotSize.x * inventory.items.Length, slotSize.y * 2);
		inventoryUiTransform.position = new Vector2(inventoryUiTransform.position.x, inventoryUiTransform.position.y + slotSize.y * downOffset * 3);
		slots = new Image[inventory.items.Length];
		for (int v = 0; v < inventory.items.Length; v++)
		{
			Image slot = new GameObject().AddComponent<Image>();
			slot.gameObject.name = "Slot " + v;
			slot.rectTransform.SetParent(inventoryUiTransform);
			slot.material = slotMat;
			slot.rectTransform.sizeDelta = slotSize;
			slot.rectTransform.localPosition = new Vector3((v - inventory.items.Length / 2) * slotSize.x, 0, 0);
			slots[v] = new GameObject().AddComponent<Image>();
			slots[v].rectTransform.SetParent(slot.rectTransform);
			slots[v].material = new Material(world.blocksSprite);
		}
		inventorySelector = new GameObject().AddComponent<Image>();
		inventorySelector.gameObject.name = "Slot Selector";
		inventorySelector.rectTransform.SetParent(inventoryUiTransform);
		inventorySelector.material = inventorySelectorMat;
		inventorySelector.rectTransform.sizeDelta = slotSize + inventorySelectorOffset;
	}

	// Update is called once per frame
	void Update()
	{

	}
	public void InventoryFU()
	{
		for (int v = 0; v < inventory.items.Length; v++)
		{
			slots[v].material.SetTextureOffset("_MainTex", EntityId.ByName(inventory.items[v]).textures[0] * slots[v].material.GetTextureScale("_MainTex"));
			slots[v].rectTransform.localPosition = new Vector3(0, 0, 0);
			slots[v].rectTransform.sizeDelta = slotSize * (EntityId.ByName(inventory.items[v]).type == "block" ? blockMultiplier : otherMultiplier);
		}

		inventorySelector.rectTransform.localPosition = new Vector3((inventory.selected - inventory.items.Length / 2) * slotSize.x, 0, 0);
	}
}
