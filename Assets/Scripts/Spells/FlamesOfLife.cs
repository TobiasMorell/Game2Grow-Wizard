using System;
using UnityEngine;
using Assets.Scripts.Effects;

namespace Spells
{
	public class FlamesOfLife : SingleTarget
	{
		[SerializeField] private GameObject feedbackParticles;

		public override void Cast (GameObject primaryTarget)
		{
			base.Cast (primaryTarget);

			//Get the burning-effects from the target
			Entity ent = primaryTarget.GetComponent<Entity> ();
			var brns = ent.GetAllEffects (Assets.Scripts.Effects.EffectSchool.Fire);
			int healing = 0;
			//Sum the remaining damage of all the burns
			foreach (var brn in brns) {
				healing += Mathf.CeilToInt((brn as PeriodicDamageEffect).RemainingDamage);
				ent.RemoveEffect (brn);
			}

			//If there actually was a burning effect spawn particles that seek towards the player and heals him
			if (healing > 0) {
				var fb = Instantiate (feedbackParticles);
				fb.transform.position = transform.position;
				var hp = fb.GetComponent<HealingParticles> ();
				hp.Healing = healing;
				if (movingRight)
					hp.Speed = -hp.Speed;
			}
		}


	}
}

