using System;
using UnityEngine;

namespace Assets.Scripts.Effects
{
	public class SpeedReductionEffect : Effect
	{
		float slowDown;

		public SpeedReductionEffect (float slowDown, float duration, Sprite icon) : base(duration, icon)
		{
			this.slowDown = slowDown;
			this.School = EffectSchool.Water;
		}

		public override void onApplication (Entity entity)
		{
			base.onApplication (entity);
			entity.SlowDown (slowDown);
		}

		public override void onEffectEnded ()
		{
			base.onEffectEnded ();
			bearer.SpeedUp (slowDown);
		}
	}
}

