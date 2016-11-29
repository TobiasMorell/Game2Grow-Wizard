using System.Collections;
using UnityEngine;

namespace Spells
{
	public class HealingSpell : Castable
	{
		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			primaryTarget.GetComponent<Entity> ().Heal (calculateDamage ());
			transform.position = primaryTarget.transform.position + new Vector3(0, 3);
			target = primaryTarget;
			Destroy (this.gameObject, lifetime);
		}

		void Update () {
			this.transform.position += new Vector3 (0, Speed * Time.deltaTime);
		}
	}
}

