using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Xml;
using ExtensionMethods;

namespace ItemClasses {
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
			fillDatabase ();
		}

		private void fillDatabase() {
			//Open and xml-reader to the Items.xml file, which contains all items
			string fullPath = Directory.GetCurrentDirectory () + "/Assets/Resources/Items.xml";
			if (File.Exists (fullPath)) {
				Uri basePath = new Uri (fullPath);
				using(XmlReader reader = XmlReader.Create(basePath.AbsoluteUri)){
					readItems (reader);
				}
			} else
				Debug.Log ("The specified save-game did not exist.");
		}

		private void readItems(XmlReader reader) {
			reader.ReadToFollowing ("ItemDatabase");
			bool continueReading = reader.ReadToDescendant ("Item");

			while (continueReading) {
				//Read all information from the .xml-file
				reader.ReadToDescendant ("Id");
				int id = reader.ReadElementContentAsInt ();
				reader.ReadToNextSibling ("Name");
				string name = reader.ReadElementContentAsString ();
				reader.ReadToNextSibling ("Description");
				string desc = reader.ReadElementContentAsString ();
				reader.ReadToNextSibling ("Value");
				int value = reader.ReadElementContentAsInt ();
				reader.ReadToNextSibling ("Type");
				string type = reader.ReadElementContentAsString ();
				ItemType t = type.StringToType ();

				//And add the item to the database
				Item i = new Item (name, desc, value, t, id);

				//check for attributes and add them to the item
				checkForAttributes(reader, i);

				//Add the item to the databse and read end element
				items.Add(i);
				reader.ReadEndElement ();
				//Check if there is another item to read
				continueReading = reader.ReadToNextSibling ("Item");
			}
		}

		private void checkForAttributes(XmlReader reader, Item i) {
			bool attributePresent = reader.ReadToNextSibling ("Attribute");

			while (attributePresent) {
				switch (reader.GetAttribute ("Type")) {
				case "Stacksize":
					i.maxStackSize = reader.ReadElementContentAsInt ();
					Debug.Log ("Set max stacksize of " + i.ItemName + " to " + i.maxStackSize);
					break;
				}
				attributePresent = reader.ReadToNextSibling ("Attribute");
			}
		}
	}

	public enum ItemType
	{
		Weapon, Armor, Consumable, Junk, Quest, Magic
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
		public int maxStackSize;
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
}