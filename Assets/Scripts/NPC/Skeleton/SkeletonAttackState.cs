using UnityEngine;
using System.Collections;

namespace Assets.Scripts.NPC.Skeleton {
	public class SkeletonAttackState : IAIState<SkeletonController> {
		SkeletonController holder;

		public void Enter(SkeletonController sc) {
			this.holder = sc;
		}

		public void Execute() {
			if (holder.Targets.Count == 1) {
				holder.ChangeState (new SkeletonFollowState ());
				return;
			}

			GameObject enemy = holder.Targets [1];

			//Move towards the enemy
			float hDiff = enemy.transform.position.x - holder.transform.position.x;
			if (hDiff > holder.AttackRange)
				holder.Move (Vector2.right);
			else if (hDiff < -holder.AttackRange)
				holder.Move (Vector2.left);
			else {
				//Attack the target.
			}

		}

		public void Exit () {

		}

		public void OnTriggerEnter (Collider2D other) {

		}
	}
}
