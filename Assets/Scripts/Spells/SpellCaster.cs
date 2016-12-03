using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.UI;

namespace Spells 
{
	public class SpellCaster : MonoBehaviour {
		private Spell[] spells;

		[HideInInspector] public float mana;
		float maxMana;
		public Slider manaUI;

		private Entity holder;
		[SerializeField] GameObject magicAttackPoint;

		IEnumerator channelManaDrain;
		bool channeling;

		public float DamageModifier;
		public float ManaRegenPrTick;
		[SerializeField] float ManaRegenInterval;
		bool generatingMana;

		public bool[] SpellToggle = new bool[4] { false, false, false, false };

		public void Initialize(Entity holder, Spell[] newSpells, float maxMana) {
			spells = newSpells;
			this.holder = holder;
			mana = maxMana;
			this.maxMana = mana;
			StartCoroutine (generateMana ());
		}
		public void Initialize(Entity holder, Spell[] newSpells, Slider manaUI) {
			spells = newSpells;
			this.holder = holder;
			this.manaUI = manaUI;
			this.mana = manaUI.maxValue;
			this.maxMana = mana;
			StartCoroutine (generateMana ());
		}
		public void UpdateSpells(Spell newSpell, int slot) {
			spells[slot] = newSpell;
			var slots = GameObject.FindGameObjectWithTag("Spellbar").GetComponentsInChildren<SpellSlot>();
			slots[slot].Place(newSpell);
		}

		public void Cast(int index, GameObject target) {
			Spell toCast = spells [index];

			//Check for errors that prevent spell-casting (only if player)
			if (holder is Wizard) {
				if (toCast == null)
					return;
				if (toCast.OnCooldown ()) {
					ErrorLogUI.Instance.LogError (toCast.Name + " is on cooldown.");
					return;
				}
				if (toCast.RequiresTarget && target == null) {
					ErrorLogUI.Instance.LogError ("This spell requires a target to cast.");
					return;
				}
				//Find out if there is enough mana for a spellcast and display eventual errors.
				if (mana < toCast.Cost) {
					ErrorLogUI.Instance.LogError ("There is not enough mana to cast " + toCast.Name);
					return;
				}
				if (channeling) {
					ErrorLogUI.Instance.LogError ("Cannot cast spells while channeling");
					return;
				}
			}

			//Check if the spell being cast is toggled and stop it if it is already active
			if (toCast.Toggled && SpellToggle [index]) {
				ToggleOff (index);
				return;
			}
			
			//Actually cast the spell
			mana -= toCast.Cost;
			holder.PlayAnimation(toCast.AnimationType.ToString());

			//Determines which type of spell is being cast and chose the target accordingly.
			if (toCast.TargetsSelf)
				toCast.Cast(holder.gameObject, holder.facingRight, DamageModifier);
			else if (!toCast.RequiresTarget)
				toCast.Cast(magicAttackPoint, holder.facingRight, DamageModifier);
			else if (toCast.RequiresTarget)
			{
				//We know there is a target from the first check.
				toCast.Cast(target, holder.facingRight, DamageModifier);
			}

			if (toCast.Channeled) {
				channelManaDrain = drainMana (toCast.Cost);
				StartCoroutine (channelManaDrain);
				channeling = true;
			} else if (toCast.Toggled) {
				SpellToggle [index] = true;
			}
		}

		IEnumerator drainMana (float manaPrSec) {
			while (true) {
				yield return new WaitForSeconds (1);
				mana -= manaPrSec;
			}
		}
		IEnumerator generateMana() {
			while (generatingMana && !channeling) {
				if (mana + ManaRegenPrTick >= maxMana)
					mana = maxMana;
				else
					mana += ManaRegenPrTick;
				yield return new WaitForSeconds (ManaRegenInterval);
			}
		}

		public void StopCast(int index) {
			Spell toStop = spells [index];

			if (toStop == null)
				return;

			if (toStop.Channeled) {
				toStop.StopCast ();
				StopCoroutine (channelManaDrain);
				channeling = false;
			} 
		}

		private void ToggleOff(int index) {
			Spell spell = spells [index];
			spell.StopCast ();
			SpellToggle [index] = false;
		}

		void Update() {
			foreach (var spell in spells) {
				if(spell != null)
					spell.cooldownTimer -= Time.deltaTime;
			}

			if (manaUI)
				manaUI.value = mana;
		}

		public void ToggleManaRegeneration(bool newState) {
			generatingMana = newState;
		}
	}
}
