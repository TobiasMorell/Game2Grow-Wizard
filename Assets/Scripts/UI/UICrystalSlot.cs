using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spells;

namespace Assets.Scripts.UI {
	public class UICrystalSlot : DragableSlot {
				/// <summary>
		/// Determines if some slot accepts a given ItemType
		/// </summary>
		/// <returns><c>true</c> if the item is a Magic item, <c>false</c> otherwise.
		/// </returns>
		/// <param name="type">Type to check for.</param>
		public override bool DoesAccept (ItemClasses.ItemType type)
		{
			return type == ItemClasses.ItemType.Magic;
		}

		public override void Start ()
		{
			base.Start ();
		}

		public override void OnDrag (UnityEngine.EventSystems.PointerEventData ped)
		{
			base.OnDrag (ped);
		}

		public override void OnEndDrag (UnityEngine.EventSystems.PointerEventData ped)
		{
			base.OnEndDrag (ped);
		}
	}
}
