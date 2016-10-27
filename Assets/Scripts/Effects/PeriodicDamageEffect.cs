using UnityEngine;

namespace Assets.Scripts.Effects
{
	public class PeriodicDamageEffect : Effect
	{
		protected float dmgCooldown = 1;
		protected float dmgTimer = 0;
		public float RemainingDamage { get; private set; }
		protected int damagePrTick;

		/// <summary>
		/// Initializes a new instance of the <see cref="Assets.Scripts.Effects.PeriodicDamageEffect"/> class, assuming that the
		/// taget takes damage every second.
		/// </summary>
		/// <param name="duration">Duration of the effect.</param>
		/// <param name="totalDamage">Total damage of the effect.</param>
		/// <param name="icon">Icon for UI (only for player).</param>
		public PeriodicDamageEffect(float duration, float totalDamage, Sprite icon) : base(duration, icon)
		{
			this.damagePrTick = Mathf.CeilToInt(totalDamage / duration);
			RemainingDamage = damagePrTick * duration;
			dmgTimer = dmgCooldown;
		}

		public override void onUpdate()
		{
			base.onUpdate();
			if (dmgTimer >= dmgCooldown)
			{
				bearer.TakeDamage(damagePrTick, true);
				dmgTimer = 0;
				RemainingDamage -= damagePrTick;
			}
			else {
				dmgTimer += Time.deltaTime;
			}
		}
	}
}
