using System;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	public abstract class AIEntity<T> : Entity
	{
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
	}
}

