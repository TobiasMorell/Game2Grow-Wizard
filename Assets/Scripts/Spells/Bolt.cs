using UnityEngine;

namespace Spells
{
	public class Bolt : Castable
	{
		[SerializeField]
		float range;

		// Use this for initialization
		protected virtual void Start()
		{
			this.lifetime = range / speed;
			Destroy(this.gameObject, lifetime);

			if (!movingRight)
				Flip();
		}
		// Update is called once per frame
		protected virtual void Update()
		{
			transform.position += Vector3.right * speed * Time.deltaTime;
		}

		protected virtual void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Hostile" && !other.isTrigger)
			{
				if(particleEffect != null)
					Instantiate(particleEffect, transform.position, Quaternion.identity);
				other.GetComponent<Enemy>().TakeDamage(calculateDamage());
			}
		}
	}
}
