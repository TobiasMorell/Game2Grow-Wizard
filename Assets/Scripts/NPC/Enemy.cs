using Assets.Scripts.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.NPC {
	public abstract class Enemy : AIEntity<Enemy>
	{
		[SerializeField] protected float attackRange;

		public virtual bool inAttackRange{
			get {
				if (Targets[0] != null) {
					return Vector2.Distance (transform.position, Targets[0].transform.position) <= attackRange;
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
			if(canMove)
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
