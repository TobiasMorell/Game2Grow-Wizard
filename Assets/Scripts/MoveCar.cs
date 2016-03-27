using UnityEngine;
using System.Collections;

public class MoveCar : MonoBehaviour {

	public float force;
	private Rigidbody2D rb;
	public float resistance;

	private Animator anim;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float h = Input.GetAxis ("Horizontal");
		if (h != 0f) {
			rb.AddForce (Vector2.right * h * force);
			anim.SetBool ("driving", true);
		} else {
			rb.AddForce (rb.velocity * resistance);
			anim.SetBool ("driving", false);
		}
	}
}
