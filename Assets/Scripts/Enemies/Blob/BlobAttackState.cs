using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	public class BlobAttackState : IEnemyState
	{
		BlobController enemy;

		float cooldownTimer = 0f;
		const float cooldown = 4f;

		private bool onCooldown {
			get { return cooldownTimer < cooldown; }
		}

		#region IEnemyState implementation
		public void Execute ()
		{
			if (enemy.inAttackRange && !onCooldown) {
				attack ();
				cooldownTimer = 0f;
			}
			else if (enemy.Target == null)
				enemy.ChangeState (new BatIdleState ());
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

		private void attack()
		{
			enemy.SpitBolt ();
		}
	}
}

