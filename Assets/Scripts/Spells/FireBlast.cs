using UnityEngine;
using System.Collections;
using Assets.Scripts.Spells;

public class FireBlast : Spell {
	Enemy target;
	
	// Update is called once per frame
	void Update () {
		transform.position = target.transform.position;
		transform.Rotate(0, 0, 20);
	}

	public override Spell Cast(Entity primaryTarget)
	{
		base.Cast(primaryTarget);
	}
}
