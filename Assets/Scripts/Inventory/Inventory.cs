using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;
using ItemClasses;

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
		if (database == null)
			Debug.LogAssertion ("FATAL ERROR: Could not find item database!");

		AddItem ("Novice Sword");
		AddItem ("Walking Stick");
		AddItem ("Health Potion", 3);
		AddItem ("Mana Potion", 3);
		AddItem ("Small Crystal");
	}

	void Update() {
		UI.UpdateItems (inventory);
	}
	#endregion
	#region Inventory manipulaiton
	public void AddItem(int id) {
		AddItem (id, 1);
	}
	public void AddItem(string name) {
		AddItem (name, 1);
	}
	private void AddItem(Item item, int quantity) {
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
					inventory [index].stackSize += quantity;
					return;
				}
			}
		}
		//Place the item in slot unless inventory is full (only called with non-stackable items
		if (availIndex != inventory.Length) {
			inventory [availIndex] = item.Clone ();
			if (quantity > 1)
				inventory [availIndex].stackSize = quantity;
		}
		else
			Debug.Log ("Inventory is full");
	}
	public void AddItem(String name, int quantity) {
		Item item = database [name];
		if (item != null)
			AddItem (item, quantity);
		else if (item != null && item.maxStackSize > quantity)
			throw new ArgumentException ("Cannot more of an item than its maximum stacksize");
		else
			throw new ArgumentException ("The item with name " + name + " does not exist");
	}
	public void AddItem(int id, int quantity) {
		Item item = database [id];
		if (item != null)
			AddItem (item, quantity);
		else if (item.maxStackSize > quantity)
			throw new ArgumentException ("Cannot more of an item than its maximum stacksize");
		else
			throw new ArgumentException("The item with id " + id + " does not exist");
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

	public void UseItem(InventorySlot slot) {
		int index = findIndexOf (slot.Content);

		switch (slot.Content.Type) {
		case ItemType.Consumable:
			if (inventory[index].stackSize > 1) {
				inventory[index].stackSize--;
				slot.UpdateItemQuantity ();
			} else {
				RemoveItemAt (index);
			}
			break;
		case ItemType.Weapon:
		case ItemType.Armor:
			Debug.Log ("Now equipping " + slot.Content.ItemName);
			RemoveItemAt (index);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Wizard> ().Equip (slot.Content);
			break;
		default:
			break;
		}
	}
	private int findIndexOf(Item item) {
		for(int i = 0; i < inventory.Length; i++) {
			if(item == inventory[i])
				return i;
		}

		throw new ArgumentException ("The given item was not found in the inventory");
	}

	#endregion
	public void Initialize(int size) {
		inventory = new Item[size];
		Debug.Log ("Created inventory with size: " + size);
	}
	public void UpdateInventory(InventorySlot[] slots) {
		if (slots.Length != inventory.Length)
			Debug.LogAssertion ("Inventory and UI are not of the same size!");

		for (int i = 0; i < slots.Length; i++) {
			inventory [i] = slots [i].Content;
		}
	}

	public void SaveInventory(System.Xml.XmlWriter writer) {
		writer.WriteStartElement ("Inventory");
		foreach (var item in inventory) {
			if (item != null) {
				writer.WriteStartElement ("Item");
				writer.WriteAttributeString ("ID", item.Id.ToString());
				writer.WriteElementString ("Quantity", item.stackSize.ToString());
				writer.WriteEndElement ();
			}
		}
		writer.WriteEndElement ();
	}

	public void LoadInventory(System.Xml.XmlReader reader) {
		int itemCount = 0;
		reader.ReadToFollowing ("Inventory");
		reader.ReadToDescendant ("Item");
		do {
			string id = reader.GetAttribute ("ID");
			inventory [itemCount] = database [int.Parse (id)].Clone ();
			reader.ReadToDescendant ("Quantity");
			inventory [itemCount].stackSize = reader.ReadElementContentAsInt();
			Debug.Log ("Just parsed " + inventory [itemCount]);
			itemCount++;
			reader.ReadEndElement();
		} while(reader.ReadToNextSibling("Item"));
		reader.ReadEndElement ();
	}
}
