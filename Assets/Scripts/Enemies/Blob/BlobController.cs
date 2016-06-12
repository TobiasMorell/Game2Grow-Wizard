using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Enemies;

public class BlobController : Enemy {
	[SerializeField] GameObject poisonBoltPrefab;
	[SerializeField] Transform poisonBoltSpawn;
	[SerializeField] float boltBaseVelocity = 3;
	[SerializeField] float minDistToPlayer = 5;

	private float fleeTimer = 3f;
	private float fleeTime = 1f;

	private bool fleeing {
		get { return fleeTime > fleeTimer; }
	}

	// Use this for initialization
	public override void Awake () {
		base.Awake();
		facingRight = false;
	}

	public override void Start ()
	{
		base.Start ();
		ChangeState (new BlobIdleState ());
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (Target != null && Vector2.Distance (transform.position, Target.transform.position) <= minDistToPlayer)
			FleeFrom (Target);
		
		if (!fleeing)
			base.Update ();
		else {
			Move ();
			fleeTime += Time.deltaTime;
			Debug.Log ("Blob is fleeing");
		}
	}

	public void SpitBolt()
	{
		GameObject pbInst = Instantiate<GameObject> (poisonBoltPrefab);
		pbInst.transform.position = poisonBoltSpawn.position;

		//Calculate the needed velocity of the bolt
		Vector2 v = (Vector2.up + Direction) * boltBaseVelocity;

		pbInst.GetComponent<Rigidbody2D> ().velocity = v;
	}
	public void FleeFrom(GameObject hostileTarget){
		float myX = transform.position.x;
		float hostX = hostileTarget.transform.position.x;

		if ((myX > hostX && !facingRight) || (myX <= hostX && facingRight)) {
			Flip ();
			fleeTimer = Random.Range (1.8f, 3f);
			fleeTime = 0;
		}
	}
}
