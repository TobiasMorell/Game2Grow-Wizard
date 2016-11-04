using System;
using Assets.Scripts.Effects;
using UnityEngine;
using Assets.Scripts.UI;

namespace Spells
{
	public class DrawLava : Castable, IEffectDispenser
	{
		[SerializeField] float effectDuration;

		public Effect GetEffectInstance() {
			return new FireEffect (effectDuration, calculateDamage (), DatabaseInstance.Icon);
		}

		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			var wizz = primaryTarget.GetComponent<Wizard> ();
			if (wizz) {
				wizz.weapon.Enchant (this, Color.red, lifetime);
				UIStatusManager.Instance.AddBuff (DatabaseInstance.Icon, lifetime, "Deals fire damage on melee attacks");
			}
			Destroy (this.gameObject, lifetime);
		}
	}
}

