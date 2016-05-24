using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Bolt : MonoBehaviour {
	[SerializeField] private float speed;
	public int damage;
	public int cost = 3;
	[SerializeField] private float lifetime = 1.5f;
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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Hostile") {
			//Maybe some particle effect

			other.GetComponent<Enemy>().TakeDamage (damage);
		}
	}
}
