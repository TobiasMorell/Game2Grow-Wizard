using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	class MoveState : IEnemyState
	{
		private Enemy enemy;
		private float moveTimer;
		private float moveDuration = 3f;

		public void Enter(Enemy enemy)
		{
			Debug.Log ("Entered FollowState");
			this.enemy = enemy;
		}

		public void Execute()
		{
			//Debug.Log ("Executing in FollowState");

			if (enemy.Target == null) {
				//Move at random
				enemy.Move ();
				if (UnityEngine.Random.value > .99f)
					enemy.ChangeDirection ();
				moveTimer += Time.deltaTime;
				if (moveTimer >= moveDuration)
					enemy.ChangeState (new IdleState ());
			}
			else
				enemy.ChangeState (new AttackState ());
		}

		public void Exit()
		{
			Debug.Log ("Exited FollowState");
		}

		public void OnTriggerEnter(Collider2D other)
		{
			Debug.Log ("OnTriggerEnter in FollowState");
		}
	}
}
