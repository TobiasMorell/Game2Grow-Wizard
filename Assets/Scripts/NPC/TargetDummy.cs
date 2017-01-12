using System;
using UnityEngine;
using Spells;

namespace Assets.Scripts.NPC {
	public class TargetDummy : Enemy
	{
		public override void TakeDamage (float damage, School school, bool ignoreImmunity)
		{
			//Do nothing, the dummy does not take damage
			Debug.Log("TargetDummy took " + damage + " damage");
			//	GameRegistry.ItemDropFabric.Drop (GameRegistry.ItemDatabase ["Mote of Fire"], 1, transform.position);
		}
	}
}

