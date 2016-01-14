using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour {
	public float speed;
	public float lifetime = 1.5f;
	[HideInInspector] public bool movingRight;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, lifetime);
		if (!movingRight)
			Flip ();
	}

	private void Flip()
	{
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		speed *= -1;
		this.transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.right * speed * Time.deltaTime;
	}
}
