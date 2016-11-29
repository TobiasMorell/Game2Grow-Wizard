using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	public interface IAIState<T>
	{
		void Execute();
		void Enter(T enemy);
		void Exit();
		void OnTriggerEnter(Collider2D other);
	}
}
