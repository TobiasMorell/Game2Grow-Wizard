using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	class IdleState : IEnemyState
	{
		public void Enter(Enemy enemy)
		{
			throw new NotImplementedException();
		}

		public void Execute()
		{
			Debug.Log("Executing in Idle");
		}

		public void Exit()
		{
			throw new NotImplementedException();
		}

		public void OnTriggerEnter(Collider other)
		{
			throw new NotImplementedException();
		}
	}
}
