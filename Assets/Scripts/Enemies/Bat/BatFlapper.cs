using UnityEngine;
using System.Collections;

public class BatFlapper : MonoBehaviour {
	private BatController bat;

	void Awake()
	{
		bat = GetComponentInParent<BatController> ();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Platform")
			bat.flap (Random.Range(0.8f, 1.4f));
	}
}
