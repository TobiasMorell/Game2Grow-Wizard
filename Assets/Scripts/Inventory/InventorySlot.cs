using System;
using UnityEngine;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ItemClasses;
using Assets.Scripts.UI;

public class InventorySlot : UITooltipSlot<Item>, IPointerDownHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] Text quantityText;
	private InventoryUI UI;

	public override void Start() {
		base.Start();
		if (quantityText == null)
			Debug.LogAssertion ("Could not find text component!");
		UI = GetComponentInParent<InventoryUI> ();
		if (UI == null)
			Debug.LogAssertion ("Reference from inventory-slot to UI missing!");
	}

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
	public void OnPointerDown(PointerEventData ped) {
		if (Content != null) {
			if (ped.button == PointerEventData.InputButton.Right) {
				UI.UsedItemFromSlot (this);
			}
		}
	}
	public void OnDrag(PointerEventData ped) {
		if (!UI.DraggingItem) {
			UI.StartDrag (this);
		}
	}
	public void OnEndDrag(PointerEventData ped) {
		UI.EndDrag ();
	}

	public override void Place(Item item)
	{
		Content = item;
		if (item != null)
		{
			createTooltip(item);
			//Change icon of item slot and activate the image-component
			iconImage.sprite = item.icon;
			iconImage.gameObject.SetActive(true);
			//Display stacksize if it is larger than 1
			if (item.stackSize > 1)
			{
				quantityText.text = item.stackSize.ToString();
				quantityText.gameObject.SetActive(true);
			}
		}
		else {
			RemoveItem();
		}
	}

	public void UpdateItemQuantity() {
		if (Content.stackSize > 1) {
			quantityText.text = Content.stackSize.ToString();
			quantityText.gameObject.SetActive (true);
		} else {
			quantityText.gameObject.SetActive (false);
		}
	}

	public void RemoveItem() {
		Content = null;
		iconImage.sprite = null;
		iconImage.gameObject.SetActive(false);
	}
	public bool Holds(int id) {
		if (Content != null && Content.Id == id)
			return true;

		return false;
	}

	protected override void createTooltip(Item item) {
		StringBuilder tooltipText = new StringBuilder ();
		createHeadline(tooltipText, item.ItemName);
		
		tooltipText.Append ("\n\n");
		
		createDescription(tooltipText, item.Description);
		tooltipText.Append("\n\n");

		//Value
		appendColorOpen (tooltipText, "FFD700");
		tooltipText.Append (item.Value);
		tooltipText.Append (" golds");
		appendColorClosure (tooltipText);
		tooltipText.Append ("\n\n");

		//Type
		appendColorOpen (tooltipText, "BAEEFF");
		tooltipText.Append (item.Type);
		appendColorClosure (tooltipText);

		Tooltip = tooltipText.ToString ();
	}
	
}

