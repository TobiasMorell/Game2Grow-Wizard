using System;
using UnityEngine;
using UnityEditor;

public class Weapon : MonoBehaviour
{
	[SerializeField] public string weaponName;
	[SerializeField] int damage;
	[SerializeField] int durability;
	[SerializeField] bool rangedWeapon;
	[SerializeField] bool repeatAnimation;
	[SerializeField] GameObject projectile;
	[SerializeField] AnimationClip swingAnimation;
	private Collider2D _collider;
	private GameObject proj;

	Wizard player;

	void Start() {
		this.player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Wizard>();
		_collider = this.GetComponent<Collider2D> ();
	}

	public void Swing() {
		if (rangedWeapon) {
			proj = Instantiate (projectile);
			Vector3 pos = _collider.transform.position;
			if (player.facingRight) {
				pos.x += 3.2f;
			} else {
				pos.x -= 3.2f;
			}
			pos.y += 0.25f;
			Debug.Log ("Setting position of " + proj + " to " + pos);
			proj.gameObject.transform.position = pos;
			Bolt b = proj.GetComponent<Bolt> ();
			if (b != null) {
				b.movingRight = player.facingRight;
				b.damage = damage;
			} else {
				MadnessBeam mb = proj.GetComponent<MadnessBeam> ();
				if (!player.facingRight) {
					Vector3 scale = proj.transform.localScale;
					scale.x *= -1;
					proj.transform.localScale = scale;
				}
				if (mb != null)
					mb.damage = damage;
			}
		}
		durability--;
		if (durability <= 0) {
			Destroy (this.gameObject);
			Destroy (this);
		}
	}
	public void StopSwing() {
		if (!repeatAnimation) {
			Destroy (proj);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Hostile") && !other.isTrigger) {
			other.GetComponent<Enemy> ().TakeDamage (damage);

			this.durability -= 1;

			if (durability <= 0)
				Destroy (this.gameObject);
		}
	}
}

