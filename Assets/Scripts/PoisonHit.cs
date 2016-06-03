using UnityEngine;
using System.Collections;

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
		if (other.tag == "Platform" || other.tag == "Player") {
			this.rb.velocity = Vector2.zero;
			this.rb.isKinematic = true;
			this.sr.sprite = hitSprite;
			Wizard player = other.GetComponent<Wizard> ();
			if (player != null) {
				player.ApplyEffect (Effects.Poison);
			}
		}
	}
}
