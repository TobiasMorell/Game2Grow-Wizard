using System;
using ItemClasses;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts.UI
{
	
	public abstract class DragableSlot : UITooltipSlot<Item>, IDragHandler, IEndDragHandler
	{
		protected InventoryUI UI;

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
			if (UI.DraggingItem) {
				UI.hovering = this;
				return;
			}
		}
		public override void OnPointerExit (PointerEventData ped) {
			base.OnPointerExit(ped);
			if (UI.DraggingItem)
				UI.hovering = null;
		}
		public virtual void OnDrag(PointerEventData ped) {
			if (!UI.DraggingItem) {
				UI.StartDrag (this);
			}
		}
		public virtual void OnEndDrag(PointerEventData ped) {
			UI.EndDrag ();
		}
		#endregion

		public override void Place (Item content)
		{
			Content = content;
			if (content != null)
			{
				createTooltip(content);
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

