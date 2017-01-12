using System.Collections;
using UnityEngine;
using Assets.Scripts.NPC;

public class Platform : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Hostile")) {
			Enemy enem = other.GetComponent<Enemy> ();
			enem.Flip ();
		}
	}
}
