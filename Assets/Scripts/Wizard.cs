using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Effects;
using System.Xml;
using Assets.Scripts.Spells;
using ExtensionMethods;

enum Specialization
{
	Fire, Frost, Life, Death, Pyromancer, LavaBender, Elementalist, PhoenixLord, Hydromancer, IceLord, Necromancer, Vampire, Cleric
}

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
	private Weapon weapon;

	#region Attributes
	public int mana_regen_multiplier = 1;
	public int bolt_damage = 2;
	private int strength;

	public int Strength {
		get { return strength; }
		private set { }
	}

	Spell[] spells;

	[SerializeField]
	private Specialization Spec;
	private void changeSpec(Specialization spec)
	{
		var slots = GameObject.FindGameObjectWithTag("Spellbar").GetComponentsInChildren<SpellSlot>();
		Spell[] newSpells = spec.GetSpells();
		for (int i = 0; i < slots.Length; i++)
		{
			Debug.Log("Found spell: " + newSpells[i].Name);
			slots[i].Place(newSpells[i]);
		}
	}
	#endregion

	public bool Attacking {
		get { return attacking; }
		set { 
			attacking = value;
			if(weapon != null)
				WeaponCollider.enabled = attacking;
		}
	}
	public void SwingWeapon() {
		if(weapon != null)
			weapon.Swing ();
	}
	public void StopSwing() {
		if (weapon != null)
			weapon.StopSwing ();
	}

	public override void Awake(){
		base.Awake ();
	}

	// Use this for initialization
	public override void Start ()
    {
		base.Start ();
		Bolt.GetComponent<Bolt> ().damage = bolt_damage;
		sprite_renderer = this.GetComponent<SpriteRenderer> ();
		GameObject wep = GameObject.FindGameObjectWithTag ("Weapon");
		if (wep != null) {
			weapon = wep.GetComponent<Weapon> ();
			WeaponCollider = weapon.GetComponent<Collider2D> ();
		}

		spells = new Spell[4];
		changeSpec(Spec);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
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

	public override void TakeDamage(int damage, bool ignoreImmunity)
	{
		if (ignoreImmunity || !immune) {
			this.HP -= damage;
			this.healthBar.value = HP;

			if (HP <= 0)
				die ();

			if(!ignoreImmunity)
				StartCoroutine (handleImmunity (imuniTimer));
		}
	}
	IEnumerator handleImmunity(int immunity_cooldown)
	{
		//Show that the player is immune in some way
		this.immune = true;
		if(sprite_renderer.color == Color.white)
			this.sprite_renderer.color = Color.gray;
		yield return new WaitForSeconds (imuniTimer);
		this.immune = false;
		if(sprite_renderer.color == Color.gray)
			this.sprite_renderer.color = Color.white;
	}

	protected override void die()
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

	public void LevelUp() {

	}

	public override void Save (XmlWriter writer)
	{
		writer.WriteStartElement ("Wizard");
		base.Save (writer);
		writer.WriteEndElement ();
	}
	protected override void SaveStatus (XmlWriter writer)
	{
		writer.WriteStartElement ("Status");

		writer.WriteElementString("HP", HP.ToString());
		writer.WriteElementString ("Mana", mana.value.ToString ());
		writer.WriteElementString ("Exp", "N/A");
		writer.WriteElementString ("Level", "N/A");

		GetComponent<Inventory> ().SaveInventory (writer);
		writer.WriteEndElement();
	}

	public override void Load (XmlReader reader)
	{
		reader.ReadToFollowing ("Wizard");
		base.Load (reader);
		reader.ReadEndElement ();
	}
	protected override void LoadStatus (XmlReader reader)
	{
		base.LoadStatus (reader);
		reader.ReadToNextSibling ("Mana");
		mana.value = reader.ReadElementContentAsFloat ();
		Debug.Log("Found Exp field: " + reader.ReadToNextSibling ("Exp"));
		Debug.Log("Found level field: " + reader.ReadToNextSibling ("Level"));

		GetComponent<Inventory> ().LoadInventory (reader);
		reader.ReadEndElement ();
	}
}
