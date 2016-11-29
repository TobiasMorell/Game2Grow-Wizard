using System;
using UnityEngine;
using Assets.Scripts.Effects;

namespace Spells
{
	public class Quicksand : SingleTarget
	{
		void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag ("Hostile")) {
				other.GetComponent<Entity> ().ApplyEffect (new SpeedReductionEffect (BaseDamage, lifetime, DatabaseInstance.Icon));
			}
		}
	}
}

