using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	public interface IEnemyState
	{
		void Execute();
		void Enter(Enemy enemy);
		void Exit();
		void OnTriggerEnter(Collider other);
	}
}
