using UnityEngine;
using System.Collections;

namespace SMBehaviours {
	public class StandBehaviour : StateMachineBehaviour {
		Light magicLight;

		public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//Should be changed to a better solution!
			if (magicLight == null) {
				magicLight = animator.gameObject.GetComponentInChildren<Light> ();
			}

			magicLight.enabled = true;
		}

		public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			magicLight.enabled = false;
		}
	}
}