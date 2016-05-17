using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	class BatAttackState : IEnemyState
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

		public void method_name(int number_parameter){
			Console.WriteLine (number_parameter + 5);
		}

		public void Execute()
		{
			//Debug.Log ("enemy.inAttackRange is " + enemy.inAttackRange);
			if (enemy.inAttackRange && !onCooldown)
				attack ();
			else if (onCooldown)
				cooldown();
			else if (enemy.Target == null)
				enemy.ChangeState (new BatIdleState ());
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
				//Store target and current position (used for a smooth attack animation)
				targetPosition = enemy.Target.transform.position;
				previousPosition = enemy.transform.position;

				//Disable ground-checker and physics-based rigidbody to prevents physics going crazy
				enemy.GetComponentInChildren<BoxCollider2D>().enabled = false;
				enemy.GetComponent<Rigidbody2D> ().isKinematic = true;

				//Set the boolean attacking (such that the bat cannot attack twice in a row
				attacking = true;

				//Play attack animation
				//enemy.PlayAnimation("attack");
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
			//Move sprite back from the player to the previous position
			enemy.transform.position = Vector2.Lerp (enemy.transform.position, previousPosition, 0.2f);

			if (Vector2.Distance (enemy.transform.position, previousPosition) < 0.01f) {
				onCooldown = false;

				//reset rigidbody and collider to resume normal behaviour
				enemy.GetComponentInChildren<BoxCollider2D>().enabled = true;
				enemy.GetComponent<Rigidbody2D> ().isKinematic = false;
			}
		}

		public void Exit()
		{		}

		public void OnTriggerEnter(Collider2D other)
		{
			if (other.tag == "Player")
				other.GetComponent<Wizard>().TakeDamage (enemy.attackDamage);
		}
	}
}
