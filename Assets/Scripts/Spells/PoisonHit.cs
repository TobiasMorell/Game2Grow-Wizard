using UnityEngine;
using System.Collections;
using Assets.Scripts.Effects;

public class PoisonHit : MonoBehaviour {
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	public Sprite statusIcon;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Collision with: " + other);
		var e = other.GetComponent<Entity> ();
		if (e == null && !other.isTrigger)
			splash ();
		else if (!(e is BlobController) && !other.isTrigger) {
			StartCoroutine (entityHit (e));
			Destroy (this.GetComponent<Collider2D> ());
		}
	}

	private void splash() {
		//Stop the bolt and ask to destroy it
		this.rb.velocity = Vector2.zero;
		this.rb.isKinematic = true;

		//And play the splash animation
		this.GetComponent<Animator>().SetTrigger("HitGround");
	}

	public void Remove() {
		Destroy (this.gameObject);
	}

	IEnumerator entityHit(Entity other) {
		yield return new WaitForSeconds(0.2f);
		other.ApplyEffect (new PoisonEffect(3.5f, 4f, statusIcon));
		Destroy (this.gameObject);
	}
}
