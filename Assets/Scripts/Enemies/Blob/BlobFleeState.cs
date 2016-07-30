using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	public class BlobFleeState : IEnemyState
	{
		Enemy enemy;
		private float fleeCap;
		private float fleeTimer;

		public void Execute ()
		{
			if (fleeTimer >= fleeCap) {
				enemy.ChangeState (new BlobIdleState ());
			} else {
				enemy.Move ();
			}
		}

		public void Enter (Enemy enemy)
		{
			this.enemy = enemy;
			enemy.Flip ();

			fleeCap = UnityEngine.Random.Range (0f, 1.8f);
			fleeTimer = 0f;
		}

		public void Exit ()
		{
			enemy.Flip ();
			enemy.StopMove ();
		}

		public void OnTriggerEnter (UnityEngine.Collider2D other)
		{
			
		}

	}
}

