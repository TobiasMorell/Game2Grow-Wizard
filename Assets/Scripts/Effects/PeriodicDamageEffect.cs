using UnityEngine;

namespace Assets.Scripts.Effects
{
	public class PeriodicDamageEffect : Effect
	{
		protected float dmgCooldown;
		protected float dmgTimer = 0;

		public PeriodicDamageEffect(float duration, float totalDamage, Sprite icon) : base(duration, icon)
		{
			this.dmgCooldown = totalDamage / duration;
			dmgTimer = dmgCooldown;
		}

		public override void onUpdate()
		{
			base.onUpdate();
			if (dmgTimer >= dmgCooldown)
			{
				bearer.TakeDamage(1, true);
				dmgTimer = 0;
			}
			else {
				dmgTimer += Time.deltaTime;
			}
		}
	}
}
