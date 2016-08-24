using System;
using Assets.Scripts.UI;
using ItemClasses;
using UnityEngine;

public class EquipmentSlot : DragableSlot
{
	[SerializeField] private ItemType slotType;

	public override void Start ()
	{
		base.Start ();
	}

	public override void Place (Item content)
	{
		throw new NotImplementedException ();
	}

	protected override void createTooltip (Item content)
	{
		throw new NotImplementedException ();
	}
}
