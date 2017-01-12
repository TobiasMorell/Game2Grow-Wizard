using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.NPC
{
	public abstract class AIEntity<T> : Entity
	{
		public List<GameObject> Targets;

		#region AIState methods
		protected IAIState<T> CurrentState;
		public abstract void ChangeState (IAIState<T> newState);
		#endregion


		public override void Update ()
		{
			base.Update ();

			if(CurrentState != null)
				CurrentState.Execute();
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if(CurrentState != null)
				CurrentState.OnTriggerEnter (other);
		}

		public void Move(Vector3 direction) {
			if (facingRight && direction == Vector3.left)
				Flip ();
			else if (!facingRight && direction == Vector3.right)
				Flip ();

			if(canMove)
				transform.position += direction * Time.deltaTime * MoveSpeed;
		}
	}
}

