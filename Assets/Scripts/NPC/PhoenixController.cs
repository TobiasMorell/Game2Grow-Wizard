using UnityEngine;
using Spells;
using System.Collections.Generic;
using Assets.Scripts.Enemies;

namespace Phoenix {
	public class PhoenixController : AIEntity<PhoenixController>
	{
		const byte BOLT = 0;
		const byte HEAL = 1;
		public bool Summoned = false;

		#region fields
		Spell[] spellPrefabs;
		[HideInInspector] public SpellCaster spellCaster;

		public List<GameObject> Targets;
		public float AttackRange { get; private set; }

		IAIState<PhoenixController> state;
		#endregion

		public override void ChangeState (IAIState<PhoenixController> newState)
		{
			//Exits old state
			if(CurrentState != null)
				CurrentState.Exit();

			//Enters new
			CurrentState = newState;
			CurrentState.Enter(this);
		}

		public override void Awake ()
		{
			base.Awake ();
			spellCaster = this.GetComponent<SpellCaster> ();
			facingRight = false;

			Targets = new List<GameObject> ();
			Targets.Add (GameObject.FindGameObjectWithTag ("Player"));
		}

		public override void Start() {
			base.Start ();

			spellPrefabs = new Spell[2];
			spellPrefabs [BOLT] = GameRegistry.SpellDatabase [34];
			spellPrefabs [HEAL] = GameRegistry.SpellDatabase [35];
			spellCaster.Initialize (this, spellPrefabs, 20);

			ChangeState (new PhoenixFollowState ());
		}

		public override void Update() {
			if (Summoned) {
				CurrentState.Execute ();

				if (!spellPrefabs [HEAL].OnCooldown ()) {
					spellCaster.Cast (HEAL, Targets [0]);
				}
			}
		}

		public void Move(Vector3 direction) {
			if (facingRight && direction == Vector3.left)
				Flip ();
			else if (!facingRight && direction == Vector3.right)
				Flip ();

			transform.position += direction * Time.deltaTime * MoveSpeed;
		}

		public void SpitFire() {
			if (!spellPrefabs [BOLT].OnCooldown ()) {
				spellCaster.Cast (BOLT, null);
			}
		}

		protected override void die ()
		{
			base.die ();
			animator.SetBool ("Dead", true);
			Destroy (this.gameObject, 10);
		}

		protected override void SaveStatus (System.Xml.XmlWriter writer)
		{
			throw new System.NotImplementedException ();
		}
	}
}

