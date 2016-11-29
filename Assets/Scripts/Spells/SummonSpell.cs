using System;
using UnityEngine;

namespace Spells
{
	public class SummonSpell : Castable
	{
		[SerializeField] Entity entityToSummon;

		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			base.Cast (primaryTarget);
			var inst = Instantiate (entityToSummon);
			inst.transform.position = primaryTarget.transform.position - new Vector3(0, 1f);
		}
	}
}

