using UnityEngine;
using System.Collections;
using Spells;

namespace Assets.Scripts.UI {
	public class UICrystalSpellSlot : UICrystalSlot {
		//A reference to the players SpellCaster script
		private SpellCaster holder;
		/// <summary>
		/// The position in the array of four Crystal slots.
		/// </summary>
		[SerializeField] int position;

		public override void Place (ItemClasses.Item content)
		{
			base.Place (content);
			if (content != null) {
				var highest = findHighestSchools (content);
				Spell newSpell = GameRegistry.SpellDatabase ().GetSpellFromCrystal (highest);

				Debug.Log ("Should assign: " + newSpell.Name);

				holder.UpdateSpells (newSpell, position);
			}
		}

		private ItemClasses.AttributeTag[] findHighestSchools(ItemClasses.Item content) {
			ItemClasses.AttributeTag[] schools = content.GetAllTags ();

			int highest = 0, second_highest = 0, h_index = -1, sh_index = -1;

			for (int i = 0; i < schools.Length; i++) {
				if (schools [i].Value > highest) {
					//Put the old highest as second highest
					second_highest = highest;
					sh_index = h_index;

					//Update the values for the new highest
					highest = schools [i].Value;
					h_index = i;

					Debug.Log ("Found an attribute with higher values: " + schools [i].Name);

					continue;
				}

				//Update the second highest value in-case the value is higher
				if (schools [i].Value > second_highest) {
					second_highest = schools [i].Value;
					sh_index = i;
				}
			}

			var ret = new ItemClasses.AttributeTag[2];

			//Set both schools to null, if all schools are 0
			if (h_index == -1) {
				ret [0] = null;
				ret [1] = null;

				return ret;
			} 

			//Set secondary school to null, if only one school is higher than 0
			if (h_index != -1 && sh_index == -1) {
				ret [0] = schools [h_index];
				ret [1] = null;

				return ret;
			} 
			//There are points in two schools, add them to the array
			ret [0] = schools [h_index];
			ret [1] = schools [sh_index];

			Debug.Log ("Highest school: " + ret[0]);
			Debug.Log ("Other school: " + ret [1]);

			return ret;
		}

		public override void Start ()
		{
			base.Start ();
			this.holder = GameObject.FindGameObjectWithTag ("Player").GetComponent<SpellCaster> ();
		}

		public override void OnDrag (UnityEngine.EventSystems.PointerEventData ped)
		{
			base.OnDrag (ped);
			holder.UpdateSpells (null, position);
		}

		public override void OnEndDrag (UnityEngine.EventSystems.PointerEventData ped)
		{
			base.OnEndDrag (ped);
			holder.UpdateSpells (null, position);
		}
	}
}
