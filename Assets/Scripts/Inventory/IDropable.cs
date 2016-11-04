using System;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
	public interface IDropable
	{
		void DropStack (ItemClasses.Item i, int stacksize);
		void Drop (ItemClasses.Item i);
		void PickUp(Inventory collector);
	}
}

