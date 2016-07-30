using System;
using UnityEngine;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IEndDragHandler
{
	public Item Item {
		get;
		private set;
	}
	[SerializeField] Image iconImage;
	[SerializeField] GUISkin skin;

	private bool showTooltip;

	private InventoryUI UI;

	public string Tooltip {
		get;
		private set;
	}

	void Start() {
		UI = GetComponentInParent<InventoryUI> ();
		if (UI == null) {
			Debug.LogAssertion ("Reference from inventory-slot to UI missing!");
		}
	}

	public void OnGUI() {
		if (showTooltip) {
			GUIStyle ttStyle = skin.GetStyle ("Tooltip");
			float dynamicHeight = ttStyle.CalcHeight (new GUIContent (Tooltip), 200);

			GUI.Box (new Rect (Event.current.mousePosition.x + 10f, Event.current.mousePosition.y, 200, dynamicHeight),
				Tooltip, ttStyle);
		}
	}

	public void OnPointerEnter(PointerEventData ped) {
		if (UI.DraggingItem) {
			UI.hovering = this;
			return;
		}
		if (Item != null) {
			showTooltip = true;
		}
	}
	public void OnPointerExit (PointerEventData ped) {
		if (UI.DraggingItem)
			UI.hovering = null;
		showTooltip = false;
	}
	public void OnPointerDown(PointerEventData ped) {
		if (ped.button == PointerEventData.InputButton.Right) {
			Debug.Log ("Should use item " + Item.ItemName);
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

	public void PlaceItem(Item item) {
		Item = item;
		if (item != null) {
			createTooltip (item);
			iconImage.sprite = item.icon;
			iconImage.gameObject.SetActive(true);
		}
	}
	public void RemoveItem() {
		Item = null;
		iconImage.sprite = null;
		iconImage.gameObject.SetActive(false);
	}
	public bool Holds(int id) {
		if (Item != null && Item.Id == id)
			return true;

		return false;
	}

	void createTooltip(Item item) {
		StringBuilder tooltipText = new StringBuilder ();

		//Name of item
		tooltipText.Append ("<size=18>");
		tooltipText.Append ("<b>");
		appendColorOpen (tooltipText, "ffffff");
		tooltipText.Append (item.ItemName);
		appendColorClosure (tooltipText);
		tooltipText.Append ("</b>");
		tooltipText.Append ("</size>");
		tooltipText.Append ("\n\n");

		//Description
		appendColorOpen (tooltipText, "E8E8E8");
		tooltipText.Append (item.Description);
		appendColorClosure (tooltipText);
		tooltipText.Append ("\n\n");

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
	void appendColorOpen(StringBuilder tooltipText, string hexColor) {
		tooltipText.Append ("<color=#");
		tooltipText.Append (hexColor);
		tooltipText.Append (">");

	}
	void appendColorClosure(StringBuilder tooltipText) {
		tooltipText.Append ("</color>");
	}
}

