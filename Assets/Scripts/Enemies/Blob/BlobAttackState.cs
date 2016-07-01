using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	public class BlobAttackState : IEnemyState
	{
		BlobController enemy;

		float cooldownTimer = 0f;
		public static float cooldown;

		private bool onCooldown {
			get { return cooldownTimer < cooldown; }
		}

		#region IEnemyState implementation
		public void Execute ()
		{
			if (!onCooldown) {
				enemy.SpitBolt ();
				cooldownTimer = 0f;
			} else if (enemy.Target != null && Vector2.Distance (enemy.transform.position, enemy.Target.transform.position) <= enemy.minDistToPlayer)
				enemy.ChangeState (new BlobFleeState ());
			else if (enemy.Target == null)
				enemy.ChangeState (new BlobIdleState ());
			else
				cooldownTimer += Time.deltaTime;
		}
		public void Enter (Enemy enemy)
		{
			this.enemy = (BlobController) enemy;
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

