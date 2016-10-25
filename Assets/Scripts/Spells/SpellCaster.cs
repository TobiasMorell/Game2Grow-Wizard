using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.UI;

namespace Spells 
{
	public enum SpellAnimationType
	{
		Swing, FrontalSpell
	}
	public class SpellCaster : MonoBehaviour {
		private Spell[] spells;
		[HideInInspector] public float mana;
		private Entity holder;
		public Slider manaUI;
		[SerializeField] GameObject magicAttackPoint;

		public float damageModifier { get; set; }

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

			//Check for errors that prevent spell-casting
			if (toCast == null)
			{
				ErrorLogUI.Instance.LogError("There is no spell in that slot.");
				return;
			}
			if(toCast.OnCooldown())
			{
				ErrorLogUI.Instance.LogError(toCast.Name + " is on cooldown.");
				return;
			}
			if (toCast.RequiresTarget && target == null)
			{
				ErrorLogUI.Instance.LogError("This spell requires a target to cast.");
				return;
			}

			//Find out if there is enough mana for a spellcast and display eventual errors.
			float remainingMana;
			if(manaUI != null)
				remainingMana = manaUI.value;
			else
				remainingMana = mana;
			if(remainingMana < toCast.Cost)
			{
				ErrorLogUI.Instance.LogError("There is not enough mana to cast " + toCast.Name);
				return;
			}
			
			//Actually cast the spell
			remainingMana -= toCast.Cost;
			holder.PlayAnimation(toCast.AnimationType.ToString());

			//Determines which type of spell is being cast and chose the target accordingly.
			if (toCast.TargetsSelf)
				toCast.Cast(holder.gameObject, holder.facingRight, damageModifier);
			else if (!toCast.RequiresTarget)
				toCast.Cast(magicAttackPoint, holder.facingRight, damageModifier);
			else if (toCast.RequiresTarget)
			{
				//We know there is a target from the first check.
				toCast.Cast(target, holder.facingRight, damageModifier);
			}

			if (manaUI != null)
				manaUI.value = remainingMana;
			else
				mana = remainingMana;
		}

		void Update() {
			foreach (var spell in spells) {
				if(spell != null)
					spell.cooldownTimer -= Time.deltaTime;
			}
		}
	}
}
