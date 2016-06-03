using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	class BatMoveState : IEnemyState
	{
		private Enemy enemy;
		private float moveTimer;
		private float moveDuration = 3f;

		public void Enter(Enemy enemy)
		{
			this.enemy = enemy;
		}

		public void Execute()
		{
			if (enemy.Target == null) {
				moveAtRandom();
			}
			else
				enemy.ChangeState (new BatAttackState ());
		}

		private void moveAtRandom()
		{
			//Move at random
			enemy.Move ();
			if (UnityEngine.Random.value > .99f)
				enemy.Flip ();
			moveTimer += Time.deltaTime;
			if (moveTimer >= moveDuration)
				enemy.ChangeState (new BatIdleState ());
		}

		public void Exit()
		{
		}

		public void OnTriggerEnter(Collider2D other)
		{
		}
	}
}
