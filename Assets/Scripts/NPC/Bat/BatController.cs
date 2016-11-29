using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Enemies;

public class BatController : Enemy {
	[SerializeField] private float flapSpeed = 45;
	private BoxCollider2D groundChecker;

	// Use this for initialization
	public override void Awake () {
		base.Awake ();
		groundChecker = GetComponentInChildren<BoxCollider2D> ();
		facingRight = false;
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
		flap();
	}
		
	public void flap()
	{
		if (groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
			//anim.SetTrigger ("flap");
			rb.velocity = Vector2.up * flapSpeed * Random.Range(0.9f, 1.4f); 
		}
	}

	public override void Save (System.Xml.XmlWriter writer)
	{
		writer.WriteStartElement("Enemy");
		writer.WriteAttributeString("Type", "Bat");
		base.Save (writer);
		writer.WriteEndElement ();
	}
}
