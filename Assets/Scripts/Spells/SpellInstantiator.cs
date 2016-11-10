using UnityEngine;
using System.Collections;

namespace Spells
{
	public abstract class SpellInstantiator : Castable
	{
		[SerializeField] int minInstances, maxInstances;
		[SerializeField] Castable instancePrefab;
		[SerializeField] float timeBetweenSpawns;

		public override void Cast (GameObject primaryTarget)
		{
			base.Cast (primaryTarget);

			int numBolts = Random.Range (minInstances, maxInstances + 1);
			StartCoroutine (spawnInstances (numBolts));
		}

		protected IEnumerator spawnInstances(int number) {
			for (int i = 0; i < number; i++) {
				var inst = Instantiate<Castable> (instancePrefab);
				inst.movingRight = movingRight;
				inst.transform.position = transform.position;
				inst.AssignModifier (damageModifier);
				formatSpawn (inst);

				yield return new WaitForSeconds (timeBetweenSpawns);
			}
			//Destroy (this.gameObject);
		}

		/// <summary>
		/// Formats the spawn according to the behaviour of the spell.
		/// </summary>
		/// <param name="spawn">Spawn.</param>
		protected abstract void formatSpawn(Castable spawn);
	}
}

