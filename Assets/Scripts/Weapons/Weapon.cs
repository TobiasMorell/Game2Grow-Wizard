using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] string weaponName;
	[SerializeField] int damage;
	[SerializeField] int durability;
	[SerializeField] bool rangedWeapon;
	[SerializeField] bool repeatAnimation;
	[SerializeField] GameObject projectile;
	[SerializeField] AnimationClip swingAnimation;

	Wizard player;

	public Weapon (Wizard player) 
	{
		this.player = player;
		if (!repeatAnimation)
			swingAnimation.wrapMode = WrapMode.Once;
		else
			swingAnimation.wrapMode = WrapMode.Loop;
	}

	public void Swing() {
		if (rangedWeapon) {
			Instantiate (projectile);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Hostile")) {
			other.GetComponent<Enemy> ().TakeDamage (damage + player.Strength);

			this.durability -= 1;

			if (durability <= 0)
				Destroy (this.gameObject);
		}
	}
}

