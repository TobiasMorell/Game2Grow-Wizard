using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemies {
	public class BlobIdleState : IEnemyState
	{
		Enemy enemy;
		float inStateFor = 0f;
		const float minStateTime = 2f;

		#region IEnemyState implementation
		public void Execute ()
		{
			//give it a change to move when it has been stationary for 2 secs
			if (inStateFor > minStateTime && UnityEngine.Random.value > 0.67F)
				enemy.ChangeState (new BlobMoveState ());
			else
				inStateFor += Time.deltaTime;

			//Change to attack state if blob has found the player
			if (enemy.Target != null)
				enemy.ChangeState (new BlobAttackState ());
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

