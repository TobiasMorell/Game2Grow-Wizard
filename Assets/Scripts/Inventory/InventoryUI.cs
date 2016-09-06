using System;
using UnityEngine;
using ItemClasses;
using Assets.Scripts.UI;

public class InventoryUI : MonoBehaviour
{
	InventorySlot[] slots;
	Inventory inventory;

	public bool DraggingItem { get; private set; }
	private DragableSlot draggedFrom;
	public Item draggedItem;
	public DragableSlot hovering;

	EquipmentUI equipment;

	void Start() {
		//Assign inventory slots
		slots = this.GetComponentsInChildren<InventorySlot> ();
		inventory = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ();
		inventory.Initialize (slots.Length);

		DraggingItem = false;
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
			slots [i].Place(items [i]);
		}
	}

	public void StartDrag(DragableSlot fromSlot) {
		if (fromSlot.Content != null) {
			DraggingItem = true;
			draggedItem = fromSlot.Content;
			fromSlot.RemoveContent ();
			draggedFrom = fromSlot;
		}
	}

	public void EndDrag() {
		if (draggedFrom == null)
			return;
		
		DraggingItem = false;
		if (hovering != null) {
			if (!(hovering is EquipmentSlot) || (hovering as EquipmentSlot).slotType == draggedItem.Type) {
				draggedFrom.Place (hovering.Content);
				hovering.Place (draggedItem);
			} else
				draggedFrom.Place (draggedItem);
		} else {
			draggedFrom.Place(draggedItem);
		}
		inventory.UpdateInventory (slots);
	}

	public void CancelDrag() {
		DraggingItem = false;
		if (hovering != null) {
			draggedFrom.Place (draggedItem);
		}
		inventory.UpdateInventory (slots);
	}

	public void UsedItemFromSlot(InventorySlot slot) {
		inventory.UseItem (slot);
	}
}

