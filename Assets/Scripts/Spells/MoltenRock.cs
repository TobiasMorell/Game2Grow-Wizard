using System;
using UnityEngine;

namespace Spells
{
	public class MoltenRock : Castable
	{
		[SerializeField] float ySpeed;
		private Vector3 _vecSpeed;

		void Awake() {
			_vecSpeed = new Vector3 (Speed, ySpeed);
		}

		void Update() {
			transform.position += _vecSpeed * Time.deltaTime;
		}

		public override void Cast (GameObject primaryTarget)
		{
			target = primaryTarget;

			if (!movingRight)
				Flip ();

			transform.position = target.transform.position + new Vector3 (3, 3);
		}

		void OnTriggerEnter2D(Collider2D other) {
			Debug.Log ("Collided with: " + other.tag);

			if (!other.isTrigger) {
				//Just explode if the rock hits the platform.
				if (other.CompareTag ("Platform")) {
					Debug.Log ("Should explode");
					var inst = Instantiate (particleEffect);
					inst.transform.position = transform.position;
					Destroy (this.gameObject);
					return;
				}
					
				var ent = other.GetComponent<Entity> ();
				//Deal increased damage to the primary target
				if (other.gameObject == target) {
					damageModifier *= 1.8f;
					ent.TakeDamage (calculateDamage (), DatabaseInstance.School);
					var part = Instantiate (particleEffect);
					part.transform.position = ent.transform.position;
					damageModifier /= 1.8f;
				}
				//And normal damage to any other target the rock hits.
				else if (ent) {
					ent.TakeDamage (calculateDamage(), DatabaseInstance.School);
					var part = Instantiate (particleEffect);
					part.transform.position = ent.transform.position;
				}
			}
		}
	}
}

