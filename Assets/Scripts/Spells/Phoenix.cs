using UnityEngine;

namespace Spells
{
	public class Phoenix : Castable
	{
		#pragma warning disable 0649 // Disable 'never assigned' warning --> assign through inspector
		GameObject phoenixPrefab;
		#pragma warning restore 0649

		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			Instantiate (phoenixPrefab, transform.position, Quaternion.identity);
		}
	}
}

