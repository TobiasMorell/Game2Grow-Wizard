using UnityEngine;
using System.Collections;

public class IgnoreCollision : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), player.GetComponent<Collider2D> ());
	}
}
