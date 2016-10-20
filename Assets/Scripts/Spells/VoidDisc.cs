using Spells;
using System;
using UnityEngine;

public class VoidDisc : Bolt
{
	[SerializeField] float rotationsPrSec;
	protected override void Update ()
	{
		base.Update ();
		transform.Rotate (0, 0, Time.deltaTime * rotationsPrSec, Space.Self);
	}
}

