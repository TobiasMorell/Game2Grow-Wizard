using System.Collections;
using UnityEngine;
using Assets.Scripts.Enemies;

public class Platform : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Hostile")) {
			Enemy enem = other.GetComponent<Enemy> ();
			enem.Flip ();
		}
	}
}
