using System;
using UnityEngine;

namespace Assets.Scripts.Effects
{
	public class PoisonEffect : HostileEffect
	{
		private float dmgCooldown;
		private float dmgTimer = 0;
		private Sprite icon;

		public PoisonEffect (float effectTime, float totalDamage, Sprite icon) : base (effectTime)
		{
			this.icon = icon;
			this.dmgCooldown = totalDamage / effectTime;
			dmgTimer = dmgCooldown;
		}

		public override void onApplication (Entity entity)
		{
			base.onApplication (entity);
			entity.GetComponent<SpriteRenderer> ().color = Color.green;
			if(entity.CompareTag("Player"))
				UIStatusManager.Instance.AddDebuff(icon, duration);
		}

		public override void onEffectEnded ()
		{
			base.onEffectEnded ();
			bearer.sprite_renderer.color = Color.white;
		}

		public override void onUpdate ()
		{
			base.onUpdate ();
			if (dmgTimer >= dmgCooldown) {
				bearer.sprite_renderer.color = Color.green;
				bearer.TakeDamage (Strength, true);
				dmgTimer = 0;
			} else {
				dmgTimer += Time.deltaTime;
			}
		}
	}
}

