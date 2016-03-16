using Assets.Scripts.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class Enemy : MonoBehaviour
	{
		private IEnemyState currentState;

		protected virtual void Start()
		{
			//Starts in Idle state
			ChangeState(new IdleState());
		}

		protected virtual void Update()
		{
			currentState.Execute();
		}

		public void ChangeState(IEnemyState newState)
		{
			//Exits old state
			if(currentState != null)
			{
				currentState.Exit();
			}

			//Enters new
			currentState = newState;
			currentState.Enter(this);
		}

	}
}
