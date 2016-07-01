using UnityEngine;
using System.Collections;
using Assets.Scripts.Effects;

public class PoisonHit : MonoBehaviour {
	private Rigidbody2D rb;
	private SpriteRenderer sr;

	public Sprite hitSprite;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
		sr = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Platform")
			splash ();
		else if (other.tag == "Player")
			StartCoroutine (playerHit (other.gameObject));
	}

	private void splash() {
		//Stop the bolt and ask to destroy it
		this.rb.velocity = Vector2.zero;
		this.rb.isKinematic = true;
		Destroy (this.gameObject, 0.5f);

		//And change the sprite to a splash
		this.sr.sprite = hitSprite;
	}

	IEnumerator playerHit(GameObject other) {
		yield return new WaitForSeconds(0.1f);

		splash ();
		Wizard player = other.GetComponent<Wizard> ();
		if (player != null)
			player.ApplyEffect (new PoisonEffect(3.5f));
	}
}
