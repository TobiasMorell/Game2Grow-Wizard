using System;
using System.Collections;
using UnityEngine;

namespace Spells
{
	public class VolcanicActivity : SpellInstantiator
	{
		float remaining = 2.75f;
		[SerializeField] float ascendSpeed;

		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			transform.position = primaryTarget.transform.position - new UnityEngine.Vector3(0, 3);
			StartCoroutine (ascend());
			Destroy (this.gameObject, lifetime);
		}

		IEnumerator ascend() {
			if (remaining > 0) {
				float step = ascendSpeed * Time.deltaTime;
				this.transform.position += new UnityEngine.Vector3 (0, step);
				remaining -= step;

				yield return null;
			} else {
				//Just keep spawning until destoyed by destroy called in Cast.
				StartCoroutine (spawnInstances (1000));
			}
		}

		protected override void formatSpawn (Castable spawn)
		{
			//Move bolt to top of volcano
			spawn.transform.position += new Vector3(0, 1.5f);

			//Calculate a random direction to shoot the bolt.
			float angle = -Mathf.Deg2Rad * UnityEngine.Random.Range (0, 180);
			//The y-coordinate in the rotation formula is not necesarry as we're rotating from <-1, 0> (Vector2.left)
			float x = -1 * Mathf.Cos (angle);
			float y = -1 * Mathf.Sin (angle);
			spawn.GetComponent<Rigidbody2D> ().velocity = new Vector2 (x * Speed, y * Speed);
		}
	}
}

