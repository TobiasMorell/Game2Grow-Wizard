using UnityEngine;
using System.Collections;
using Assets.Scripts.Effects;


namespace Spells
{
	public class Ignite : SingleTarget
	{

		public override void Cast(GameObject primaryTarget)
		{
			base.Cast(primaryTarget);
			primaryTarget.GetComponent<Entity>().ApplyEffect(new FireEffect(10.0f, calculateDamage(), null));
		}
	}
}
