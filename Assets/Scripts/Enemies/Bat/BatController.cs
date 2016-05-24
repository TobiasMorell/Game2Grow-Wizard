using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Enemies;

public class BatController : Enemy {
	[SerializeField] private float flapForce;
	private BoxCollider2D groundChecker;

	private Rigidbody2D rb;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		rb = GetComponent<Rigidbody2D> ();
		groundChecker = GetComponentInChildren<BoxCollider2D> ();

		//Starts in Idle state
		ChangeState(new BatIdleState());
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

		//Checks if it's necesarry to flap
		flap(Random.Range(0.9f, 1.4f));
	}

	public void flap(float forceScale)
	{
		if (groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
			Debug.Log ("Bat intended to flap!");

			//anim.SetTrigger ("flap");
			rb.AddForce (Vector2.up * flapForce * forceScale * (rb.drag * 3));
		}
	}
}
