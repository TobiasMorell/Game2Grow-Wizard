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
	[SerializeField] private Slider mana;

    private Animator wiz_anim;
	private Rigidbody2D rb;
	private float manaTimer = 0f;
	private float imuniTimer = 0f;
	public bool immune = false;

	void Awake(){
		wiz_anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		StaffAttackPoint.SetActive (false);
	}

	// Use this for initialization
	void Start ()
    {
		Bolt.GetComponent<Bolt> ().damage = 2;
	}
	
	// Update is called once per frame
	void Update () {
		handleMovement ();
		handleMagicAttack ();
		handleStaffAttack ();

		//Generates one mana every third second
		if (mana.value < mana.maxValue && manaTimer >= 3f) {
			mana.value++;
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
		if (Input.GetButtonDown ("Fire1") && mana.value >= 3) {
			wiz_anim.SetTrigger ("Magic_attack");
			mana.value -= 3;
		} 
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
		if (!immune) {
			this.health.value -= damage;
			this.immune = true;
			this.imuniTimer = 0f;

			//Show that the player is immune in some way
		} else if (immune && imuniTimer < 2f)
			imuniTimer += Time.deltaTime;
		else
			immune = false;
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
