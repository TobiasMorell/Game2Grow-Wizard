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
	public enum SpellAnimationType
	{
		Swing, FrontalSpell, UpwardsSpell
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
		public SpellAnimationType AnimationType;

		/// <summary>
		/// The cooldown timer should be maintained elsewhere.
		/// </summary>
		[HideInInspector]public float cooldownTimer;

		public bool OnCooldown() {
			return cooldownTimer > 0;
		}

		public void Cast(GameObject target, bool facingRight, float mod) {
			cooldownTimer = Cooldown;
			var inst = UnityEngine.Object.Instantiate(Prefab).GetComponent<Castable>();
			inst.movingRight = facingRight;
			inst.AssignModifier(mod);
			inst.Cast(target);
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
