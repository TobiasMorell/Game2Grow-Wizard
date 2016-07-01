using System;
using UnityEngine;

namespace Assets.Scripts.Effects {

	public class HostileEffect
	{
		protected float duration;
		protected float timer;
		float Strength { get; set; }
		private Entity bearer;

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
		}

		public virtual void onApplication (Entity entity){
			this.bearer = entity;
		}
	}
}


