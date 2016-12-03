using UnityEngine;

namespace Spells
{
	public class Blizzard : SpellInstantiator
	{
		#pragma warning disable 0649 // Disable 'never assigned' warning --> assign through inspector
		[SerializeField] float minXOffset, maxXOffset;
		#pragma warning restore 0649

		public override void Cast (GameObject primaryTarget)
		{
			base.Cast (primaryTarget);
			transform.position = primaryTarget.transform.position + new Vector3(0, 7);
			Destroy (this.gameObject, lifetime);
		}

		protected override void formatSpawn (Castable spawn)
		{
			float offset = Random.Range (minXOffset, maxXOffset);
			Debug.Log ("Offset was: " + offset);

			Vector3 randPos = transform.position + new Vector3(offset, 0);
			spawn.transform.position = randPos;
		}
	}
}

