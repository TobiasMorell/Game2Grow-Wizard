using System;
using UnityEngine;

namespace Assets.Scripts.NPC.Blob
{
	public class BlobMoveState : IAIState<Enemy>
	{
		Enemy enemy;
		float moveTime = UnityEngine.Random.Range(3f, 5.6f);
		float stateTimer;

		#region IEnemyState implementation
		public void Execute ()
		{
			enemy.Move ();
			stateTimer += Time.deltaTime;

			if (stateTimer > moveTime)
				enemy.ChangeState (new BlobIdleState ());

			//Change to attack state if blob has found the player
			if (enemy.Targets.Count > 0)
				enemy.ChangeState (new BlobAttackState ());
		}
		public void Enter (Enemy enemy)
		{
			this.enemy = enemy;
		}
		public void Exit ()
		{
			enemy.StopMove ();
		}
		public void OnTriggerEnter (UnityEngine.Collider2D other)
		{
			if(other.tag.Equals("Platform")) {
				enemy.Flip();
			}
		}
		#endregion
		
	}
}

