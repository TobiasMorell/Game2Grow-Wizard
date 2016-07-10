using System;
using UnityEngine;

namespace Assets.Scripts.Effects {

	public class HostileEffect
	{
		protected float duration;
		protected float timer;
		protected int Strength { get; set; }
		protected Entity bearer;

		public HostileEffect(float duration) {
			this.duration = duration;
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
			Strength = 1;
			bearer.effectUpdate += onUpdate;
		}

		public virtual void onEffectEnded() {
			bearer.effectUpdate -= onUpdate;
		}
	}
}


