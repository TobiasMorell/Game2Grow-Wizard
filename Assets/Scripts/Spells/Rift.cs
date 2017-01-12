using UnityEngine;
using System.Collections.Generic;

namespace Spells
{
	public class Rift : Castable
	{
		[SerializeField] float degreesPrSec;
		List<Entity> nearbyHostiles;
		[SerializeField] float minimumDragDistance, dragSpeed, scaledSpeed;
		float timer;

		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			Vector2 position = primaryTarget.transform.position;
			position.y += 3.5f;
			transform.position = position;

			nearbyHostiles = new List<Entity> ();
			timer = 0;
			scaledSpeed = dragSpeed * Time.deltaTime;
		}

		void Update () {
			this.transform.Rotate (0, 0, degreesPrSec * Time.deltaTime);
			//Clear away dead targets to avoid null-exceptions
			nearbyHostiles.RemoveAll(e => e == null);

			//Deal damage to all nearby enemies every second
			if (timer > 1) {
				foreach (var hostile in nearbyHostiles)
					hostile.TakeDamage (calculateDamage (), DatabaseInstance.School);
				timer = 0;
			} else
				timer += Time.deltaTime;

			//Drag all nearby enemies towards the rift
			foreach (var hostile in nearbyHostiles) {
				dragTowardsRift (hostile);
			}
		}

		private void dragTowardsRift(Entity target) {
			float distanceToRift = transform.position.x - target.transform.position.x;

			if (distanceToRift > minimumDragDistance) {
				target.transform.Translate (scaledSpeed, 0, 0);
			} else if (distanceToRift < -minimumDragDistance) {
				target.transform.Translate (-scaledSpeed, 0, 0);
			}
		}

		void OnTriggerEnter2D(Collider2D other) {
			Debug.Log ("Enemy added to Rift's target list: " + other.name);

			if (other.CompareTag ("Hostile")) {
				nearbyHostiles.Add (other.GetComponent<Entity>());
			}
		}

		void OnTriggerExit2D(Collider2D other) {
			if (other.CompareTag ("Hostile")) {
				nearbyHostiles.Remove (other.GetComponent<Entity> ());
			}
		}
	}
}

