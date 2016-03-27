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
		private Vector2 previousPosition = Vector2.zero;
		private Vector2 targetPosition;

		private bool onCooldown = false;
		private bool attacking = false;

		public void Enter(Enemy enemy)
		{
			this.enemy = enemy;
		}

		public void Execute()
		{
			//Debug.Log ("enemy.inAttackRange is " + enemy.inAttackRange);
			if (enemy.inAttackRange && !onCooldown)
				attack ();
			else if (onCooldown)
				cooldown();
			else if (enemy.Target == null)
				enemy.ChangeState (new IdleState ());
			else if (!attacking && !onCooldown) 
				followPlayer ();
		}

		private void followPlayer()
		{
			float xDir = enemy.Target.transform.position.x - enemy.transform.position.x;

			if (xDir < 0 && enemy.facingRight || xDir > 0 && !enemy.facingRight)
				enemy.ChangeDirection ();

			enemy.Move ();
		}

		private void attack()
		{
			//Setup target and indicate start of attack
			if (!attacking) {
				targetPosition = enemy.Target.transform.position;
				previousPosition = enemy.transform.position;

				attacking = true;
			}

			//Move the sprite closer to the player and trigger cooldown, when close enough
			enemy.transform.position = Vector2.Lerp (enemy.transform.position, targetPosition, 0.2f);

			if (Vector2.Distance (enemy.transform.position, targetPosition) < 0.1f) {
				attacking = false;
				onCooldown = true;
			}
		}

		private void cooldown()
		{
			enemy.transform.position = Vector2.Lerp (enemy.transform.position, previousPosition, 0.2f);

			if (Vector2.Distance (enemy.transform.position, previousPosition) < 0.01f) {
				//Debug.Log ("Reset cooldown - ready to attack");
				onCooldown = false;
			}
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
