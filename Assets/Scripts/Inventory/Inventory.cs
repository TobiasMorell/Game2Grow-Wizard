using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	#region Variables
	//Actual inventory
	private Item[] inventory;
	private ItemDatabase database;
	[SerializeField] private InventoryUI UI;

	#endregion
	#region Unity methods
	// Use this for initialization
	void Start () {
		database = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<ItemDatabase> ();
		if (!database)
			Debug.LogAssertion ("FATAL ERROR: Could not find item database!");

		inventory = new Item[25];

		AddItem ("Health Potion");
		AddItem ("Health Potion");
		AddItem (1);
		AddItem ("Novice Sword");
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.I))
			UI.ToggleGUI ();
		UI.UpdateItems (inventory);
	}
	#endregion
	#region Inventory manipulaiton
	public void AddItem(int id) {
		Item item = database [id];
		if (item != null)
			AddItem (item);
		else
			throw new ArgumentException("The item with id " + id + " does not exist");
	}
	public void AddItem(string name) {
		Item item = database [name];
		if (item != null)
			AddItem (item);
		else
			throw new ArgumentException ("The item with name " + name + " does not exist");
	}
	private void AddItem(Item item) {
		int availIndex = inventory.Length;
		//Run through inventory to find an available slot
		for (int index = 0; index < inventory.Length; index++) {
			//Stores the first available position in inventory
			if (inventory [index]== null && availIndex == inventory.Length) {
				availIndex = index;
				//Break if the item is not stackable
				if (item.maxStackSize == 1)
					break;
			}
			//Check if the item in slot is equal to the stackable item at hand
			else if (item.maxStackSize > 1 && inventory[index] != null) {
				if (inventory[index].Id == item.Id) {
					inventory [index].stackSize++;
					return;
				}
			}
		}
		//Place the item in slot unless inventory is full (only called with non-stackable items
		if (availIndex != inventory.Length)
			inventory [availIndex] = item.Clone ();
		else
			Debug.Log ("Inventory is full");
	}

	public bool Contains(int id) {
		Item item = database [id];
		if (item != null)
			return contains (item);
		else
			throw new ArgumentException(string.Format("An item with id {0} does not exist.", id)); 
	}
	public bool Contains(string name) {
		Item item = database [name];
		if (item != null)
			return contains (item);
		else
			throw new ArgumentException (string.Format ("An item with name {0} does not exist.", name));
	}
	private bool contains(Item item) {
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory [i].Id == item.Id)
				return true;
		}

		return false;
	}

	public void RemoveItem(int id) {
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i].Id == id) {
				if (inventory [i].stackSize > 1)
					inventory [i].stackSize--;
				else
					inventory [i] = null;
				break;
			}
		}
	}
	public void RemoveItem(string name) {
		Item item = database [name];
		RemoveItem (item.Id);
	}
	public void RemoveItemAt(int index) {
		inventory [index] = null;
	}
	#endregion
	public void Initialize(int size) {
		inventory = new Item[size];
	}
	public void UpdateInventory(InventorySlot[] slots) {
		if (slots.Length != inventory.Length)
			Debug.LogAssertion ("Inventory and UI are not of the same size!");

		for (int i = 0; i < slots.Length; i++) {
			inventory [i] = slots [i].Item;
		}
	}
	/*void OnGUI() {
		if (!showGUI)
			return;

		drawInventory ();

		if (draggingItem) {
			GUI.DrawTexture (new Rect (Event.current.mousePosition.x - 0.5f *draggedIconWidth, 
				Event.current.mousePosition.y - 0.5f * draggedIconWidth,
				draggedIconWidth, draggedIconWidth), draggedItem.icon);
		}
	}*/

	/*void handleDragAndDrop(Rect position, Event e, int index) {
		if (position.Contains (e.mousePosition)) {
			if (draggingItem && Input.GetMouseButtonUp(0)) {
				slots [draggedFrom].PlaceItem(slots [index].Item);
				slots [index].PlaceItem(draggedItem);
				draggingItem = false;
			}
		}
	}*/
}
