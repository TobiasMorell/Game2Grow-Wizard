using System;
using UnityEngine;
using Assets.Scripts.Effects;
using System.Collections;
using Assets.Scripts.NPC;

namespace Assets.Scripts.Weapon {
	public class Weapon : Displayable
	{
		private int damage;
		private Collider2D _collider;
		private ParticleSystem enchantmentParticles;
		private IEffectDispenser _dispenser;

		public override void Equip(int damage) {
			this.damage = damage;
		}

		/// <summary>
		/// Enchants the weapon with the specified effect-dispenser, particle-color and duration.
		/// </summary>
		/// <param name="dispenser">EffectDispenser to instantiate effects.</param>
		/// <param name="particleColor">Particle color of the enchantment.</param>
		/// <param name="duration">Duration of the enchantment (0 yields until replaced).</param>
		public void Enchant(IEffectDispenser dispenser, Color particleColor, float duration) {
			_dispenser = dispenser;

			enchantmentParticles.startColor = particleColor;
			enchantmentParticles.Play();
			if(duration > 0) 
				StartCoroutine (RemoveEnchant (duration));
		}

		private IEnumerator RemoveEnchant(float duration) {
			yield return new WaitForSeconds (duration);
			_dispenser = null;
			enchantmentParticles.Stop ();
		}

		void Start() {
			_collider = this.GetComponent<Collider2D> ();
			enchantmentParticles = transform.parent.GetComponentInChildren<ParticleSystem> ();//GetComponentInParent<ParticleSystem> ();
			Debug.Log ("Enchantment particles: " + enchantmentParticles.name);
		}

		public void Swing() {
			_collider.enabled = true;
		}
		public void StopSwing() {
			_collider.enabled = false;
		}

		void OnTriggerEnter2D(Collider2D other) {
			if (other.tag.Equals ("Hostile") && !other.isTrigger) {
				var en = other.GetComponent<Enemy> ();
				en.TakeDamage (damage, Spells.School.Melee);
				if (_dispenser != null) {
					en.ApplyEffect (_dispenser.GetEffectInstance ());
				}
			}
		}
	}
}
