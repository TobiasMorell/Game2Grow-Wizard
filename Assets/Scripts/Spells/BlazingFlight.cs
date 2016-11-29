using UnityEngine;
using Assets.Scripts.UI;
using System.Collections;

namespace Spells
{
	public class BlazingFlight : Castable
	{
		Wizard wizard;
		[SerializeField] GameObject wingPrefab;
		GameObject inst;
		Rigidbody2D rb;
		IEnumerator spawnWings_Co;
		bool wingsSpawned = false;


		public override void Cast (UnityEngine.GameObject primaryTarget)
		{
			spawnWings_Co = spawnWings ();

			wizard = primaryTarget.GetComponent<Wizard> ();
			if (!wizard.jumping) {
				ErrorLogUI.Instance.LogError ("You must jump before using this spell");
				Destroy (this.gameObject);
				return;
			}
			this.transform.SetParent (wizard.transform, false);

			wizard.GetComponent<Animator> ().SetBool ("Flying", true);
			rb = wizard.GetComponent<Rigidbody2D> ();
			rb.isKinematic = true;

			StartCoroutine (spawnWings_Co);
		}

		void Update() {
			if (wingsSpawned) {
				rb.velocity = wizard.Direction * Speed;
			}
		}

		IEnumerator spawnWings() {
			yield return new WaitForSeconds (0.3f);
			inst = Instantiate (wingPrefab);
			inst.transform.SetParent (wizard.transform, false);
			wingsSpawned = true;
		}

		public override void StopCast ()
		{
			StopCoroutine (spawnWings_Co);

			wizard.GetComponent<Animator> ().SetBool ("Flying", false);
			var rb = wizard.GetComponent<Rigidbody2D> ();
			rb.isKinematic = false;
			Destroy (inst);
			Destroy (this.gameObject);
		}

		void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag ("Hostile")) {
				other.GetComponent<Entity> ().TakeDamage (calculateDamage ());
			}
		}
	}
}

