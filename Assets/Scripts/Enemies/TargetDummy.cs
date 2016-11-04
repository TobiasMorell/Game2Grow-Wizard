using System;
using UnityEngine;

public class TargetDummy : Enemy
{
	public override void TakeDamage (int damage, bool ignoreImmunity)
	{
		//Do nothing, the dummy does not take damage
		Debug.Log("TargetDummy took " + damage + " damage");
		GameRegistry.ItemDropFabric.Drop (GameRegistry.ItemDatabase ["Mote of Fire"], 1, transform.position);
	}
}

