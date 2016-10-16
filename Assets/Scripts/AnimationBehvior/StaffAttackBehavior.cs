using UnityEngine;
using System.Collections;
using System;

public class StaffAttackBehavior : StateMachineBehaviour {
	private Wizard wizz;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		wizz = animator.GetComponent<Wizard> ();
		animator.SetBool ("Walking", false);
		wizz.Attacking = true;
	}

	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		wizz.weapon.StopSwing ();
		wizz.Attacking = false;
	}
}
