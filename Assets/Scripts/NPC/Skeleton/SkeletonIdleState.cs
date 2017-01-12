using UnityEngine;
using System.Collections;

namespace Assets.Scripts.NPC.Skeleton {
	public class SkeletonIdleState : IAIState<SkeletonController> {
		SkeletonController holder;

		public void Enter (SkeletonController sc) {
			this.holder = sc;
		}

		public void Execute() {
			//Transition to attack
			if (holder.Targets.Count > 1) {
				holder.ChangeState (new SkeletonAttackState ());
				return;
			}

			//Transition to follow
			float hDiff = holder.Targets[0].transform.position.x - holder.transform.position.x;
			if (Mathf.Abs (hDiff) > holder.FollowDistance) {
				holder.ChangeState (new SkeletonFollowState ());
				return;
			}
			
		}

		public void Exit() {

		}

		public void OnTriggerEnter(Collider2D other) {

		}
	}
}
