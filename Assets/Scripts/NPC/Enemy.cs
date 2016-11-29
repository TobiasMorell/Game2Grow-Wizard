using Assets.Scripts.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Enemies {
	public abstract class Enemy : AIEntity<Enemy>
	{
		[SerializeField] protected float attackRange;
		public int EnemyDamage { 
			get { return this.Damage; }
			protected set {
				this.Damage = value;
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
		public override void ChangeState (IAIState<Enemy> newState)
		{
			//Exits old state
			if(CurrentState != null)
				CurrentState.Exit();

			//Enters new
			CurrentState = newState;
			CurrentState.Enter(this);
		}
		public virtual void Move()
		{
			animator.SetBool ("Moving", true);
			transform.position += Direction * MoveSpeed * Time.deltaTime;
		}
		public virtual void StopMove() {
			animator.SetBool ("Moving", false);
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
}
