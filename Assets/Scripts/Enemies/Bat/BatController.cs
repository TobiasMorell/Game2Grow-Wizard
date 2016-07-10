using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Enemies;

public class BatController : Enemy {
	[SerializeField] private float flapForce = 45;
	private BoxCollider2D groundChecker;

	// Use this for initialization
	public override void Awake () {
		base.Awake ();
		groundChecker = GetComponentInChildren<BoxCollider2D> ();
		facingRight = true;
	}

	public override void Start ()
	{
		base.Start ();
		//Starts in Idle state
		ChangeState(new BatIdleState());
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		//Checks if it's necesarry to flap
		flap(Random.Range(0.9f, 1.4f));
	}
		
	public void flap(float forceScale)
	{
		if (groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
			//anim.SetTrigger ("flap");
			rb.AddForce (Vector2.up * flapForce * forceScale * (rb.drag * 3));
		}
	}
}
