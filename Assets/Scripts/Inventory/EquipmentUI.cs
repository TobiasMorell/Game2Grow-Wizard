using System;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
	EquipmentSlot[] slots;
	Wizard wizard;

	void Start() {
		slots = GetComponentsInChildren<EquipmentSlot> ();
		wizard = GameObject.FindGameObjectWithTag ("Player").GetComponent<Wizard> ();

		//Only temporary - should be moved to some start-up script
		Equip(GameRegistry.ItemDatabase()["Walking Stick"]);
		Equip (GameRegistry.ItemDatabase () ["Novice Robes"]);
	}

	public void Equip(ItemClasses.Item item) {
		if (item == null)
			return;
		
		foreach (var slot in slots) {
			if (slot.slotType == item.Type) {
				slot.Place (item);
				wizard.Equip (item);
				break;
			}
		}
	}

	public void Unequip(EquipmentSlot fromSlot) {
		wizard.Unequip (fromSlot.Content.Type);
	}
}

