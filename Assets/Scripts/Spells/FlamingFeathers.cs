using UnityEngine;

namespace Spells
{
	public class FlamingFeathers : SpellInstantiator
	{
		
		protected override void formatSpawn (Castable spawn)
		{
			float speedMod = Random.Range (.6f, 1.2f);
			spawn.Speed *= speedMod;
			float angle = UnityEngine.Random.Range (-15f, 15f);
			spawn.transform.Rotate (0, 0, angle);
		}
	}
}

