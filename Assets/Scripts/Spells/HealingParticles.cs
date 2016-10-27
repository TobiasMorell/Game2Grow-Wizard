using System;
using UnityEngine;

namespace Spells
{
	public class HealingParticles : MonoBehaviour
	{
		public int Healing { get; set; }
		public float Speed;

		void Update() {
			this.transform.position += Vector3.right * Speed * Time.deltaTime;
		}

		void OnTriggerEnter2D(Collider2D other) {
			if(other.CompareTag("Player")) {
				Debug.Log ("Healing particle healed for: " + Healing);
				other.GetComponent<Entity>().Heal (Healing);
				Destroy (this.gameObject);
			}
		}
	}
}

