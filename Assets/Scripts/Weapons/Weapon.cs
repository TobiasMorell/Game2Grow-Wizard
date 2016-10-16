using System;
using UnityEngine;
//using UnityEditor;

namespace Assets.Scripts.Weapon {
	public class Weapon : Displayable
	{
		private int damage;
		private Collider2D _collider;

		public override void Equip(int damage) {
			this.damage = damage;
		}


		void Start() {
			_collider = this.GetComponent<Collider2D> ();
		}

		public void Swing() {
			_collider.enabled = true;
		}
		public void StopSwing() {
			_collider.enabled = false;
		}

		void OnTriggerEnter2D(Collider2D other) {
			if (other.tag.Equals ("Hostile") && !other.isTrigger) {
				other.GetComponent<Enemy> ().TakeDamage (damage);
			}
		}
	}
}
