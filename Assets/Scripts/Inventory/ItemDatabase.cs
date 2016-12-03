using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Xml;
using ExtensionMethods;
using System.Text;
using Assets.Scripts.UI;
using Spells;

namespace ItemClasses {
	public class ItemDatabase : MonoBehaviour {
		#region Variables and operator overloads
		[SerializeField] private List<Item> items;

		public Item this [int i] {
			get { return items.Find((Item itm) => itm.Id == i); }
			private set { int ind = items.FindIndex((Item itm) => itm.Id == i); items[ind] = value; }
		}

		public Item this[string s] {
			get { return items.Find ((Item i) => i.ItemName == s); }
			private set { int ind = items.FindIndex ((Item i) => i.ItemName == s); items [ind] = value; }
		}
		#endregion

		#region Unity
		void Start() {
			GameRegistry.AssignItemDatabase (this);
			/* Items are now created using the inspector, due to problems with WebGL builds.
			 * items = new List<Item> ();
			 * fillDatabase ();*/
		}
		#endregion

		#if false
		#region Fill database
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
			int vit = 0, inte = 0, str = 0;

			while (attributePresent) {
				switch (reader.GetAttribute ("Type")) {
				case "Stacksize":
					i.maxStackSize = reader.ReadElementContentAsInt ();
					Debug.Log ("Set max stacksize of " + i.ItemName + " to " + i.maxStackSize);
					break;
				case "Vitality":
					vit = reader.ReadElementContentAsInt ();
					break;
				case "Intellect":
					inte = reader.ReadElementContentAsInt ();
					break;
				case "Strength":
					str = reader.ReadElementContentAsInt ();
					break;
				default:
					Debug.LogAssertion ("Found an unknown attribute while importing items: " + reader.GetAttribute ("Type"));
					break;
				}
				attributePresent = reader.ReadToNextSibling ("Attribute");
			}

			//i.AssignAttributes (inte, str, vit);
		}
		#endregion
		#endif
	}

	public enum ItemType
	{
		Weapon, Offhand, Armor, Neck, Ring, Consumable, Junk, Quest, Magic
	}
		
	public enum Attribute {
		Strength, Vitality, Intellect
	}

	[Serializable]
	public class AttributeTag {
		public string Name;
		public int Value;

		public AttributeTag(String name, int value) {
			Name = name;
			Value = value;
		}

		public AttributeTag(AttributeTag clonedFrom) {
			Name = clonedFrom.Name;
			Value = clonedFrom.Value;
		}
	}

	[System.Serializable]
	public class Item {
		public string ItemName;
		public int Id;
		public string Description;
		public int Value;
		public ItemType Type;
		public Sprite icon;
		public GameObject prefab;
		public int maxStackSize;
		public int stackSize = 1;

		public AttributeTag[] Attributes;

		public Item(string name, string desc, int value, ItemType type, int id)
			: this(name, desc, value, type, 1, id, null) {}

		public Item(string name, string desc, int value, ItemType type, int maxStack, int id, AttributeTag[] attrs) {
			ItemName = name;
			Id = id;
			Description = desc;
			Value = value;
			Type = type;
			maxStackSize = maxStack;
			Attributes = attrs;
		}

		public Item (Item clonedFrom) : this(clonedFrom.ItemName, clonedFrom.Description, 
			clonedFrom.Value, clonedFrom.Type, clonedFrom.maxStackSize, clonedFrom.Id, null) 
		{
			this.icon = clonedFrom.icon;
			this.prefab = clonedFrom.prefab;

			var attrs = new AttributeTag[clonedFrom.Attributes.Length];
			//Clone attributes
			for (int i = 0; i < clonedFrom.Attributes.Length; i++) {
				attrs [i] = new AttributeTag (clonedFrom.Attributes [i]);
			}
			Attributes = attrs;

			if (prefab != null) {
				var inGame = prefab.GetComponent<Assets.Scripts.Weapon.Displayable> ();
				inGame.RegisterToItemID (Id);
			}
		}

		public AttributeTag[] GetAllTags() {
			return Attributes;
		}
			
		public void AddValueToTag(string tag, int value) {
				for (int i = 0; i < Attributes.Length; i++) {
					if (Attributes [i].Name.Equals (tag)) {
						Attributes [i].Value += value;
						return;
					}
				}
		}

		public void AddValueToSchool(School schl, int value) {
			AddValueToTag (schl.ToString (), value);
		}

		public void AddValueToAttribute(Attribute attr, int value) {
			AddValueToTag (attr.ToString (), value);
		}

		public int GetValueFromTag(string tag) {
			for (int i = 0; i < Attributes.Length; i++) {
				if(Attributes[i].Name.Equals(tag)) {
					return Attributes[i].Value;
				}
			}

			return 0;
		}

		public int GetValueFromAttribute(Attribute attr) {
			return GetValueFromTag (attr.ToString ());
		}

		public int GetValueFromSchool(School schl) {
			return GetValueFromTag (schl.ToString ());
		}

		public Item Clone() {
			return new Item (this);
		}

		#region Tooltip building
		public virtual string Tooltip() {
			System.Text.StringBuilder tooltipText = new StringBuilder ();
			TooltipBuilder.CreateHeadline(tooltipText, ItemName);
			tooltipText.Append ('\n');
			//Type
			TooltipBuilder.AppendColorOpen (tooltipText, "BAEEFF");
			tooltipText.Append (Type);
			TooltipBuilder.AppendColorClosure (tooltipText);

			tooltipText.Append ("\n\n");

			//Attributes
			TooltipBuilder.AppendColorOpen(tooltipText, "00CC00");
			foreach (var attr in Attributes) {
				tooltipText.Append (attr.Name);
				tooltipText.Append (": ");
				tooltipText.Append (attr.Value);
				tooltipText.Append ('\n');
			}
			TooltipBuilder.AppendColorClosure (tooltipText);

			//Description
			TooltipBuilder.CreateDescription(tooltipText, Description);
			tooltipText.Append("\n\n");

			//Value
			TooltipBuilder.AppendColorOpen (tooltipText, "FFD700");
			tooltipText.Append (Value);
			tooltipText.Append (" gold");
			TooltipBuilder.AppendColorClosure (tooltipText);

			return tooltipText.ToString ();
		}
		#endregion
	}
}