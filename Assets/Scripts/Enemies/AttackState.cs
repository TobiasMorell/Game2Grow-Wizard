using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	class AttackState : IEnemyState
	{
		private Enemy enemy;

		public void Enter(Enemy enemy)
		{
			Debug.Log ("Entered AttackState");
			this.enemy = enemy;
		}

		public void Execute()
		{
			Debug.Log ("Executing in AttackState");

			if (enemy.Target == null)
				enemy.ChangeState (new IdleState ());

			followPlayer ();
		}

		private void followPlayer()
		{
			float xDir = enemy.Target.transform.position.x - enemy.transform.position.x;

			if (xDir < 0 && enemy.facingRight || xDir > 0 && !enemy.facingRight)
				enemy.ChangeDirection ();

			enemy.Move ();
		}

		public void Exit()
		{
			Debug.Log ("Exited AttackState");
		}

		public void OnTriggerEnter(Collider2D other)
		{
			Debug.Log ("OnTriggerEnter in AttackState");
		}
	}
}
