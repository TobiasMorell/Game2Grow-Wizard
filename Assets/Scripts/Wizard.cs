﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Effects;

public class Wizard : Entity {
	[HideInInspector] public bool movingRight = false;
	[HideInInspector] public bool movingLeft = false;
	[HideInInspector] public bool jumping = true;
	[HideInInspector] private bool attacking = false;

	public Transform BoltSpawnpoint;
	public GameObject Bolt;
	public float jumpForce;
	public Slider mana;

	private float manaTimer = 0f;
	private int imuniTimer = 2;
	private bool immune = false;
	private Collider2D WeaponCollider;

	#region Attributes
	public int mana_regen_multiplier = 1;
	public int bolt_damage = 2;
	private int strength;

	public int Strength {
		get { return strength; }
		private set { }
	}
	#endregion

	public bool Attacking {
		get { return attacking; }
		set { 
			attacking = value;
			WeaponCollider.enabled = attacking;
		}
	}

	delegate void effectUpdateDelegate();
	private effectUpdateDelegate effectUpdate;

	public override void Awake(){
		base.Awake ();
	}

	// Use this for initialization
	public override void Start ()
    {
		base.Start ();
		Bolt.GetComponent<Bolt> ().damage = bolt_damage;
		sprite_renderer = this.GetComponent<SpriteRenderer> ();
		WeaponCollider = GameObject.FindGameObjectWithTag ("Weapon").GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		handleMovement ();
		handleMagicAttack ();
		handleStaffAttack ();

		//Generates one mana every third second
		if (mana.value < mana.maxValue && manaTimer >= 3f) {
			mana.value += 1 * mana_regen_multiplier;
			manaTimer = 0;
		}
		else
			manaTimer += Time.deltaTime;

		checkDeathFromFalling ();
		if(effectUpdate != null) effectUpdate ();
	}
	#region Control
	void handleMovement()
	{
		float h = Input.GetAxis ("Horizontal");

		if (h != 0 && !attacking) {
			animator.SetBool ("Walking", true);
			transform.position += Vector3.right * h * MoveSpeed * Time.deltaTime;
		} else
			animator.SetBool ("Walking", false);

		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
	}

	void handleMagicAttack()
	{
		if (Input.GetButtonDown ("Fire1") && mana.value >= 3)
			animator.SetTrigger ("Magic_attack");
	}

	void handleStaffAttack()
	{
		if (Input.GetButton("Fire2"))
			animator.SetBool ("Staff_attacking", true);
		else if (Input.GetButtonUp("Fire2"))
			animator.SetBool ("Staff_attacking", false);
	}
	#endregion

	public override void TakeDamage(int damage)
	{
		if (!immune) {
			this.HP -= damage;
			this.healthBar.value = HP;

			if (HP <= 0)
				die ();

			StartCoroutine (handleImmunity (imuniTimer));
		}
	}
	IEnumerator handleImmunity(int immunity_cooldown)
	{
		//Show that the player is immune in some way
		this.immune = true;
		this.sprite_renderer.color = Color.gray;
		yield return new WaitForSeconds (imuniTimer);
		this.immune = false;
		this.sprite_renderer.color = Color.white;
	}

	public void ApplyEffect(HostileEffect effectToApply)
	{
		effectToApply.onApplication (this);
		effectUpdate += effectToApply.onUpdate;
	}

	private void checkDeathFromFalling()
	{
		if (transform.position.y < -20)
			die();
	}

	private void die()
	{
		pointCamToSpawn();
		Destroy (this.gameObject, 0f);
	}

	private void pointCamToSpawn()
	{
		var cam = GetComponentInChildren<Camera> ();
		if (cam != null) {
			cam.transform.parent = null;
			CamController cc = cam.GetComponent<CamController> ();
			if (cc != null) {
				cc.startPoint = this.transform.position;
				cc.inTransistion = true;
			}
		}
	}

	void FixedUpdate(){
        if (Input.GetButtonDown("Jump") && !jumping)
        {
            jumping = true;
            rb.AddForce(Vector2.up * jumpForce);
        }
        else if (!Input.GetButton("Jump") && rb.velocity.y < Mathf.Epsilon)
            jumping = false;
            
    }
}
