using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Effects;
using System.Xml;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour
{
	#region fields
	public int HP;
	[SerializeField] protected int Damage;
	[SerializeField] protected int expForKill;
	protected int level;
	public float MeleeDamageModifier;

	public Slider healthBar;
	[SerializeField] protected float MoveSpeed;
	[HideInInspector] public SpriteRenderer sprite_renderer;
	protected Rigidbody2D rb;

	[HideInInspector] public bool facingRight;
	protected Animator animator;

	protected Vector2 Direction {
		get { return this.facingRight ? Vector2.right : Vector2.left; }
	}
	#endregion

	#region Unity Methods
	public virtual void Awake() {
		animator = this.GetComponent<Animator> ();
		sprite_renderer = this.GetComponent<SpriteRenderer> ();
		rb = this.GetComponent<Rigidbody2D>();
		activeEffects = new List<Effect>();
	}

	public virtual void Start() {
		if (healthBar != null) {
			this.healthBar.maxValue = HP;
			this.healthBar.value = HP;
		}
	}

	public virtual void Update() {
		if(effectUpdate != null) effectUpdate ();
		checkDeathFromFalling ();
	}
	#endregion

	public void SetHP(int value)
	{
		int diff = Mathf.CeilToInt(healthBar.maxValue - HP);

		healthBar.maxValue = value;
		HP = value - diff;
		healthBar.value = HP;
	}

	public void PlayAnimation(string animationTriggerName)
	{
		animator.SetTrigger(animationTriggerName);
	}

	private void checkDeathFromFalling()
	{
		if (transform.position.y < -20)
			die();
	}
	protected virtual void die(){
		effectUpdate = null;
		Destroy (this.gameObject, 0f);
		ExpManager.instance.GiveExp (expForKill);
	}

	public virtual void TakeDamage(int damage, bool ignoreImmunity) {
		this.HP -= damage;
		this.healthBar.value = HP;

		if (HP <= 0) {
			die ();
		}
	}

	public virtual void Heal(int healing) {
		if (HP + healing > healthBar.maxValue) {
			healthBar.value = healthBar.maxValue;
			HP = Mathf.FloorToInt(healthBar.maxValue);
		} else {
			HP += healing;
			healthBar.value = HP;
		}
	}

	public void TakeDamage(int damage) {
		TakeDamage (damage, false);
	}

	public void Flip()
	{
		this.facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	#region Effects
	public delegate void effectUpdateDelegate();
	public effectUpdateDelegate effectUpdate;
	List<Effect> activeEffects;

	public void ApplyEffect(Effect effectToApply)
	{
		effectToApply.onApplication (this);
		activeEffects.Add(effectToApply);
	}

	public void RemoveEffect(Effect effectToRemove)
	{
		effectUpdate -= effectToRemove.onUpdate;
		sprite_renderer.color = Color.white;
		activeEffects.Remove(effectToRemove);
	}

	public bool HasEffect(EffectSchool schl)
	{
		foreach (var eff in activeEffects) {
			if (eff.School == schl)
				return true;
		}

		return false;
	}

	public Effect GetEffect(EffectSchool schl) {
		foreach (var eff in activeEffects) {
			if (eff.School == schl)
				return eff;
		}

		return null;
	}

	public List<Effect> GetAllEffects(EffectSchool schl) {
		List<Effect> effcts = new List<Effect> ();

		foreach (var eff in activeEffects) {
			if (eff.School == schl)
				effcts.Add (eff);
		}

		return effcts;
	}
	#endregion

	public virtual void Save(XmlWriter writer) {
		SavePosition (writer);
		SaveStatus (writer);
	}
	protected void SavePosition (XmlWriter writer) {
		writer.WriteStartElement ("Position");
		writer.WriteStartElement ("x");
		writer.WriteValue (transform.position.x);
		writer.WriteEndElement ();
		writer.WriteStartElement ("y");
		writer.WriteValue (transform.position.y);
		writer.WriteEndElement ();
		writer.WriteStartElement ("z");
		writer.WriteValue (transform.position.z);
		writer.WriteEndElement ();
		writer.WriteEndElement ();
	}
	protected abstract void SaveStatus (XmlWriter writer);
	public virtual void Load(XmlReader reader) {
		LoadPosition (reader);
		LoadStatus (reader);
	}
	protected virtual void LoadStatus (XmlReader reader) {
		reader.ReadToFollowing ("Status");
		reader.ReadToDescendant ("HP");
		HP = reader.ReadElementContentAsInt ();
	}
	protected void LoadPosition(XmlReader reader) {
		reader.ReadToFollowing ("Position");
		reader.ReadToDescendant ("x");
		Vector3 loadedPosition = new Vector3 ();
		loadedPosition.x = reader.ReadElementContentAsFloat ();
		reader.ReadToNextSibling ("y");
		loadedPosition.y = reader.ReadElementContentAsFloat ();
		reader.ReadToNextSibling ("z");
		loadedPosition.z = reader.ReadElementContentAsFloat ();
		transform.position = loadedPosition;
		reader.ReadEndElement ();
	}

	public virtual void Equip(ItemClasses.Item item) {}
}

