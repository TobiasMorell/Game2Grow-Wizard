using System;
using ItemClasses;
using Assets.Scripts.Spells;
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
			default:
				throw new ArgumentException ("The string given has no associated ItemType: " + s);
			}
		}

		public static Spell[] GetSpells(this Specialization spec)
		{
			Spell[] spells = new Spell[4];
			SpellDatabase database = SpellDatabase.Instance();

			switch (spec)
			{
				case Specialization.Fire:
					
					break;
				case Specialization.Frost:
					break;
				case Specialization.Life:
					break;
				case Specialization.Death:
					break;
				case Specialization.Pyromancer:
					spells[0] = (database["Fire Blast"]);
					spells[1] = (database["Ignite"]);
					spells[2] = (database["Flames of Life"]);
					spells[3] = (database["Flame Star"]);
					break;
				case Specialization.LavaBender:
					spells[0] = database["Molten Rock"];
					spells[1] = database["Draw Lava"];
					spells[2] = database["Lava Cannon"];
					spells[3] = database["Volcanic Activity"];
					break;
				case Specialization.Elementalist:
					spells[0] = database["Steam Blast"];
					spells[1] = database["Quicksand"];
					spells[2] = database["Rock Shower"];
					spells[3] = database["N/A"];
					break;
				case Specialization.PhoenixLord:
					spells[0] = database["Flaming Feathers"];
					spells[1] = database["Ignite"];
					spells[2] = database["Blazing Flight"];
					spells[3] = database["Summon Phoenix"];
					break;
				case Specialization.Hydromancer:
					spells[0] = database["Water Bolt"];
					spells[1] = database["Hydro Pump"];
					spells[2] = database["Refresh"];
					spells[3] = database["Bubble"];
					break;
				case Specialization.IceLord:
					spells[0] = database["Ice Bolt"];
					spells[1] = database["Blizzard"];
					spells[2] = database["Ice Coat"];
					spells[3] = database["Break Ice"];
					break;
				case Specialization.Necromancer:
					spells[0] = database["Drain Life"];
					spells[1] = database["Summon Dead"];
					spells[2] = database["Scream of Death"];
					spells[3] = database["Bone Boomerang"];
					break;
				case Specialization.Vampire:
					spells[0] = database["Void Trap"];
					spells[1] = database["Chaos Disc"];
					spells[2] = database["Mind Control"];
					spells[3] = database["Rift"];
					break;
				case Specialization.Cleric:
					spells[0] = database["Bolt of Light"];
					spells[1] = database["Prayer"];
					spells[2] = database["Condemn"];
					spells[3] = database["Holy Radiance"];
					break;
				default:
					break;
			}
			return spells;
		}
	}
}

