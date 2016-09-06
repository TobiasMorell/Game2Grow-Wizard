using System;
using Assets.Scripts.UI;
using ItemClasses;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : DragableSlot
{
	public ItemType slotType;
	[SerializeField] Sprite unequippedBackground;
	[SerializeField] Sprite equippedBackground;
	private EquipmentUI eUI;

	public override void Start ()
	{
		base.Start ();
		this.GetComponent<Image> ().sprite = unequippedBackground;
		eUI = GetComponentInParent<EquipmentUI> ();
	}

	public override void Place (Item content)
	{
		base.Place (content);
		if(content != null)
			this.GetComponent<Image> ().sprite = equippedBackground;
	}


	public override void OnEndDrag (UnityEngine.EventSystems.PointerEventData ped)
	{
		//Update content of the slot and display on UI
		base.OnEndDrag (ped);
		//Equip the new piece of equipment
		eUI.Equip (Content);
	}
	public override void OnDrag (UnityEngine.EventSystems.PointerEventData ped)
	{
		//Unequip what was in the slot before
		if(Content != null)
			eUI.Unequip (this);
		//And call base to handle UI stuff
		base.OnDrag (ped);
	}

	public override void RemoveContent ()
	{
		base.RemoveContent ();
		this.GetComponent<Image> ().sprite = unequippedBackground;
	}

	protected override void createTooltip (Item item)
	{
		var tooltipText = new System.Text.StringBuilder ();
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
