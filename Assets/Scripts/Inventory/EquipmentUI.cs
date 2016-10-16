using System;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
	EquipmentSlot[] slots;
	Wizard wizard;

	void Start() {
		slots = GetComponentsInChildren<EquipmentSlot> ();
		wizard = GameObject.FindGameObjectWithTag ("Player").GetComponent<Wizard> ();
	}

	public void EquipRightClick(ItemClasses.Item item) {
		if (item == null)
			return;
		
		foreach (var slot in slots) {
			if (slot.slotType == item.Type) {
				slot.Place (item);
				break;
			}
		}
	}

	public void EquipFromSlot(EquipmentSlot fromSlot) {
		Debug.Log ("Should equip: " + fromSlot.Content.ItemName);
		wizard.Equip (fromSlot.Content);
	}

	public void Unequip(EquipmentSlot fromSlot) {
		wizard.Unequip (fromSlot.Content.Type);
	}
}

