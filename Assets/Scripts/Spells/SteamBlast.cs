using System;

namespace Spells
{
	public class SteamBlast : SingleTarget
	{
		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			base.Cast (primaryTarget);
			primaryTarget.GetComponent<Entity> ().TakeDamage (calculateDamage (), DatabaseInstance.School);
		}
	}
}

