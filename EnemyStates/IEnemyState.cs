using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnemyStates
{
	public interface IEnemyState
	{
		void Enter(Enemy enemy);
		void Execute();
		void Exit();
		void OnTriggerEnter(UnityEngine.Collider2D other);
	}
}
