using System;
using System.Collections;
using UnityEngine;

namespace Spells
{
	public class VolcanicActivity : Castable
	{
		float remaining = 2.75f, timer = 0, lifeTimer = 0;
		#pragma warning disable 0649
		[SerializeField] Castable boltPrefab;
		[SerializeField] float ascendSpeed, timeBetweenShots;
		#pragma warning restore 0649

		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			transform.position = primaryTarget.transform.position - new UnityEngine.Vector3(0, 3);
		}

		void Update() {
			if (remaining > 0) {
				float step = ascendSpeed * Time.deltaTime;
				this.transform.position += new UnityEngine.Vector3 (0, step);
				remaining -= step;
			} else {
				if (timer > timeBetweenShots) {
					timer = 0;
					var inst = Instantiate (boltPrefab);
					formatSpawn (inst);
				}
				timer += Time.deltaTime;
			}

			if (lifeTimer > lifetime)
				Destroy (this.gameObject);
			else
				lifeTimer += Time.deltaTime;
		}

		protected void formatSpawn (Castable spawn)
		{
			spawn.AssignModifier (damageModifier);
			//Move bolt to top of volcano
			spawn.transform.position = transform.position + new Vector3(0, 1.5f);

			//Calculate a random direction to shoot the bolt.
			float angle = -Mathf.Deg2Rad * UnityEngine.Random.Range (0, 180);
			//The y-coordinate in the rotation formula is not necesarry as we're rotating from <-1, 0> (Vector2.left)
			float x = -1 * Mathf.Cos (angle);
			float y = -1 * Mathf.Sin (angle);
			spawn.GetComponent<Rigidbody2D> ().velocity = new Vector2 (x * Speed, y * Speed);
		}
	}
}

