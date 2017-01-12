using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.NPC;

namespace Spells {
	public class MadnessBeam : Castable
	{
		const float dmg_cooldown = 0.2f;
		private List<EnemyDmgTimer> touchingEnemies;

		class EnemyDmgTimer
		{
			public float dmg_timer = 3;
			public Enemy e;
		}

		public override void Cast(GameObject target) {
			base.Cast (target);
			touchingEnemies = new List<EnemyDmgTimer> ();
		}

		void Update() {
			foreach (var edt in touchingEnemies) {
				if (edt.e == null) {
					touchingEnemies.Remove (edt);
					break;
				}

				if (edt.dmg_timer >= dmg_cooldown) {
					edt.e.TakeDamage (calculateDamage(), DatabaseInstance.School);
					edt.dmg_timer = 0;
				} else {
					edt.dmg_timer += Time.deltaTime;
				}
			}
		}

		void OnTriggerEnter2D(Collider2D other) {
			if (!other.isTrigger && other.CompareTag("Hostile")) {
				EnemyDmgTimer edt = new EnemyDmgTimer ();
				edt.e = other.GetComponent<Enemy>();
				touchingEnemies.Add (edt);
			}
		}

		void OnTriggerExit2D(Collider2D other) {
			if (!other.isTrigger && other.CompareTag("Hostile")) {
				touchingEnemies.RemoveAll (edt => edt.e == other.GetComponent<Enemy>() );
			}
		}
	}
}