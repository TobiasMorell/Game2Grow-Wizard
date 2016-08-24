using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Spells
{
    public class SpellDatabase : MonoBehaviour
    {
		private List<Spell> spells;

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

		void Start()
		{
			GameRegistry.AssignSpellDatabase (this);

			spells = new List<Spell>();
			LoadSpells();
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
