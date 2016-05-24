using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	class BatIdleState : IEnemyState
	{
		private Enemy enemy;
		private float minIdleDuration = 2f;
		private float idleTimer = 0f;

		public void Enter(Enemy enemy)
		{
			this.enemy = enemy;
		}

		public void Execute()
		{
			//Debug.Log("Executing in Idle");

			if (enemy.Target != null)
				enemy.ChangeState (new BatAttackState ());

			//Add to the timer and give random chance that the bat will start to move
			idleTimer += Time.deltaTime;
			if (idleTimer >= minIdleDuration && UnityEngine.Random.value > 0.95f)
				enemy.ChangeState (new BatMoveState ());
		}

		public void Exit()
		{
		}

		public void OnTriggerEnter(Collider2D other)
		{
		}
	}
}
