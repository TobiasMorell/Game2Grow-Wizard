using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Spells
{
	public class Spell
	{
		public readonly int Id;
		public readonly string Name;
		public readonly string Description;
		public readonly int Cost;
		public readonly int Cooldown;
		public readonly Sprite Icon;
		public readonly GameObject Prefab;


		public Spell(int id, string name, string desc, int cost, int cooldown)
		{
			this.Id = id;
			this.Name = name;
			this.Description = desc;
			this.Cooldown = cooldown;
			this.Cost = cost;
			Icon = Resources.Load<Sprite>("Spell Icons/" + name);
			Prefab = Resources.Load<GameObject>("Spell Prefabs/" + name);
		}
	}
}
