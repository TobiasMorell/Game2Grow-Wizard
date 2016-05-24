using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Wizard : MonoBehaviour {
	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool movingRight = false;
	[HideInInspector] public bool movingLeft = false;
	[HideInInspector] public bool jumping = true;
	[HideInInspector] public bool attacking = false;

	public float moveSpeed = 1;
	public GameObject StaffAttackPoint;
	public Transform BoltSpawnpoint;
	public GameObject Bolt;
	public float jumpForce;
	[SerializeField] private Slider health;
	public Slider mana;

    private Animator wiz_anim;
	private Rigidbody2D rb;
	private SpriteRenderer sprite_renderer;
	private float manaTimer = 0f;
	private int imuniTimer = 2;
	private bool immune = false;

	#region Attributes
	public int mana_regen_multiplier = 1;
	public int bolt_damage = 2;
	#endregion

	void Awake(){
		wiz_anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		StaffAttackPoint.SetActive (false);
	}

	// Use this for initialization
	void Start ()
    {
		Bolt.GetComponent<Bolt> ().damage = bolt_damage;
		sprite_renderer = this.GetComponent<SpriteRenderer> ();
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

		checkDeath ();
	}
	#region Control
	void handleMovement()
	{
		float h = Input.GetAxis ("Horizontal");

		if (h != 0 && !attacking) {
			wiz_anim.SetBool ("Walking", true);
			transform.position += Vector3.right * h * moveSpeed * Time.deltaTime;
		} else {
			wiz_anim.SetBool ("Walking", false);
		}

		if (h > 0 && !facingRight) {
			Flip ();
		} else if (h < 0 && facingRight) {
			Flip ();
		}
	}

	void handleMagicAttack()
	{
		if (Input.GetButtonDown ("Fire1") && mana.value >= 3)
			wiz_anim.SetTrigger ("Magic_attack");
	}

	void handleStaffAttack()
	{
		if (Input.GetButton("Fire2"))
			wiz_anim.SetBool ("Staff_attacking", true);
		else if (Input.GetButtonUp("Fire2"))
			wiz_anim.SetBool ("Staff_attacking", false);
	}
	#endregion

	public void TakeDamage(int damage)
	{
		//Todo: the incrementing of the counter must be co-routined.
		if (!immune) {
			Debug.Log ("Took damage");
			this.health.value -= damage;
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

	public void ApplyEffect(Effects effectToApply)
	{
		switch (effectToApply) {
		case Effects.Poison:
			sprite_renderer.color = Color.green;

			break;
		case Effects.Fire:

			break;
		case Effects.Freeze:

			break;
		default:
			break;
		}
	}

	private void checkDeath()
	{
		checkDeathFromFalling ();

		if (health.value <= 0)
			die ();
	}

	private void checkDeathFromFalling()
	{
		if (transform.position.y < -20) {
			die();
		}
	}

	private void die()
	{
		pointCamToSpawn();
		this.gameObject.SetActive(false);
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

	void Flip(){
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}


}
