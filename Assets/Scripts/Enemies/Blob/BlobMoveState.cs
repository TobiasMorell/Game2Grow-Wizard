using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	public class BlobMoveState : IEnemyState
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
			Debug.Log ("On trigger enter in MoveState: " + other.tag);
			if(other.tag.Equals("Platform")) {
				enemy.Flip();
			}
		}
		#endregion
		
	}
}

