using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Enemies;

public class BlobController : Enemy {
	[SerializeField] GameObject poisonBoltPrefab;
	[SerializeField] Transform poisonBoltSpawn;
	public float minDistToPlayer = 5;
	[SerializeField] float spitCooldown;

	// Use this for initialization
	public override void Awake () {
		base.Awake();
		facingRight = false;
	}

	public override void Start ()
	{
		base.Start ();
		BlobAttackState.cooldown = spitCooldown;
		ChangeState (new BlobIdleState ());
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	public void SpitBolt()
	{
		//Instantiate bolt and get rigidbody
		GameObject pbInst = Instantiate<GameObject> (poisonBoltPrefab);
		pbInst.transform.position = poisonBoltSpawn.position;
		Rigidbody2D boltRb = pbInst.GetComponent<Rigidbody2D> ();

		//Give bolt a 45 degree angle and calculate the needed velocity of the bolt
		Vector2 angle = (Direction + Vector2.up);
		float dist = Vector2.Distance (transform.position, Target.transform.position);
		float velocity = Mathf.Sqrt (dist / 0.102f);
		float Vx = velocity * Mathf.Cos(45f * Mathf.Deg2Rad);

		//And fire it with the calculated velocity
		boltRb.velocity = Vx * angle;
	}

	void OnTriggerEnter2D(Collider2D other) {
		currentState.OnTriggerEnter (other);
	}

	protected override void SaveStatus (System.Xml.XmlWriter writer)
	{
		writer.WriteStartElement("Enemy");
		writer.WriteAttributeString("Type", "Blob");
		base.Save (writer);
		writer.WriteEndElement ();
	}

	public override void Move ()
	{
		base.Move ();
	}
}
