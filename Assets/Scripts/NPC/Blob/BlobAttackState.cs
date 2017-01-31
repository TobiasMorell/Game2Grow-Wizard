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
			if (enemy.Targets.Count == 0) {
				enemy.ChangeState (new BlobIdleState ());
				return;
			}

			if (!onCooldown) {
				enemy.SpitBolt ();
				cooldownTimer = 0f;
			} else if (Vector2.Distance (enemy.transform.position, enemy.Targets[0].transform.position) <= enemy.minDistToPlayer)
				enemy.ChangeState (new BlobFleeState ());
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

