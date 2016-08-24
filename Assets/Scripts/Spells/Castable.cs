using System;
using UnityEngine;

namespace Spells
{
	public abstract class Castable : MonoBehaviour
	{
		protected GameObject target;

		public virtual void Cast(GameObject primaryTarget) {
			this.target = primaryTarget;
			transform.position = target.transform.position;
		}
	}
}

