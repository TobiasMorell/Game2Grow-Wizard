using System;
using UnityEngine;

namespace Spells
{
	public class FlameStar : SpellInstantiator
	{
		protected override void formatSpawn (Castable spawn)
		{
			float angle = -Mathf.Deg2Rad * UnityEngine.Random.Range (0, 180);
			//The y-coordinate in the rotation formula is not necesarry as we're rotating from <-1, 0> (Vector2.left)
			float x = -1 * Mathf.Cos (angle);
			float y = -1 * Mathf.Sin (angle);
			spawn.GetComponent<Rigidbody2D> ().velocity = new Vector2 (x * Speed, y * Speed);
		}
	}
}

