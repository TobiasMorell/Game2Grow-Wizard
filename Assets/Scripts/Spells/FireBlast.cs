using UnityEngine;
using System.Collections;

namespace Spells {

	public class FireBlast : Castable {
		[SerializeField] float degreesPrSec;
		[SerializeField] float lifetime;

		void Start() {
			Destroy (this.gameObject, lifetime);
		}

		// Update is called once per frame
		void Update () {
			transform.Rotate(0, 0, degreesPrSec * Time.deltaTime);
		}

		public override void Cast (GameObject primaryTarget)
		{
			base.Cast (primaryTarget);
			//Something to calculate damage here
			int damage = 3;

			//if(primaryTarget.HasEffect(Effects.Burning)) {
			// damage = Mathf.Ceil(1.5f * damage);
			//}
			primaryTarget.GetComponent<Entity>().TakeDamage(damage);
		}
	}
}
