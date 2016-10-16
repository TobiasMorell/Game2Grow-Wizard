using System;
using UnityEngine;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ItemClasses;
using Assets.Scripts.UI;

public class InventorySlot : DragableSlot, IPointerDownHandler
{
	/// <summary>
	/// Used to display the number of stackable items.
	/// </summary>
	[SerializeField] Text quantityText;

	/// <summary>
	/// Determines if some slot accepts a given ItemType
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="type">Type to check for.</param>
	public override bool DoesAccept (ItemType type)
	{
		//InventorySlots accept all ItemTypes.
		return true;
	}

	public override void Start() {
		base.Start();
		if (quantityText == null)
			Debug.LogAssertion ("Could not find text component!");
	}

	/// <summary>
	/// Place the specified content in the slot.
	/// </summary>
	/// <param name="content">Content to place (may be null).</param>
	/// <param name="item">Item.</param>
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

	/// <summary>
	/// Removes the content from the slot.
	/// </summary>
	public override void RemoveContent ()
	{
		base.RemoveContent ();
		//Disable the text that displays stacksize.
		quantityText.gameObject.SetActive (false);
	}

	/// <summary>
	/// Updates the item quantity.
	/// </summary>
	public void UpdateItemQuantity() {
		//Update and display stacksize if it is larger than 1
		if (Content.stackSize > 1) {
			quantityText.text = Content.stackSize.ToString();
			quantityText.gameObject.SetActive (true);
		} 
		//Else just hide it (stacksize 1 is implicit)
		else {
			quantityText.gameObject.SetActive (false);
		}
	}

	/// <summary>
	/// Determines if this slot holds an item with the given id.
	/// </summary>
	/// <param name="id">Identifier to search for.</param>
	public bool Holds(int id) {
		if (Content != null && Content.Id == id)
			return true;

		return false;
	}

	public void OnPointerDown(PointerEventData ped) {
		if (Content != null) {
			//Tells the UI that the user wants to use an item, when he right-clicks.
			if (ped.button == PointerEventData.InputButton.Right) {
				UI.UsedItemFromSlot (this);
			}
		}
	}
}

