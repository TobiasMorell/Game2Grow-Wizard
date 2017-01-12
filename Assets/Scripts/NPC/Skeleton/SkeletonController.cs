using UnityEngine;
using System.Collections;

namespace Assets.Scripts.NPC.Skeleton {
	public class SkeletonController : AIEntity<SkeletonController> {
		public float AttackRange;
		public float FollowDistance;

		public override void ChangeState (IAIState<SkeletonController> newState)
		{
			if (CurrentState != null)
				CurrentState.Exit ();

			//Enter new state
			CurrentState = newState;
			CurrentState.Enter (this);
		}

		public override void Awake ()
		{
			base.Awake ();

			facingRight = true;
		}

		protected override void SaveStatus (System.Xml.XmlWriter writer)
		{
			throw new System.NotImplementedException ();
		}
	}
}