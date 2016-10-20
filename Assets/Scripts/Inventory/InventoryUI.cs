using UnityEngine;
using ItemClasses;
using Assets.Scripts.UI;

public class InventoryUI : MonoBehaviour
{
	//Reference to UI slots and inventory
	InventorySlot[] slots;
	Inventory inventory;

	//Variables used for storing item and slot information when dragging an item
	public bool DraggingItem { get; private set; }
	private DragableSlot draggedFrom;
	public Item draggedItem;
	public DragableSlot hovering;

	void Start() {
		//Assign inventory slots
		slots = this.GetComponentsInChildren<InventorySlot> ();
		//Find a reference to the inventory and initialize it
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
		inventory.Initialize (slots.Length);

		//Make sure dragging is set to false in order to allow dragging
		DraggingItem = false;
	}

	void OnGUI () {
		//Display the dragged item's icon at the mouse when dragging something
		if (DraggingItem) {
			GUI.DrawTexture (new Rect (Event.current.mousePosition.x - 25, Event.current.mousePosition.y - 25, 50, 50), 
				draggedItem.icon.texture);
		}
	}

	/// <summary>
	/// Updates the items with an array of new ones.
	/// </summary>
	/// <param name="items">The new items.</param>
	public void UpdateItems(Item[] items) {
		//Do not update items when dragging (avoids duplicating the dragged item's icon)
		if (DraggingItem)
			return;

		//Run through all slots and place the new item in it
		for (int i = 0; i < items.Length; i++) {
			slots [i].Place(items [i]);
		}
	}

	/// <summary>
	/// Called when the user starts dragging an item from a slot
	/// </summary>
	/// <param name="fromSlot">The slot the user dragged from.</param>
	public void StartDrag(DragableSlot fromSlot) {
		//Only react to the drag if there's actually an item in the slot.
		if (fromSlot.Content != null) {
			DraggingItem = true;
			//Store previous content in a local variable
			draggedItem = fromSlot.Content;
			fromSlot.RemoveContent ();
			//And store the slot the user dragged from, in order to place items when ending the drag
			draggedFrom = fromSlot;
		}
	}

	/// <summary>
	/// Called when the user releases an item he has dragged.
	/// </summary>
	public void EndDrag() {
		//If the user has not dragged anything, there's no need to update.
		if (draggedFrom == null)
			return;
		
		DraggingItem = false;
		//If the user releases over an Item slot, try to place it there
		if (hovering != null) {
			//First determine if the slot accepts the item (most relevant to Equipment slots, place the item there if it does.
			if (hovering.DoesAccept(draggedItem.Type)) {
				draggedFrom.Place (hovering.Content);
				hovering.Place (draggedItem);
			}
			//If the slot did not accept the item, place it back where it came from.
			else
				draggedFrom.Place (draggedItem);
		}
		//If the released the drag outside a slot, place the item where it came from.
		else {
			draggedFrom.Place(draggedItem);
		}

		//Update the inventory with said change.
		inventory.UpdateInventory (slots);
	}

	/// <summary>
	/// Called when the user right-clicks an item in the inventory.
	/// </summary>
	/// <param name="slot">The slot the user right-clicked.</param>
	public void UsedItemFromSlot(InventorySlot slot) {
		//Ask inventory what to do when the item has been right-clicked.
		inventory.UseItem (slot);
	}
}

