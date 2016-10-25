using UnityEngine;
using System.Collections;

namespace Spells {

	public class SingleTarget : Castable {

		void Start() {
			Destroy (this.gameObject, lifetime);
		}

		public override void Cast (GameObject primaryTarget)
		{
			base.Cast (primaryTarget);
			if (particleEffect != null)
				Instantiate(particleEffect, transform.position, Quaternion.identity);
		}
	}
}
