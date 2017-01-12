using UnityEngine;

namespace Spells
{
	class FireBlast : Bolt
	{
		protected override void OnTriggerEnter2D(Collider2D other)
		{
			var ent = other.GetComponent<Entity> ();
			float prev = damageModifier;

			if(ent != null && ent.HasEffect(Spells.School.Fire)) {
				damageModifier *= 2.0f;
			}
			base.OnTriggerEnter2D (other);
			damageModifier = prev;
			Destroy (this.gameObject);
		}
	}
}
