using System;
using Assets.Scripts.UI;
using ItemClasses;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : DragableSlot
{
	/// <summary>
	/// The type of the slot (may be Head, Ring i.e.)
	/// </summary>
	public ItemType slotType;
	/// <summary>
	/// The background for the slot, when no equipment is present.
	/// </summary>
	[SerializeField] Sprite unequippedBackground;
	/// <summary>
	/// The background for the slot, when equipment is present.
	/// </summary>
	[SerializeField] Sprite equippedBackground;
	/// <summary>
	/// A reference to the equipment UI.
	/// </summary>
	private EquipmentUI eUI;

	/// <summary>
	/// Determines if some slot accepts a given ItemType
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="type">Type to check for.</param>
	public override bool DoesAccept (ItemType type)
	{
		//Given type must be the same as the slot to place it here.
		return type == slotType;
	}

	public override void Start ()
	{
		base.Start ();
		this.GetComponent<Image> ().sprite = unequippedBackground;
		eUI = GetComponentInParent<EquipmentUI> ();
	}

	/// <summary>
	/// Place the specified content in the slot.
	/// </summary>
	/// <param name="content">Content to place (may be null).</param>
	public override void Place (Item content)
	{
		base.Place (content);
		if (content != null) {
			this.GetComponent<Image> ().sprite = equippedBackground;
			//Equip the new piece of equipment
			eUI.EquipFromSlot (this);
		}
	}

	public override void OnDrag (UnityEngine.EventSystems.PointerEventData ped)
	{
		//Unequip what was in the slot before
		if(Content != null)
			eUI.Unequip (this);
		//And call base to handle UI stuff
		base.OnDrag (ped);
	}

	/// <summary>
	/// Removes the content from the slot.
	/// </summary>
	public override void RemoveContent ()
	{
		base.RemoveContent ();
		this.GetComponent<Image> ().sprite = unequippedBackground;
	}
}
