using System;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
	public abstract class Displayable : MonoBehaviour
	{
		public int Id;

		public void RegisterToItemID(int id) {
			Id = id;
		}

		public abstract void Equip(int supportValue);
	}
}

