using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	public class BlobMoveState : IEnemyState
	{
		Enemy enemy;
		float moveTime = UnityEngine.Random.Range(0.2f, 2.4f);
		float stateTimer;

		#region IEnemyState implementation
		public void Execute ()
		{
			enemy.Move ();
			stateTimer += Time.deltaTime;
		}
		public void Enter (Enemy enemy)
		{
			this.enemy = enemy;
		}
		public void Exit ()
		{
			
		}
		public void OnTriggerEnter (UnityEngine.Collider2D other)
		{
		}
		#endregion
		
	}
}

