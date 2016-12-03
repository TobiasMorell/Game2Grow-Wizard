using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Enemies;

public class BatController : FlyingEnemy {

	// Use this for initialization
	public override void Awake () {
		base.Awake ();
		facingRight = false;
	}

	public override void Start ()
	{
		base.Start ();
		//Starts in Idle state
		ChangeState(new BatIdleState());
	}

	public override void Save (System.Xml.XmlWriter writer)
	{
		writer.WriteStartElement("Enemy");
		writer.WriteAttributeString("Type", "Bat");
		base.Save (writer);
		writer.WriteEndElement ();
	}
}
