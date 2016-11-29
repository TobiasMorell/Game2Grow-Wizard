using UnityEngine;

namespace Spells
{
	public class Phoenix : Castable
	{
		GameObject phoenixPrefab;

		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			Instantiate (phoenixPrefab, transform.position, Quaternion.identity);
		}
	}
}

