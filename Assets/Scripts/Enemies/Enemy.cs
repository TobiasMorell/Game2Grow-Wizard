﻿using Assets.Scripts.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public abstract class Enemy : MonoBehaviour
	{
		public float MovementSpeed;
		public int attackDamage;
		[SerializeField] protected float attackRange;
		[SerializeField] protected Slider healthBar;

		protected IEnemyState currentState;
		[HideInInspector] public bool facingRight;
		protected Animator anim;

		[SerializeField] protected int hp;

		protected Vector2 Direction
		{
			get {
				return facingRight ? Vector2.right : Vector2.left;
			}
		}
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

		public void TakeDamage(int damage)
		{
			this.hp -= damage;
			this.healthBar.value = hp;

			if (hp < 0)
				Destroy (this.gameObject);
		}

		protected virtual void Start()
		{
			anim = this.GetComponent<Animator> ();

			this.healthBar.maxValue = hp;
			this.healthBar.value = hp;

			facingRight = true;
		}

		protected virtual void Update()
		{
			currentState.Execute();
		}

		public void ChangeDirection()
		{
			facingRight = !facingRight;
			transform.localScale = new Vector3 (transform.localScale.x * -1, 1, 1);
		}

		public void ChangeState(IEnemyState newState)
		{
			//Exits old state
			if(currentState != null)
			{
				currentState.Exit();
			}

			//Enters new
			currentState = newState;
			currentState.Enter(this);
		}

		public void PlayAnimation(string animatorString)
		{
			anim.SetTrigger (animatorString);
		}

		public virtual void Move()
		{
			//anim.SetBool ("moving", true);

			transform.Translate (Direction * MovementSpeed * Time.deltaTime);
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			currentState.OnTriggerEnter (other);
		}

	}
}
