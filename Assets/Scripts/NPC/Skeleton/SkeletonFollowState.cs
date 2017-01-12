using UnityEngine;
using System.Collections;

namespace Assets.Scripts.NPC.Skeleton {
	public class SkeletonFollowState : IAIState<SkeletonController> {
		SkeletonController holder;
		GameObject player;

		public void Enter(SkeletonController sc) {
			holder = sc;
			player = sc.Targets [0];
		}

		public void Execute() {
			if (holder.Targets.Count > 1) {
				holder.ChangeState (new SkeletonAttackState ());
				return;
			}

			float distToPlayer = player.transform.position.x - holder.transform.position.x;
			if (distToPlayer > holder.FollowDistance) {
				holder.Move (Vector2.right);
			} else if (distToPlayer < -holder.FollowDistance) {
				holder.Move (Vector2.left);
			} else
				holder.ChangeState (new SkeletonIdleState ());
		}

		public void Exit() {

		}

		public void OnTriggerEnter(Collider2D other) {

		}
	}
}
