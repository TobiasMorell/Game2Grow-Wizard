using UnityEngine;
using System.Collections;

public class Wizard : MonoBehaviour {
	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool movingRight = false;
	[HideInInspector] public bool movingLeft = false;

	public float moveSpeed = 1;
	public GameObject StaffAttackPoint;
	public Transform BoltSpawnpoint;
	public GameObject Bolt;

	private Animator wiz_anim;
	private Rigidbody2D rb;
	private bool attacking = false;

	void Awake(){
		wiz_anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		StaffAttackPoint.SetActive (false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		handleMovement ();
		handleMagicAttack ();
		handleStaffAttack ();

		checkDeath ();
	}
	#region attack
	void handleMovement()
	{
		float h = Input.GetAxis ("Horizontal");

		if (h != 0 && !attacking) {
			wiz_anim.SetBool ("Walking", true);
			transform.position += Vector3.right * h * moveSpeed * Time.deltaTime;
		}
		else {
			wiz_anim.SetBool ("Walking", false);
		}

		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
	}

	void handleMagicAttack()
	{
		if (Input.GetButtonDown ("Fire1")) {
			wiz_anim.SetBool ("Walking", false);
			wiz_anim.SetTrigger ("Magic_attack");
			attacking = true;

			//Spawn a bolt at end of hand after animation
			StartCoroutine(waitAndSpawnBolt(0.45f));
		} 
	}

	void handleStaffAttack()
	{
		if (Input.GetButtonDown ("Fire2")) {
			wiz_anim.SetBool ("Walking", false);
			wiz_anim.SetBool ("Staff_attacking", true);
			attacking = true;

			StartCoroutine(waitAndSpawnFlames (0.45f));
		} else if (!Input.GetButton ("Fire2") && StaffAttackPoint.activeSelf) {
			wiz_anim.SetBool ("Staff_attacking", false);
			attacking = false;

			StaffAttackPoint.SetActive (false);
		}
	}

	IEnumerator waitAndSpawnFlames(float waittime)
	{
		yield return new WaitForSeconds (waittime);
		StaffAttackPoint.SetActive (true);
	}

	IEnumerator waitAndSpawnBolt(float waittime)
	{
		yield return new WaitForSeconds (waittime);
		var bolt = Instantiate (Bolt);
		var boltScrpt = bolt.GetComponent<Bolt> ();
		if (boltScrpt != null)
			boltScrpt.movingRight = facingRight;
		attacking = false;
		bolt.gameObject.transform.position = BoltSpawnpoint.transform.position;
	}
	#endregion

	private void checkDeath()
	{
		checkDeathFromFalling ();
	}

	private void checkDeathFromFalling()
	{
		if (transform.position.y < -20) {
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
	}
	void FixedUpdate(){
		
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
