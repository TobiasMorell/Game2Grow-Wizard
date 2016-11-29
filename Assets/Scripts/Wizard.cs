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
using Assets.Scripts.Weapon;
using Assets.Scripts.Inventory;

public class Wizard : Entity {
	#region variables
	[HideInInspector] public bool movingRight = false;
	[HideInInspector] public bool movingLeft = false;
	[HideInInspector] public bool jumping = true;
	[HideInInspector] private bool attacking = false;

	public Transform BoltSpawnpoint;
	//public GameObject Bolt;
	public float jumpSpeed;
	public Slider mana;

	private float manaTimer = 0f;
	private int imuniTimer = 2;
	private bool immune = false;
	public Weapon weapon;

	private SpellCaster caster;
	private AttributeManager attributes;
	private GameObject target;

	public bool Attacking {
		get { return attacking; }
		set { 
			attacking = value;
		}
	}
	[SerializeField] GameObject targetArrow;
	[SerializeField] GameObject weaponSlot;

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

		//Initialize caster component
		caster = this.GetComponent<SpellCaster> ();
		caster.Initialize(this, new Spell[4], mana);

		//Initialize AttributeManager
		attributes = this.GetComponent<AttributeManager>();
		attributes.AssignSkillpoints (5);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();

		if (GameRegistry.Typing)
			return;
		
		handleMovement ();
		handleMagicAttack ();

		if (Input.GetButtonDown ("TargetRight"))
			handleTargetRight ();
		else if (Input.GetButtonDown ("TargetLeft"))
			handleTargetLeft ();
	}

	void FixedUpdate(){
		if (Input.GetButtonDown("Jump") && !jumping)
		{
			jumping = true;
			rb.velocity += Vector2.up * jumpSpeed;
			animator.SetBool ("Jumping", true);
		}
	}

	public void HitGround() {
		animator.SetBool("Jumping", false);
		jumping = false;
	}
	#endregion
	#region Control
	/// <summary>
	/// Handles keystrokes regarding the movement.
	/// </summary>
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

	/// <summary>
	/// Handles leystrokes regarding magic attacks.
	/// </summary>
	void handleMagicAttack()
	{
		if (Input.GetButtonDown ("Spell1"))
			caster.Cast (0, target);
		else if (Input.GetButtonDown ("Spell2"))
			caster.Cast(1, target);
		else if (Input.GetButtonDown ("Spell3"))
			caster.Cast (2, target);
		else if (Input.GetButtonDown ("Spell4"))
			caster.Cast(3, target);

		if (Input.GetButtonUp ("Spell1"))
			caster.StopCast (0);
		else if (Input.GetButtonUp ("Spell2"))
			caster.StopCast(1);
		else if (Input.GetButtonUp ("Spell3"))
			caster.StopCast (1);
		else if (Input.GetButtonUp ("Spell4"))
			caster.StopCast(2);
	}

	void melee() {
		if (false /*Should be replaces with button*/) {
			weapon.Swing ();
		}
	}
	#endregion
	#region Combat & death
	/// <summary>
	/// Take damage and update all UI regarding it.
	/// </summary>
	/// <param name="damage">Damage to deal to the wizard.</param>
	/// <param name="ignoreImmunity">If set to <c>true</c> ignore immunity.</param>
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
	/// <summary>
	/// Handles the immunity timer.
	/// </summary>
	/// <returns>The immunity.</returns>
	/// <param name="immunity_cooldown">Ammount of time the wizard should stay immune.</param>
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

	/// <summary>
	/// Kill the wizard.
	/// </summary>
	protected override void die()
	{
		pointCamToSpawn();
		Destroy (this.gameObject, 0f);
	}

	/// <summary>
	/// Points the cam to spawn.
	/// </summary>
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

	/// <summary>
	/// Handle target-switch right.
	/// </summary>
	void handleTargetRight() { 
		handleTargetSwitch (1);
	}

	/// <summary>
	/// Handle target-switch left.
	/// </summary>
	void handleTargetLeft() {
		handleTargetSwitch (-1);
	}
		
	private GameObject arrow;
	/// <summary>
	/// Handles the target switch - targets are sorted by their x-position.
	/// </summary>
	/// <param name="moveBy">Move by (-1 means left, 1 means right).</param>
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

			int currIndex = list.FindIndex ((GameObject obj) => obj == target);
			int nextIndex = Mathf.Abs((currIndex + moveBy) % list.Count);
			target = list [nextIndex];
			placeArrow ();
		}
	}
	/// <summary>
	/// Places the target-arrow.
	/// </summary>
	private void placeArrow() {
		//Add new arrow to target
		arrow = Instantiate (targetArrow);
		arrow.transform.localScale = new Vector3 (3f, 3f, 1);
		arrow.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + 6f, 0);
		arrow.transform.parent = target.transform;
	}
	#endregion

	/// <summary>
	/// Level up the wizard.
	/// </summary>
	public void LevelUp() {
		attributes.LevelUp();
	}
		
	private int calculateDamage() {
		return attributes.strength;
	}

	/// <summary>
	/// Equip the specified item. This method also updates and maintains the equipmentUI.
	/// </summary>
	/// <param name="item">Item to equip.</param>
	public override void Equip(Item item) {
		attributes.RegisterEquip (item);

		if (item.Type == ItemType.Weapon) {
			var wep = Instantiate (item.prefab);
			wep.transform.SetParent (weaponSlot.transform, false);

			if (wep != null) {
				weapon = wep.GetComponent<Weapon> ();
				weapon.Equip (calculateDamage ());
			}
		}
		if(item.Type == ItemType.Offhand)
		{

		}
		if(item.Type == ItemType.Armor)
		{

		}
		//Instantiate armor - not added yet!!
	}
	/// <summary>
	/// Replace the item in the slot of the specified item.
	/// </summary>
	/// <param name="item">The new item to equip.</param>
	public void Replace(Item item) {
		Unequip (item.Type);
		Equip (item);
	}

	/// <summary>
	/// Unequip the item in the specified slot.
	/// </summary>
	/// <param name="slot">Slot to remove from.</param>
	public void Unequip(ItemType slot) {
		if (slot == ItemType.Weapon) {
			Debug.Log ("Should unequip from " + slot);

			var wep = weaponSlot.GetComponentInChildren<Weapon> ();
			if (wep != null) {
				Destroy (wep.gameObject);
				attributes.RegisterUnequip(GameRegistry.ItemDatabase[wep.Id]);
			}
		}
	}

	#region Save & load
	/// <summary>
	/// Save this instance using the specified writer.
	/// </summary>
	/// <param name="writer">Writer.</param>
	public override void Save (XmlWriter writer)
	{
		writer.WriteStartElement ("Wizard");
		base.Save (writer);
		writer.WriteEndElement ();
	}
	/// <summary>
	/// Saves the status.
	/// </summary>
	/// <param name="writer">Writer.</param>
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

	/// <summary>
	/// Load an instance using the specified reader.
	/// </summary>
	/// <param name="reader">Reader.</param>
	public override void Load (XmlReader reader)
	{
		reader.ReadToFollowing ("Wizard");
		base.Load (reader);
		reader.ReadEndElement ();
	}
	/// <summary>
	/// Loads the status.
	/// </summary>
	/// <param name="reader">Reader.</param>
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
