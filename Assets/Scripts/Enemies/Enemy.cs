using Assets.Scripts.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Entity
{
	[SerializeField] protected float attackRange;
	public int EnemyDamage { 
		get { return this.Damage; }
		protected set {
			this.Damage = value;
		}
	}

	protected IEnemyState currentState;

	public GameObject Target { get; set; }
	public virtual bool inAttackRange{
		get {
			if (Target != null) {
				return Vector2.Distance (transform.position, Target.transform.position) <= attackRange;
			}
			//No target:
			return false;
		}
	}

	public override void Awake()
	{
		base.Awake ();
		facingRight = true;
	}

	public override void Update()
	{
		base.Update ();
		currentState.Execute();
	}

	public void ChangeState(IEnemyState newState)
	{
		//Exits old state
		if(currentState != null)
			currentState.Exit();

		//Enters new
		currentState = newState;
		currentState.Enter(this);
	}

	public virtual void Move()
	{
		animator.SetBool ("Moving", true);
		transform.Translate (Direction * MoveSpeed * Time.deltaTime);
	}
	public virtual void StopMove() {
		animator.SetBool ("Moving", false);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(currentState != null)
			currentState.OnTriggerEnter (other);
	}

	protected override void SaveStatus (System.Xml.XmlWriter writer)
	{
		writer.WriteElementString ("HP", HP.ToString ());
	}

	protected override void LoadStatus (System.Xml.XmlReader reader)
	{
		base.LoadStatus (reader);
		reader.ReadEndElement ();
	}
}
