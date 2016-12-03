using UnityEngine;

namespace Spells
{
	public class BreakIce : Castable
	{
		public override void Cast (GameObject primaryTarget)
		{
			base.Cast (primaryTarget);
			var ps = GetComponent<ParticleSystem> ();
			ps.Play();
			Destroy (this, ps.duration);
		}

		void OnParticleCollision(GameObject other) {
			Debug.Log ("Particle Collision");

			if (other.CompareTag ("Hostile")) {
				Debug.Log ("The Shockwave hit " + other.name);
			}
		}
	}
}

