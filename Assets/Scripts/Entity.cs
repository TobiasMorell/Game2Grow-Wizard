﻿using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Effects;
using System.Xml;
using System.Collections.Generic;
using Spells;

public abstract class Entity : MonoBehaviour
{
	#region fields
	public float HP;
	[SerializeField] private float damage;
	public float Damage {
		get {
			return damage * DamageModifier;
		}
		protected set {
			damage = value;
		}
	}
	protected Resistance resistance;

	[SerializeField] protected int expForKill;
	protected int level;
	/// <summary>
	/// The melee damage modifier, 1 means normal damage.
	/// </summary>
	public float DamageModifier = 1;

	public Slider healthBar;
	[SerializeField] private float BaseSpeed;
	protected float MoveSpeed { 
		get {
			return ((100 - slowDownPercentage) / 100) * BaseSpeed;
		}
	}
	private float slowDownPercentage = 0;
	protected bool canMove = true;

	[HideInInspector] public SpriteRenderer sprite_renderer;
	protected Rigidbody2D rb;

	[HideInInspector] public bool facingRight;
	protected Animator animator;

	public Vector3 Direction {
		get { return this.facingRight ? Vector2.right : Vector2.left; }
	}
	#endregion

	#region Unity Methods
	public virtual void Awake() {
		animator = this.GetComponent<Animator> ();
		sprite_renderer = this.GetComponent<SpriteRenderer> ();
		rb = this.GetComponent<Rigidbody2D>();
		activeEffects = new List<Effect>();
		resistance = GetComponent<Resistance> ();
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

	public void SetHP(float value)
	{
		float diff = healthBar.maxValue - HP;

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
		Destroy (this.gameObject);
		ExpManager.instance.GiveExp (expForKill);
	}

	public virtual void TakeDamage(float damage, School school, bool ignoreImmunity) {
		float scaledDamage = resistance.ScaleDamage (damage, school);

		this.HP -= scaledDamage;
		this.healthBar.value = HP;

		if (HP <= 0) {
			die ();
		}
	}
	public virtual void Heal(float healing) {
		if (HP + healing > healthBar.maxValue) {
			healthBar.value = healthBar.maxValue;
			HP = healthBar.maxValue;
		} else {
			HP += healing;
			healthBar.value = HP;
		}
	}
	public void TakeDamage(float damage, School school) {
		TakeDamage (damage, school, false);
	}

	public virtual void HinderMovement() {
		canMove = false;
	}
	public virtual void ResumeMovement() {
		canMove = true;
	}
	public void SlowDown(float percentage) {
		this.slowDownPercentage += percentage;
	}
	public void SpeedUp(float percentage) {
		this.slowDownPercentage -= percentage;
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

	public bool HasEffect(School schl)
	{
		foreach (var eff in activeEffects) {
			if (eff.School == schl)
				return true;
		}

		return false;
	}

	public Effect GetEffect(School schl) {
		foreach (var eff in activeEffects) {
			if (eff.School == schl)
				return eff;
		}

		return null;
	}

	public List<Effect> GetAllEffects(School schl) {
		List<Effect> effcts = new List<Effect> ();

		foreach (var eff in activeEffects) {
			if (eff.School == schl)
				effcts.Add (eff);
		}

		return effcts;
	}
	#endregion
	#region Save & Load
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
	#endregion

	public virtual void Equip(ItemClasses.Item item) {}
}

