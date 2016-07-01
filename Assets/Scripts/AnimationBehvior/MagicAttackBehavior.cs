using UnityEngine;
using System.Collections;
using System;

public class MagicAttackBehavior : StateMachineBehaviour {
	private Wizard wizz;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		wizz = animator.GetComponent<Wizard> ();
		animator.SetBool ("Walking", false);
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//Spawns a magic bolt
		var bolt = Instantiate (wizz.Bolt);
		var boltScrpt = bolt.GetComponent<Bolt> ();
		if (boltScrpt != null) {
			boltScrpt.movingRight = wizz.facingRight;
			wizz.mana.value -= boltScrpt.cost;
		}
		bolt.gameObject.transform.position = wizz.BoltSpawnpoint.transform.position;
	}
}
