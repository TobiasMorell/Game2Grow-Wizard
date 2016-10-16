using System;
using Spells;
using UnityEngine;
using Assets.Scripts.UI;

namespace ItemClasses
{
	public class CrystalHelper : MonoBehaviour
	{
		[SerializeField] Sprite death, fire, life, water, none;

		private Sprite getSchoolSprite(School s) {
			switch (s) {
			case School.Death:
				return death;
			case School.Fire:
				return fire;
			case School.Life:
				return life;
			case School.Water:
				return water;
			}

			return none;
		}

		public static Spell GetSpell() {
			return null;
		}

		public void Infuse(GameObject moteSlot) {
			var slot = moteSlot.GetComponent<UICrystalSlot>();
			if (slot.Content != null) {
				infuse (slot.Content);
				slot.RemoveContent ();
			} else {
				Debug.Log ("Must place a mote in the mote-slot");
			}
		}

		private void infuse(Item mote) {
			string schl = mote.Attributes [0].Name;
			int exp = mote.Attributes [0].Value;

			var slot = GetComponent<UICrystalSlot> ();
			Item crystal = slot.Content;
			if (crystal != null) {
				crystal.AddValueToTag (schl, exp);
				crystal.icon = getSchoolSprite (getHighestSchool (crystal));
				slot.RemoveContent ();
				slot.Place (crystal);
			} else {
				Debug.Log ("Must place crystal in the crystal-slot");
			}
		}

		private School getHighestSchool(Item crystal) {
			AttributeTag[] attrs = crystal.Attributes;
			int tier = 0;
			string highest = "none";

			for (int i = 0; i < attrs.Length; i++) {
				if (attrs [i].Value > tier) {
					highest = attrs [i].Name;
					tier = attrs [i].Value;
				}
			}

			return ExtensionMethods.Extensions.StringToSchool(highest);
		}
	}
}

