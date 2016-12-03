using System;

namespace Spells
{
	public class Bubble : SingleTarget
	{
		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			base.Cast (primaryTarget);
			primaryTarget.GetComponent<Entity> ().HinderMovement ();
			this.transform.SetParent (primaryTarget.transform, true);
			transform.Translate (0, 1f, 0);
			Destroy (this.gameObject, lifetime);
		}

		void OnDestroy() {
			if (target)
				target.GetComponent<Entity> ().ResumeMovement ();
		}
	}
}

