﻿using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other) {
		if (tag.Equals ("Hostile")) {
			Enemy enem = other.GetComponent<Enemy> ();
			enem.Flip ();
		}
	}
}
