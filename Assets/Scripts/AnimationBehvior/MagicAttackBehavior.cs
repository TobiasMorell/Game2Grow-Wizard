using UnityEngine;
using System.Collections;
using System;
using Spells;

public class MagicAttackBehavior : StateMachineBehaviour {
	private Wizard wizz;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		wizz = animator.GetComponent<Wizard> ();
		animator.SetBool ("Walking", false);
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}
}
