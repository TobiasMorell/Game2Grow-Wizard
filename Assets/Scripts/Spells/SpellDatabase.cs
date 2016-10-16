using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using ItemClasses;
using ExtensionMethods;

namespace Spells
{
    public class SpellDatabase : MonoBehaviour
    {
		[SerializeField] private List<Spell> spells;

		public Spell this[int i]
		{
			get { return spells.Find((Spell spell) => spell.Id == i); }
			private set { int ind = spells.FindIndex((Spell spell) => spell.Id == i); spells[ind] = value; }
		}
		public Spell this[string s]
		{
			get { return spells.Find((Spell i) => i.Name == s); }
			private set { int ind = spells.FindIndex((Spell i) => i.Name == s); spells[ind] = value; }
		}

		#region Get spells from crystal
		public Spell GetSpellFromCrystal(AttributeTag[] twoBestSchools) {
			//There are no points in any schools on the crystal, just return null.
			if (twoBestSchools [0] == null && twoBestSchools [1] == null) {
				return null;
			}
				
			if (twoBestSchools[1] == null){
				return onlyPrimary (twoBestSchools[0]);
			}

			switch (Extensions.StringToSchool( twoBestSchools[0].Name )) {
			case School.Death:
				return deathPrimary (twoBestSchools[0], twoBestSchools[1]);
			case School.Fire:
				return firePrimary (twoBestSchools[0], twoBestSchools[1]);
			case School.Life:
				return lifePrimary (twoBestSchools[0], twoBestSchools[1]);
			case School.Water:
				return waterPrimary (twoBestSchools[0], twoBestSchools[1]);
			}

			throw new InvalidOperationException ("Could not find a spell fitting to the given schools " + twoBestSchools[0] + " and " + twoBestSchools[1]);
		}

		private Spell onlyPrimary(AttributeTag primary) {
			switch (Extensions.StringToSchool(primary.Name)) {
			case School.Death:
				return getNecromancer (primary.Value);
			case School.Fire:
				return getPyromancer(primary.Value);
			case School.Life:
				return getCleric (primary.Value);
			case School.Water:
				return getHydromancer (primary.Value);
			}

			throw new InvalidOperationException ("Could not find a spell fitting to the given school " + primary);
		}

		#region Specs
		private Spell getNecromancer(int sum) {
			if (sum == 4)
				return this [25];
			if (sum == 3)
				return this [23];
			if (sum == 2)
				return this [29];
			return this [22]; /*Needs a spell*/
		}

		private Spell getVolcano(int sum) {
			if (sum == 8)
				return this [7];
			if (sum >= 6)
				return this [6];
			if (sum >= 4)
				return this [5];
			return this [4];
		}

		private Spell getVampire(int sum) {
			if (sum == 8)
				return this [26];
			if (sum >= 6)
				return this [28];
			if (sum >= 4)
				return this [27];
			return this [21];
		}

		private Spell getIcelord (int sum) {
			if (sum == 8)
				return this [24];
			if (sum >= 6)
				return this [20];
			if (sum >= 4)
				return this [19];
			return this [18];
		}

		private Spell getPyromancer(int sum) {
			if (sum == 4)
				return this [3];
			if (sum == 3)
				return this [2];
			if (sum == 2)
				return this [1];
			return this [0];
		}

		private Spell getPhoenix(int sum) {
			if (sum == 8)
				return this [10];
			if (sum >= 6)
				return this [9];
			if (sum >= 4)
				return this [1];
			return this [8];
		}

		private Spell getSteam(int sum) {
			if (sum == 8)
				return this [13];
			if (sum >= 6)
				return this [16];
			if (sum >= 4)
				return this [12];
			return this [11];
		}

		private Spell getCleric(int sum) {
			if (sum == 4)
				return this [33];
			if (sum == 3)
				return this [32];
			if (sum == 2)
				return this [31];
			return this [30];
		}

		private Spell getHydromancer(int sum) {
			if (sum == 4)
				return this [17];
			if (sum == 3)
				return this [16];
			if (sum == 2)
				return this [15];
			return this [14];
		}

		#endregion

		private Spell deathPrimary(AttributeTag primary, AttributeTag secondary) {
			int sum = (byte) (primary.Value + secondary.Value);
			switch (Extensions.StringToSchool(secondary.Name)) {
			case School.Fire:
				return getVolcano (sum);
			case School.Life:
				return getVampire (sum);
			case School.Water:
				return getIcelord (sum);
			}

			throw new InvalidOperationException ("Could not find a spell fitting to the given schools " + primary + " and " + secondary);
		}

		private Spell firePrimary(AttributeTag primary, AttributeTag secondary) {
			byte sum = (byte) (primary.Value + secondary.Value);
			switch (Extensions.StringToSchool(secondary.Name)) {
			case School.Death:
				return getVolcano (sum);
			case School.Life:
				return getPhoenix (sum);
			case School.Water:
				return getSteam (sum);
			default:
				break;
			}

			throw new InvalidOperationException ("Could not find a spell fitting to the given schools " + primary + " and " + secondary);
		}

		private Spell lifePrimary(AttributeTag primary, AttributeTag secondary) {
			byte sum = (byte) (primary.Value + secondary.Value);
			switch (Extensions.StringToSchool(secondary.Name)) {
			case School.Death:
				return getVampire (sum);
			case School.Fire:
				return getPhoenix (sum);
			case School.Water:
				throw new NotImplementedException ("This specialization is yet to be added");
			}

			throw new InvalidOperationException ("Could not find a spell fitting to the given schools " + primary + " and " + secondary);
		}

		private Spell waterPrimary(AttributeTag primary, AttributeTag secondary) {
			byte sum = (byte) (primary.Value + secondary.Value);
			switch (Extensions.StringToSchool(secondary.Name)) {
			case School.Death:
				return getIcelord (sum);
			case School.Fire:
				return getSteam (sum);
			case School.Life:
				throw new NotImplementedException ("This specialization is yet to be added.");
			default:
				break;
			}

			throw new InvalidOperationException ("Could not find a spell fitting to the given schools " + primary + " and " + secondary);
		}

		#endregion
		void Start()
		{
			GameRegistry.AssignSpellDatabase (this);

			/* Spells are now created via inspector, due to WebGL issues.
			 * spells = new List<Spell>();
			 * LoadSpells();*/
		}

        public void LoadSpells()
        {
			string fullPath = Directory.GetCurrentDirectory() + "/Assets/Resources/Spells.xml";
			if(File.Exists(fullPath))
			{
				Uri basePath = new Uri(fullPath);
				using (XmlReader reader = XmlReader.Create(basePath.AbsoluteUri))
				{
					reader.ReadToFollowing("SpellDatabase");
					bool moreSpells = reader.ReadToDescendant("Spell");
					while(moreSpells)
					{
						readSpell(reader);
						//Read end element and check if there are more spells
						reader.ReadEndElement();
						moreSpells = reader.ReadToNextSibling("Spell");
					}
				}
			}
        }

		private void readSpell(XmlReader reader)
		{
			reader.ReadToDescendant("Id");
			int id = reader.ReadElementContentAsInt();
			reader.ReadToNextSibling("Name");
			string name = reader.ReadElementContentAsString();
			reader.ReadToNextSibling("Description");
			string desc = reader.ReadElementContentAsString();
			reader.ReadToNextSibling("Cost");
			int cost = reader.ReadElementContentAsInt();
			reader.ReadToNextSibling("Cooldown");
			float cd = reader.ReadElementContentAsFloat();

			spells.Add(new Spell(id, name, desc, cost, cd));
		}
    }
}
