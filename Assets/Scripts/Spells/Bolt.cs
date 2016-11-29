using UnityEngine;
using Assets.Scripts.Enemies;

namespace Spells
{
	public class Bolt : Castable
	{
		[SerializeField]
		float range;

		public override void Cast (GameObject primaryTarget)
		{
			transform.position = primaryTarget.transform.position;
			this.lifetime = range / Speed;
			Destroy(this.gameObject, lifetime);

			if (!movingRight)
				Flip();
		}
		// Update is called once per frame
		protected virtual void Update()
		{
			transform.position += transform.right * Speed * Time.deltaTime;
		}

		protected virtual void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Hostile") && !other.isTrigger)
			{
				if(particleEffect != null) 
					Instantiate(particleEffect, other.transform.position, Quaternion.identity);
				other.GetComponent<Enemy>().TakeDamage(calculateDamage());
			}
		}
	}
}
