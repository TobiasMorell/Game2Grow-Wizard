using UnityEngine;
using System.Collections;

namespace Spells
{
	public abstract class SpellInstantiator : Castable
	{
		#pragma warning disable 0649 // Disable 'never assigned' warning --> assign through inspector
		[SerializeField] int minInstances, maxInstances;
		[SerializeField] Castable instancePrefab;
		[SerializeField] float timeBetweenSpawns;
		#pragma warning restore 0649

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
				inst.Cast (this.gameObject);
				formatSpawn (inst);

				if (timeBetweenSpawns > 0) {
					yield return new WaitForSeconds (timeBetweenSpawns);
				}
			}
			Destroy (this.gameObject);
		}

		/// <summary>
		/// Formats the spawn according to the behaviour of the spell.
		/// </summary>
		/// <param name="spawn">Spawn.</param>
		protected abstract void formatSpawn(Castable spawn);
	}
}

