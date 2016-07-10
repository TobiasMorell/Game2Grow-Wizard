using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Bolt : MonoBehaviour {
	[SerializeField] protected float speed;
	public int damage;
	public int cost = 3;
	[SerializeField] protected float lifetime = 1.5f;
	[HideInInspector] public bool movingRight;

	// Use this for initialization
	protected virtual void Start () {
		Destroy (this.gameObject, lifetime);
		if (!movingRight)
			Flip ();
	}

	protected virtual void Flip()
	{
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		speed *= -1;
		this.transform.localScale = scale;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		transform.position += Vector3.right * speed * Time.deltaTime;
	}

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Hostile" && !other.isTrigger) {
			//Maybe some particle effect
			Debug.Log ("On trigger enter in MoveState: " + other.tag);
			other.GetComponent<Enemy>().TakeDamage (damage);
		}
	}
}
