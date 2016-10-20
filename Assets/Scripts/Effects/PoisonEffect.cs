using System;
using UnityEngine;

namespace Assets.Scripts.Effects
{
	public class PoisonEffect : PeriodicDamageEffect
	{
		public PoisonEffect (float effectTime, float totalDamage, Sprite icon) : base (effectTime, totalDamage, icon)
		{		}

		public override void onApplication (Entity entity)
		{
			base.onApplication (entity);
			entity.GetComponent<SpriteRenderer> ().color = Color.green;
		}

		public override void onEffectEnded ()
		{
			base.onEffectEnded ();
			bearer.sprite_renderer.color = Color.white;
		}
	}
}

