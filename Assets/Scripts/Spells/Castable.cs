﻿using System;
using UnityEngine;

namespace Spells
{
	public abstract class Castable : MonoBehaviour
	{
		[SerializeField] protected Spell DatabaseInstance;
		public void BindCastableToSpell(Spell spell) {
			DatabaseInstance = spell;
		}

		protected GameObject target;
		[SerializeField] protected int BaseDamage;
		public float Speed;
		[SerializeField]protected float lifetime;
		[SerializeField]protected GameObject particleEffect;

		[HideInInspector]public bool movingRight;
		protected float damageModifier = 1.0f;

		protected virtual void Flip()
		{
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			Speed *= -1;
			this.transform.localScale = scale;
		}

		public virtual void Cast(GameObject primaryTarget) {
			this.target = primaryTarget;
			transform.position = target.gameObject.transform.position;
		}

		public virtual void StopCast() {

		}

		public void AssignModifier(float mod)
		{
			this.damageModifier = mod;
		}

		protected int calculateDamage()
		{
			return Mathf.CeilToInt(damageModifier * BaseDamage);
		}
	}
}

