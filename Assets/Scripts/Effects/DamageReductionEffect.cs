using UnityEngine;

namespace Assets.Scripts.Effects
{
	public class DamageReductionEffect : Effect
	{
		float reductionPercent;

		/// <summary>
		/// Creates a new damage reduction effect.
		/// </summary>
		/// <param name="reductionPercent">Reduction percent, must be between 0 and 1.</param>
		/// <param name="duration">Duration in seconds.</param>
		/// <param name="icon">Icon for GUI.</param>
		public DamageReductionEffect (float reductionPercent, float duration, Sprite icon) : base (duration, icon)
		{
			this.reductionPercent = reductionPercent;
			this.School = Spells.School.None;
		}

		public override void onApplication (Entity entity)
		{
			base.onApplication (entity);
			bearer.DamageModifier -= reductionPercent;
		}

		public override void onEffectEnded ()
		{
			base.onEffectEnded ();
			bearer.DamageModifier += reductionPercent;
		}
	}
}

