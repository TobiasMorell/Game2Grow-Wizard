﻿using System;
using UnityEngine;
using Assets.Scripts.UI;

namespace Assets.Scripts.Effects {
	public enum EffectSchool { Fire, Life, Water, Ice, Death, Poison };

	[Serializable]
	public class Effect
	{
		[SerializeField] protected float duration;
		protected float timer;
		protected Entity bearer;
		[SerializeField] protected Sprite icon;
		public EffectSchool School;
		protected string description;


		public Effect(float duration, Sprite icon) {
			this.duration = duration;
			this.icon = icon;
		}

		protected bool IsOver
		{
			get { return timer >= duration; }
		}

		public virtual void onUpdate()
		{
			timer += Time.deltaTime;
			if (IsOver) {
				onEffectEnded();
			}
		}

		public virtual void onApplication (Entity entity){
			this.bearer = entity;
			bearer.effectUpdate += onUpdate;

			if (entity.CompareTag("Player"))
				UIStatusManager.Instance.AddDebuff(icon, duration, description);
		}

		public virtual void onEffectEnded() {
			bearer.RemoveEffect(this);
		}
	}
}


