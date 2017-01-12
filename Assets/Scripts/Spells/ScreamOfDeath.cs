using System;
using UnityEngine;
using Assets.Scripts.Effects;

namespace Spells
{
	public class ScreamOfDeath : Castable
	{
		[SerializeField] float damageReduction, vulnerabilityModifier;

		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			base.Cast (primaryTarget);
			Destroy (this.gameObject, 0.2f);
		}

		void OnTriggerEnter2D(Collider2D other) {
			var entity = other.GetComponent<Entity> ();
			entity.ApplyEffect (new DamageReductionEffect (damageReduction, lifetime, null));
			entity.ApplyEffect (new VulnerabilityEffect (vulnerabilityModifier, lifetime, null));
		}
	}
}

