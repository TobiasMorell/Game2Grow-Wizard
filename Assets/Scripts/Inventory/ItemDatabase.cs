using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
	private List<Item> items;

	public Item this [int i] {
		get { return items.Find((Item itm) => itm.Id == i); }
		private set { int ind = items.FindIndex((Item itm) => itm.Id == i); items[ind] = value; }
	}

	public Item this[string s] {
		get { return items.Find ((Item i) => i.ItemName == s); }
		private set { int ind = items.FindIndex ((Item i) => i.ItemName == s); items [ind] = value; }
	}

	void Start() {
		items = new List<Item> ();
		items.Add(new Item("Novice Sword", "A sword for novice wizards", 1, ItemType.Weapon, 0));
		items.Add(new Item("Fiend Reach", "The blade wielded by the king of the undead armies.", 25, ItemType.Weapon, 1));
		items.Add (new Item ("Flaming Staff", "The staff burns ever bright to light the way of wizards.", 12, ItemType.Weapon, 2));
		items.Add (new Item ("Kings Bane", "Legend has it that this blade was used to slay the belowed king Oidin.", 20, ItemType.Weapon, 3));
		items.Add (new Item ("Orb of Madness", "Glare into the eye and madness will corrupt your soul.", 45, ItemType.Weapon, 4));
		items.Add (new Item ("Sparkler", "Celebrate your victory with a burst of stars.", 16, ItemType.Weapon, 5));
		items.Add (new Item ("Void Wand", "Forged with materials from the depths of Despair Abyss.", 45, ItemType.Weapon, 6));
		items.Add (new Item ("Walking Stick", "Originally used as support for elderly, but also packs quiet a punch!", 3, ItemType.Weapon ,7));
		items.Add (new Item ("Wizards Robes", "A robe used by novice wizards.", 1, ItemType.Armor, 8));
		items.Add (new Item ("Chain Armor", "A sturdy armor crafted to sustain slashing and piercing.", 9, ItemType.Armor, 9));
		items.Add (new Item ("Health Potion", "Regenerates some health when used.", 5, ItemType.Consumable, 10, 10));
		items.Add (new Item ("Mana Potion", "Regenerates mana when consumed.", 5, ItemType.Consumable, 10, 11));
	}
}

public enum ItemType
{
	Weapon, Armor, Consumable, Junk, Quest
}

[System.Serializable]
public class Item {
	public string ItemName;
	public readonly int Id;
	public string Description;
	public int Value;
	public ItemType Type;
	public Sprite icon;
	public GameObject prefab;
	public readonly int maxStackSize;
	public int stackSize = 1;

	public Item(string name, string desc, int value, ItemType type, int id) : this(name, desc, value, type, 1, id) {}

	public Item(string name, string desc, int value, ItemType type, int maxStack, int id) {
		ItemName = name;
		Id = id;
		Description = desc;
		Value = value;
		Type = type;
		maxStackSize = maxStack;
		icon = Resources.Load<Sprite> ("Item Icons/" + name);
		prefab = Resources.Load<GameObject> ("Item Prefabs/" + name);
	}

	public Item Clone() {
		return new Item (ItemName, Description, Value, Type, maxStackSize, Id);
	}
}