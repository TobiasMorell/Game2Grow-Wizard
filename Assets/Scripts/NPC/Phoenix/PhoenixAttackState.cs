using System;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Phoenix
{
	public class PhoenixAttackState : IAIState<PhoenixController>
	{
		PhoenixController phoenix;

		public void Enter(PhoenixController pc)
		{
			this.phoenix = pc;
		}

		public void Execute()
		{
			if (phoenix.Targets.Count == 1) {
				phoenix.ChangeState (new PhoenixFollowState ());
				return;
			}
			
			GameObject enemy = phoenix.Targets [1];

			float hDiff = enemy.transform.position.x - phoenix.transform.position.x;

			if (hDiff > 7f)
				phoenix.Move (Vector2.right);
			else if (hDiff < -7f)
				phoenix.Move (Vector2.left);

			float vDiff = enemy.transform.position.y - phoenix.transform.position.y;
			if (vDiff > 0.3f)
				phoenix.Move (Vector2.up);
			else if (vDiff < -0.3f)
				phoenix.Move (Vector2.down);
			else
				phoenix.SpitFire ();
		}

		public void Exit()
		{
		}

		public void OnTriggerEnter(Collider2D other)
		{
		}
	}
}

