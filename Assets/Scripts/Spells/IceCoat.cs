using UnityEngine;

namespace Spells
{
	public class IceCoat : Castable
	{
		GameObject ShieldParticles;

		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			this.target = primaryTarget;
			ShieldParticles = Instantiate (particleEffect);
			ShieldParticles.transform.SetParent (primaryTarget.transform, false);

			primaryTarget.GetComponent<Wizard> ().ToggleImmunity (true);
			primaryTarget.GetComponent<SpellCaster> ().ToggleManaRegeneration (false);
		}

		public override void StopCast ()
		{
			target.GetComponent<Wizard>().ToggleImmunity(false);
			target.GetComponent<SpellCaster> ().ToggleManaRegeneration (true);
			Destroy (ShieldParticles.gameObject);
			Destroy (this.gameObject);
		}
	}
}

