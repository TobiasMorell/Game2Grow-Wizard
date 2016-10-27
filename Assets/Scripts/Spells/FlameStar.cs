using System;
using UnityEngine;

namespace Spells
{
	public class FlameStar : Castable
	{
		[SerializeField] int numberOfBolts;
		[SerializeField] GameObject flamedropPrefab;

		public override void Cast (GameObject primaryTarget)
		{
			transform.position = primaryTarget.transform.position;

			for (int i = 0; i < numberOfBolts; i++) {
				var inst = Instantiate (flamedropPrefab);
				inst.transform.position = transform.position;
				float angle = -Mathf.Deg2Rad * UnityEngine.Random.Range (0, 180);
				//The y-coordinate in the rotation formula is not necesarry as we're rotating from <-1, 0> (Vector2.left)
				float x = -1 * Mathf.Cos (angle);
				float y = -1 * Mathf.Sin (angle);
				inst.GetComponent<Rigidbody2D> ().velocity = new Vector2 (x * Speed, y * Speed);
			}
		}
	}
}

