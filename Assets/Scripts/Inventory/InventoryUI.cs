using System;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	InventorySlot[] slots;
	bool showGUI;
	Inventory inventory;

	public bool DraggingItem { get; private set; }
	private InventorySlot draggedFrom;
	private Item draggedItem;
	public InventorySlot hovering;

	void Start() {
		//Assign inventory slots
		slots = this.GetComponentsInChildren<InventorySlot> ();
		inventory = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ();
		inventory.Initialize (slots.Length);

		gameObject.SetActive(false);
		showGUI = false;
	}

	void OnGUI () {
		if (DraggingItem) {
			GUI.DrawTexture (new Rect (Event.current.mousePosition.x - 25, Event.current.mousePosition.y - 25, 50, 50), draggedItem.icon.texture);
		}
	}
	public void UpdateItems(Item[] items) {
		if (DraggingItem)
			return;
		
		if (items.Length != slots.Length)
			Debug.LogAssertion ("UI and inventory are not of the same size!");
		for (int i = 0; i < items.Length; i++) {
			slots [i].PlaceItem(items [i]);
		}
	}

	public void ToggleGUI() {
		showGUI = !showGUI;
		gameObject.SetActive(showGUI);
	}

	public void StartDrag(InventorySlot fromSlot) {
		DraggingItem = true;
		draggedItem = fromSlot.Item;
		fromSlot.RemoveItem ();
		draggedFrom = fromSlot;
	}

	public void EndDrag() {
		DraggingItem = false;
		if (hovering != null) {
			draggedFrom.PlaceItem (hovering.Item);
			hovering.PlaceItem (draggedItem);
		} else {
			draggedFrom.PlaceItem (draggedItem);
		}
		inventory.UpdateInventory (slots);
	}

}

