using UnityEngine;
using Assets.Scripts.NPC;

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
			Debug.Log ("The bolt hit: " + other.name);
			if (other.CompareTag ("Hostile") && !other.isTrigger) {
				if (particleEffect != null)
					Instantiate (particleEffect, other.transform.position + new Vector3 (0, 2), Quaternion.identity);
				other.GetComponent<Enemy> ().TakeDamage (calculateDamage (), DatabaseInstance.School);
			} else if (other.CompareTag ("Platform")) {
				Debug.Log ("Hit the platform");
				Destroy (this.gameObject);
			}
		}
	}
}
