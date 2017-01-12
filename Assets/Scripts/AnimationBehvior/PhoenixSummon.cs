using System;
using UnityEngine;
using Assets.Scripts.NPC.Phoenix;

namespace SMBehaviours
{
	public class PhoenixSummon : StateMachineBehaviour
	{
		public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.GetComponent<PhoenixController> ().Summoned = true;
		}
	}
}

