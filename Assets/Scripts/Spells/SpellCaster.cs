using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.UI;

namespace Spells 
{
	
	public class SpellCaster : MonoBehaviour {
		private Spell[] spells;
		[HideInInspector] public float mana;
		private Entity holder;
		private Slider manaUI;

		public void Initialize(Entity holder, Spell[] newSpells, float maxMana) {
			spells = newSpells;
			this.holder = holder;
			mana = maxMana;
		}
		public void Initialize(Entity holder, Spell[] newSpells, Slider manaUI) {
			spells = newSpells;
			this.holder = holder;
			this.manaUI = manaUI;
		}
		public void UpdateSpells(Spell newSpell, int slot) {
			spells[slot] = newSpell;
			var slots = GameObject.FindGameObjectWithTag("Spellbar").GetComponentsInChildren<SpellSlot>();
			slots[slot].Place(newSpell);
		}

		public void Cast(int index, GameObject target) {
			Spell toCast = spells [index];

			//Todo: should rather be a float reference
			float remainingMana;
			if(manaUI != null)
				remainingMana = manaUI.value;
			else
				remainingMana = mana;

			if (!toCast.OnCooldown () && remainingMana >= toCast.Cost) {
				remainingMana -= toCast.Cost;

				if (toCast.TargetsSelf || !toCast.RequiresTarget) {
					toCast.Cast (holder.gameObject);
				} else if (toCast.RequiresTarget) {
					if (target != null) {
						toCast.Cast (target);
					} else {
						Debug.Log ("Requires a target to cast");
					}
				}

				if (manaUI != null)
					manaUI.value = remainingMana;
				else
					mana = remainingMana;
			}
		}

		void Update() {
			foreach (var spell in spells) {
				if(spell != null)
					spell.cooldownTimer -= Time.deltaTime;
			}
		}
	}
}
