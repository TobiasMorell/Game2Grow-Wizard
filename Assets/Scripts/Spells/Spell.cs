using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.UI;

namespace Spells
{
	public enum School
	{
		Fire, Water, Life, Death, None
	}
	[System.Serializable]
	public class Spell
	{
		public string Name;
		public int Id;
		public string Description;
		public int Cost;
		public float Cooldown;
		public Sprite Icon;
		public GameObject Prefab;
		public bool RequiresTarget;
		public bool TargetsSelf;
		/// <summary>
		/// The cooldown timer should be maintained elsewhere.
		/// </summary>
		[HideInInspector]public float cooldownTimer;

		public bool OnCooldown() {
			return cooldownTimer > 0;
		}

		#region Instantiation
		/// <summary>
		/// Initializes a new instance of the <see cref="Assets.Scripts.Spells.Spell"/> class to be stored in SpellDatabase and displayed on UI.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="name">Name.</param>
		/// <param name="desc">Desc.</param>
		/// <param name="cost">Cost.</param>
		/// <param name="cooldown">Cooldown.</param>
		public Spell(int id, string name, string desc, int cost, float cooldown)
		{
			this.RequiresTarget = true;
			this.TargetsSelf = false;

			this.Id = id;
			this.Name = name;
			this.Description = desc;
			this.Cooldown = cooldown;
			this.Cost = cost;
			Icon = Resources.Load<Sprite>("Spell Icons/" + name);
			Prefab = Resources.Load<GameObject>("Spell Prefabs/" + name);
		}

		public Spell(int id, string name, string desc, int cost, float cooldown, bool targetted, bool selfCast) : this(id, name, desc, cost, cooldown) {
			this.RequiresTarget = targetted;
			this.TargetsSelf = selfCast;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Assets.Scripts.Spells.Spell"/> class. This instance should only be referenced
		/// from entities and NOT display on UI!
		/// </summary>
		/// <param name="cost">The cost of the spell.</param>
		/// <param name="cooldown">The cooldown of the spell.</param>
		public Spell(int cost, float cooldown) {
			this.Cost = cost;
			this.Cooldown = cooldown;
			cooldownTimer = cooldown;
		}

		/// <summary>
		/// Clone this instance for reference by SpellCaster class.
		/// </summary>
		public Spell Clone() {
			return new Spell (Cost, Cooldown);
		}
		#endregion

		public void Cast(GameObject target) {
			cooldownTimer = Cooldown;
			GameObject.Instantiate (Prefab).GetComponent<Castable>().Cast(target);
		}

		#region Tooltip
		public string Tooltip()
		{
			StringBuilder tooltipSB = new StringBuilder();
			TooltipBuilder.CreateHeadline(tooltipSB, Name);

			tooltipSB.Append("\n");

			//Show mana cost
			TooltipBuilder.AppendColorOpen(tooltipSB, "0021FF");
			tooltipSB.Append(Cost);
			tooltipSB.Append(" mana");
			TooltipBuilder.AppendColorClosure(tooltipSB);
			tooltipSB.Append("\n");

			//Show cooldown
			TooltipBuilder.CreateDescription(tooltipSB, Cooldown + " seconds cooldown.\n\n" + Description);

			return tooltipSB.ToString();
		}
		#endregion
	}
}
