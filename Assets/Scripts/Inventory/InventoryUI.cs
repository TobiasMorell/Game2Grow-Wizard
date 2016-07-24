using System;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	InventorySlot[] slots;
	bool showGUI;

	void Start() {
		//Assign inventory slots
		slots = this.GetComponentsInChildren<InventorySlot> ();

		gameObject.SetActive(false);
		showGUI = false;
	}

	void Update() {
		
	}
	public void UpdateItems(Item[] items) {
		if (items.Length != slots.Length)
			Debug.LogAssertion ("UI and inventory are not of the same size!");
		for (int i = 0; i < items.Length; i++) {
			slots [i].PlaceItem(items [i]);
		}
	}

	public void ToggleGUI() {
		showGUI = !showGUI;
		gameObject.SetActive(showGUI);
	}


}

