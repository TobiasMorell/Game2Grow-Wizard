using System;
using ItemClasses;
using UnityEngine.EventSystems;
using UnityEngine;
using Assets.Scripts.Inventory;

namespace Assets.Scripts.UI
{
	
	public abstract class DragableSlot : UITooltipSlot<Item>, IDragHandler, IEndDragHandler
	{
		//A reference to the inventory UI
		protected InventoryUI UI;

		/// <summary>
		/// Determines if some slot accepts a given ItemType
		/// </summary>
		/// <returns><c>true</c>, If the ItemType was accepted, <c>false</c> otherwise.</returns>
		/// <param name="type">Type to check for.</param>
		public abstract bool DoesAccept (ItemClasses.ItemType type);

		public override void Start ()
		{
			base.Start ();
			UI = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryUI> ();
			if (UI == null)
				Debug.LogAssertion ("Reference from inventory-slot to UI missing!");
		}

		#region PointerEvents
		public override void OnPointerEnter(PointerEventData ped) {
			base.OnPointerEnter(ped);
			//Update the UI to reflect that this slot is being hovered.
			if (UI.DraggingItem) {
				UI.hovering = this;
			}
		}
		public override void OnPointerExit (PointerEventData ped) {
			base.OnPointerExit(ped);
			//Set hovering to null to reflect that no slot is being hovered.
			if (UI.DraggingItem)
				UI.hovering = null;
		}
		public virtual void OnDrag(PointerEventData ped) {
			//Calls for the UI to save information when starting a drag.
			if (!UI.DraggingItem) {
				UI.StartDrag (this);
			}
		}
		public virtual void OnEndDrag(PointerEventData ped) {
			UI.EndDrag ();
		}
		#endregion
		/// <summary>
		/// Place the specified content in the slot.
		/// </summary>
		/// <param name="content">Content to place (may be null).</param>
		public override void Place (Item content)
		{
			Content = content;
			if (content != null)
			{
				Tooltip = Content.Tooltip ();
				//Change icon of item slot and activate the image-component
				iconImage.sprite = content.icon;
				iconImage.gameObject.SetActive(true);
			}
			else {
				RemoveContent();
			}
		}
	}
}

