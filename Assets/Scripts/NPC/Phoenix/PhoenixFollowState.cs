using System;
using Assets.Scripts.NPC;
using UnityEngine;

namespace Assets.Scripts.NPC.Phoenix
{
	public class PhoenixFollowState : IAIState<PhoenixController>
	{
		PhoenixController holder;
		GameObject player;
		const float FOLLOW_DISTANCE = 3f;

		public void Enter(PhoenixController pc) {
			holder = pc;
			player = holder.Targets [0];
		}

		public void Execute() {
			//Check if there are other targets than the player in the list - change to attack state if there is
			if (holder.Targets.Count > 1) {
				holder.ChangeState(new PhoenixAttackState());
				return;
			}

			float distToPlayer = player.transform.position.x - holder.transform.position.x;
			if (distToPlayer > FOLLOW_DISTANCE) {
				holder.Move (Vector2.right);
			} else if (distToPlayer < -FOLLOW_DISTANCE) {
				holder.Move (Vector2.left);
			}

			//Add code to adjust height
			float vDist = player.transform.position.y - holder.transform.position.y;
			if (vDist > -3.8f)
				holder.Move (Vector2.up);
			if (vDist < -4.2f)
				holder.Move (Vector2.down);
		}

		public void Exit() {

		}

		public void OnTriggerEnter(Collider2D other) {

		}
	}
}

