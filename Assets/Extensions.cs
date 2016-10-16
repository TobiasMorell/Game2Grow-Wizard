using System;
using ItemClasses;
using Spells;
using UnityEngine;

namespace ExtensionMethods {

	static class Extensions
	{
		public static ItemType StringToType(this String s) {
			switch (s) {
			case "Weapon":
				return ItemType.Weapon;
			case "Armor":
				return ItemType.Armor;
			case "Consumable":
				return ItemType.Consumable;
			case "Junk":
				return ItemType.Junk;
			case "Magic":
				return ItemType.Magic;
			case "Quest":
				return ItemType.Quest;
			case "Neck":
				return ItemType.Neck;
			case "Offhand":
				return ItemType.Offhand;
			case "Ring":
				return ItemType.Ring;
			default:
				throw new ArgumentException ("The string given has no associated ItemType: " + s);
			}
		}

		public static School StringToSchool(string str) {
			str = str.ToLower();

			switch (str) {
			case "fire":
				return School.Fire;
			case "water":
				return School.Water;
			case "life":
				return School.Life;
			case "death":
				return School.Death;
			}

			return School.None;
		}

	}
}

