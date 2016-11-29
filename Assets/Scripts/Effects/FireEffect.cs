using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Effects
{
	class FireEffect : PeriodicDamageEffect
	{
		public FireEffect(float duration, float totalDamage, Sprite icon) : base(duration, totalDamage, icon)
		{
			this.description = "You are burning alive! You take damage each second and you are extra vulnerable to fire attacks";
			this.School = EffectSchool.Fire;
		}

		public override void onApplication(Entity entity)
		{
			base.onApplication(entity);
			entity.GetComponent<SpriteRenderer>().color = Color.red;
		}

		public override void onEffectEnded()
		{
			base.onEffectEnded();
			base.bearer.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}
}
