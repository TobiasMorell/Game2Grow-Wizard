using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

		public void Cast(int index, GameObject target) {
			Spell toCast = spells [index];

			//Todo: should rather be a float reference
			float remainingMana;
			if (manaUI != null) {
				remainingMana = manaUI.value;
			} else {
				remainingMana = mana;
			}

			Debug.Log (toCast.Name + " onCooldown: " + toCast.OnCooldown ());

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
			}
		}

		void Update() {
			foreach (var spell in spells) {
				spell.cooldownTimer -= Time.deltaTime;
			}
		}
	}
}
