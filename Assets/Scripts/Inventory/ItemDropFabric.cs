using System;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
	public class ItemDropFabric : MonoBehaviour
	{
		public GameObject itemdropPrefab;

		void Awake() {
			GameRegistry.AssignItemDropFabric (this);
		}

		public void Drop(ItemClasses.Item item, int stacksize, Vector2 position) {
			var inst = Instantiate (itemdropPrefab);
			inst.GetComponent<IDropable>().DropStack (item, stacksize);
			inst.transform.position = position;
		}

		public void Drop(ItemClasses.Item item, Vector2 position) {
			var inst = Instantiate (itemdropPrefab);
			inst.GetComponent<IDropable>().Drop (item);
			inst.transform.position = position;
		}
	}
}

