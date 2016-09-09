using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Effects;
using System.Xml;
using Spells;
using ExtensionMethods;
using System.Collections.Generic;
using Assets.Scripts.UI;
using ItemClasses;

enum Specialization
{
	Fire, Frost, Life, Death, Pyromancer, LavaBender, Elementalist, PhoenixLord, Hydromancer, IceLord, Necromancer, Vampire, Cleric
}

public class Wizard : Entity {
	#region variables
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

	private SpellCaster caster;
	private GameObject target;

	#region Attributes
	public int mana_regen_multiplier = 1;
	private int strength, intellect, vitality;

	public void ReplaceSpell(Spell newSpell, int slot)
	{
		var slots = GameObject.FindGameObjectWithTag("Spellbar").GetComponentsInChildren<SpellSlot>();
		slots[slot].Place(newSpell);
		caster.UpdateSpells(newSpell, slot);
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
	[SerializeField] GameObject targetArrow;
	[SerializeField] GameObject weaponSlot;

	#endregion
	#region attacks and spells
	public void SwingWeapon() {
		if(weapon != null)
			weapon.Swing ();
	}
	public void StopSwing() {
		if (weapon != null)
			weapon.StopSwing ();
	}
	#endregion
	#region Unity-methods
	public override void Awake(){
		base.Awake ();
	}

	// Use this for initialization
	public override void Start ()
    {
		base.Start ();
		sprite_renderer = this.GetComponent<SpriteRenderer> ();

		caster = this.GetComponent<SpellCaster> ();
		caster.Initialize(this, new Spell[4], mana);
		ReplaceSpell(GameRegistry.SpellDatabase()["Fire Blast"], 0);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		handleMovement ();
		handleMagicAttack ();
		//handleStaffAttack ();
		if (Input.GetButtonDown ("TargetRight"))
			handleTargetRight ();
		else if (Input.GetButtonDown ("TargetLeft"))
			handleTargetLeft ();

		//Generates one mana every third second
		if (mana.value < mana.maxValue && manaTimer >= 3f) {
			mana.value += 1 * mana_regen_multiplier;
			manaTimer = 0;
		}
		else
			manaTimer += Time.deltaTime;
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
	#endregion
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
		if (Input.GetButtonUp ("Spell1"))
			caster.Cast (0, target);
		else if (Input.GetButtonUp ("Spell2"))
			caster.Cast(1, target);
		else if (Input.GetButtonUp ("Spell3"))
			caster.Cast (2, target);
		else if (Input.GetButtonUp ("Spell4"))
			caster.Cast(3, target);
	}
	#endregion
	#region Combat & death
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

	void handleTargetRight() { 
		handleTargetSwitch (1);
	}

	void handleTargetLeft() {
		handleTargetSwitch (-1);
	}

	private GameObject arrow;
	void handleTargetSwitch(int moveBy) {
		var enemies = GameObject.FindGameObjectsWithTag ("Hostile");

		//Order by x-coord so first index is lowest x-coord
		List<GameObject> list = new List<GameObject>(enemies);
		list.Sort (((GameObject x, GameObject y) => Mathf.CeilToInt(x.transform.position.x - y.transform.position.x)));

		if (target == null) {
			if (list.Count > 0) {
				target = list [0];
				placeArrow ();
			}
		} else {
			//Remove old arrow
			Destroy(arrow);

			int currIndex = list.FindIndex ((GameObject obj) => obj.GetComponent<Entity> () == target);
			int nextIndex = (currIndex + moveBy) % list.Count;
			target = list [nextIndex];
			placeArrow ();
		}
	}
	private void placeArrow() {
		//Add new arrow to target
		arrow = Instantiate (targetArrow);
		arrow.transform.localScale = new Vector3 (3f, 3f, 1);
		arrow.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + 3.5f, 0);
		arrow.transform.parent = target.transform;
	}
	#endregion



	public void LevelUp() {

	}

	/// <summary>
	/// Equip the specified item. This method also updates and maintains the equipmentUI.
	/// </summary>
	/// <param name="item">Item to equip.</param>
	public override void Equip(Item item) {
		//Det Item der equippes skal også tilføjes til UI.

		if (item.Type == ItemType.Weapon) {
			var wep = Instantiate (item.prefab);
			wep.transform.SetParent (weaponSlot.transform, false);

			if (wep != null) {
				weapon = wep.GetComponent<Weapon> ();
				WeaponCollider = weapon.GetComponent<Collider2D> ();
			}
		}
		//Instantiate armor - not added yet!!
	}

	public void Replace(Item item) {
		Unequip (item.Type);
		Equip (item);
	}

	public void Unequip(ItemType slot) {
		if (slot == ItemType.Weapon) {
			Debug.Log ("Should unequip from " + slot);

			var wep = weaponSlot.GetComponentInChildren<Weapon> ();
			if (wep != null)
				Destroy (wep.gameObject);
		}
	}

	#region Save & load
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
	#endregion
}
