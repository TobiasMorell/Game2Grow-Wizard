using System;
using UnityEngine;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ItemClasses;
using Assets.Scripts.UI;

public class InventorySlot : DragableSlot, IPointerDownHandler
{
	[SerializeField] Text quantityText;

	public override void Start() {
		base.Start();
		if (quantityText == null)
			Debug.LogAssertion ("Could not find text component!");
	}

	public override void Place(Item item)
	{
		base.Place (item);
		if (item != null)
		{
			//Display stacksize if it is larger than 1
			if (item.stackSize > 1)
			{
				quantityText.text = item.stackSize.ToString();
				quantityText.gameObject.SetActive(true);
			}
		}
	}
	public override void RemoveContent ()
	{
		base.RemoveContent ();
		quantityText.gameObject.SetActive (false);
	}

	public void UpdateItemQuantity() {
		if (Content.stackSize > 1) {
			quantityText.text = Content.stackSize.ToString();
			quantityText.gameObject.SetActive (true);
		} else {
			quantityText.gameObject.SetActive (false);
		}
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

	public void OnPointerDown(PointerEventData ped) {
		if (Content != null) {
			if (ped.button == PointerEventData.InputButton.Right) {
				UI.UsedItemFromSlot (this);
			}
		}
	}
}

