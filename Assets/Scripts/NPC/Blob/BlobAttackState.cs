using System;
using UnityEngine;

namespace Assets.Scripts.NPC.Blob
{
	public class BlobAttackState : IAIState<Enemy>
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
			} else if (enemy.Targets[0] != null && Vector2.Distance (enemy.transform.position, enemy.Targets[0].transform.position) <= enemy.minDistToPlayer)
				enemy.ChangeState (new BlobFleeState ());
			else if (enemy.Targets[0] == null)
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

