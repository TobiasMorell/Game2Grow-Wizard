using UnityEngine;

namespace Spells
{
	class FireBlast : Bolt
	{
		protected override void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Hostile" && !other.isTrigger)
			{
				if (other.GetComponent<Entity>().HasEffect(Assets.Scripts.Effects.EffectSchool.Fire))
					damageModifier *= 2.0f;
				other.GetComponent<Enemy>().TakeDamage(calculateDamage());
			}
		}
	}
}
