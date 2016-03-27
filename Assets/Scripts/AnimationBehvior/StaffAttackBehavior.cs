using UnityEngine;
using System.Collections;
using System;

public class StaffAttackBehavior : StateMachineBehaviour {
	private Wizard wizz;
	private float spawnFlameTimer;
	private float spawnFlameAfter = 0.45f;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		spawnFlameTimer = 0f;
		wizz = animator.GetComponent<Wizard> ();
		animator.SetBool ("Walking", false);
		wizz.attacking = true;
	}

	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		spawnFlameTimer += Time.deltaTime;
		if (spawnFlameTimer >= spawnFlameAfter)
			wizz.StaffAttackPoint.SetActive (true);
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		wizz.attacking = false;
		wizz.StaffAttackPoint.SetActive (false);
	}
}
