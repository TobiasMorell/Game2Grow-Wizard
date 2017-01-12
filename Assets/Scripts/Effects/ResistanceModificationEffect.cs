using System;
using UnityEngine;
using Spells;

namespace Assets.Scripts.Effects
{
	public class ResistanceModificationEffect : Effect
	{
		float modifier;
		School school;

		public ResistanceModificationEffect (School school, float modifier, float duration, Sprite icon) : base(duration, icon)
		{
			this.modifier = modifier;
			this.school = school;
		}

		public override void onApplication (Entity entity)
		{
			base.onApplication (entity);
			bearer.GetComponent<Resistance> ().IncreaseResistanceToSchool (school, modifier);
		}

		public override void onEffectEnded ()
		{
			base.onEffectEnded ();
			bearer.GetComponent<Resistance> ().DecreaseResistanceToSchool (school, modifier);
		}
	}
}

