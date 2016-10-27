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

	//A reference to the ItemDatabase to safe lookup time.
	private ItemDatabase database;
	//References to both inventory and equipment UI - used for displaying changes.
	[SerializeField] private InventoryUI UI;
	[SerializeField] private EquipmentUI eUI;

	//A reference to the bearer of the inventory (allows Enemies to have inventories)
	private Entity bearer;

	#endregion
	#region Unity methods
	void Start () {
		database = GameRegistry.ItemDatabase();
		if (database == null)
			Debug.LogAssertion ("FATAL ERROR: Could not find item database!");
		//Find the entity that carries this script.
		bearer = GetComponentInParent<Entity> ();

		//Crystals and motes (ONLY FOR DEBUG!!)
		Item i1 = GameRegistry.ItemDatabase()["Power Crystal"].Clone();
		i1.AddValueToSchool (Spells.School.Fire, 1);
		AddItem (i1);

		//Crystals and motes (ONLY FOR DEBUG!!)
		Item i2 = GameRegistry.ItemDatabase()["Power Crystal"].Clone();
		i2.AddValueToSchool (Spells.School.Fire, 2);
		AddItem (i2);

		Item i3 = GameRegistry.ItemDatabase () ["Power Crystal"].Clone ();
		i3.AddValueToSchool (Spells.School.Fire, 3);
		AddItem (i3);

		Item i4 = GameRegistry.ItemDatabase () ["Power Crystal"].Clone ();
		i4.AddValueToSchool (Spells.School.Fire, 4);
		AddItem (i4);

		//Armor and weapons (ONLY FOR DEBUG!!)
		AddItem(26);
	}
	#endregion
	#region Inventory manipulaiton
	/// <summary>
	/// Adds the item with specified Id.
	/// </summary>
	/// <param name="id">Identifier of item.</param>
	public void AddItem(int id) {
		AddItem (id, 1);
	}
	/// <summary>
	/// Adds the item with specified name.
	/// </summary>
	/// <remarks>Some items may have the same name! Use Id to make sure the right item is added.</remarks>
	/// <param name="name">Name of item to add.</param>
	public void AddItem(string name) {
		AddItem (name, 1);
	}
	/// <summary>
	/// Adds the given item to the inventory.
	/// </summary>
	/// <param name="item">Item to add.</param>
	public void AddItem(Item item) {
		AddItem (item, 1);
	}

	/// <summary>
	/// Adds a stack of some item to the inventory.
	/// </summary>
	/// <param name="item">Item to add.</param>
	/// <param name="quantity">Stacksize of the item.</param>
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
				//Add the given amount to an existing stack.
				if (inventory[index].Id == item.Id) {
					inventory [index].stackSize += quantity;
					UI.UpdateItems (inventory);
					return;
				}
			}
		}
		//Place the item in slot unless inventory is full (only called with non-stackable items).
		if (availIndex != inventory.Length) {
			inventory [availIndex] = item.Clone ();
			if (quantity > 1)
				inventory [availIndex].stackSize = quantity;
			UI.UpdateItems (inventory);
		}
		else
			Debug.Log ("Inventory is full");
	}
	/// <summary>
	/// Adds a stack of some item specified by name.
	/// </summary>
	/// <remarks>Some items may share name. Using Id is safer.</remarks>
	/// <param name="name">Name of item to add.</param>
	/// <param name="quantity">How many to add.</param>
	public void AddItem(String name, int quantity) {
		Item item = database [name];
		if (item != null)
			AddItem (item, quantity);
	}
	/// <summary>
	/// Adds a stack of some item specified by Id.
	/// </summary>
	/// <param name="id">Identifier of item to add.</param>
	/// <param name="quantity">How many to add.</param>
	public void AddItem(int id, int quantity) {
		Item item = database [id];
		if (item != null)
			AddItem (item, quantity);
	}

	/// <summary>
	/// Determines if the inventory contains an item with given Id.
	/// </summary>
	/// <param name="id">Identifier to check for.</param>
	public bool Contains(int id) {
		Item item = database [id];
		if (item != null)
			return contains (item);

		//Return false if the caller specified an invalid Id.
		return false;
	}
	/// <summary>
	/// Determines if the inventory contains an item with given Name.
	/// </summary>
	/// <param name="name">Name of item to check for.</param>
	public bool Contains(string name) {
		Item item = database [name];
		if (item != null)
			return contains (item);

		//Return false if the caller specified an invalid Id.
		return false;
	}
	/// <summary>
	/// Determines if the inventory contains the given item.
	/// </summary>
	/// <param name="item">Item.</param>
	private bool contains(Item item) {
		//Run through inventory to search for item.
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory [i].Id == item.Id)
				return true;
		}

		//No item has been found.
		return false;
	}

	/// <summary>
	/// Removes one of the item specified by id.
	/// </summary>
	/// <param name="id">Identifier of item to remove.</param>
	public void RemoveItem(int id) {
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i].Id == id) {
				if (inventory [i].stackSize > 1) {
					inventory [i].stackSize--;
				}
				else
					inventory [i] = null;
				UI.UpdateItems (inventory);
				break;
			}
		}
	}
	/// <summary>
	/// Removes one of the item specified by name.
	/// </summary>
	/// <remarks>Using Id is safer, as items may share names!</remarks>
	/// <param name="name">Name of item to remove.</param>
	public void RemoveItem(string name) {
		Item item = database [name];
		RemoveItem (item.Id);
	}
	/// <summary>
	/// Removes the item at given index.
	/// </summary>
	/// <param name="index">Index to remove from.</param>
	public void RemoveItemAt(int index) {
		inventory [index] = null;
	}

	/// <summary>
	/// Uses the item contained in given slot.
	/// </summary>
	/// <param name="slot">Slot to use item from.</param>
	public void UseItem(InventorySlot slot) {
		int index = findIndexOf (slot.Content);

		switch (slot.Content.Type) {
		case ItemType.Consumable:
			//Consumable items are always stackable, remove one and update the UIText.
			if (inventory[index].stackSize > 1) {
				inventory[index].stackSize--;
				slot.UpdateItemQuantity ();
			} 
			else {
				RemoveItemAt (index);
			}
			break;
		//Fall-through - all equipable items are treated similarly.
		case ItemType.Weapon:
		case ItemType.Neck:
		case ItemType.Offhand:
		case ItemType.Ring:
		case ItemType.Armor:
			//Remove from inventory and add to equipment
			RemoveItemAt (index);
			if (eUI != null)
				eUI.EquipRightClick (slot.Content);
			else
				bearer.Equip (slot.Content);
			break;
		default:
			break;
		}

		UI.UpdateItems(inventory);
	}
	/// <summary>
	/// Finds the index of the specified item.
	/// </summary>
	/// <returns>The index of the item.</returns>
	/// <param name="item">Item to search for.</param>
	private int findIndexOf(Item item) {
		for(int i = 0; i < inventory.Length; i++) {
			if(item == inventory[i])
				return i;
		}

		throw new ArgumentException ("The given item was not found in the inventory");
	}
	#endregion
	/// <summary>
	/// Initialize the inventory with specified size.
	/// </summary>
	/// <param name="size">Size of the inventory.</param>
	public void Initialize(int size) {
		inventory = new Item[size];
	}
	/// <summary>
	/// Updates the inventory with information from the UI (called when a drag has ended).
	/// </summary>
	/// <param name="slots">Updated information on the slots.</param>
	public void UpdateInventory(InventorySlot[] slots) {
		if (slots.Length != inventory.Length)
			Debug.LogAssertion ("Inventory and UI are not of the same size!");

		for (int i = 0; i < slots.Length; i++) {
			inventory [i] = slots [i].Content;
		}
	}

	#region XML
	/// <summary>
	/// Saves the inventory in XML.
	/// </summary>
	/// <param name="writer">Writer.</param>
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
	/// <summary>
	/// Loads the inventory from XML.
	/// </summary>
	/// <param name="reader">Reader.</param>
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
	#endregion
}
