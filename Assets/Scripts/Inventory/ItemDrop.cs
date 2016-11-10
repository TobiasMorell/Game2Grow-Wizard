using System;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
	public class ItemDrop : MonoBehaviour, IDropable
	{
		public float rotationSpeed;
		public float lifetime;
		private ItemClasses.Item heldItem;
		private int stacksize;

		public void Drop(ItemClasses.Item i) {
			DropStack (i, 1);
		}

		public void DropStack(ItemClasses.Item i, int stacksize) {
			this.heldItem = i;
			this.stacksize = stacksize;
			GetComponent<SpriteRenderer> ().sprite = i.icon;
			Destroy (this.gameObject, lifetime);
		}

		public void PickUp(Inventory collector) {
			collector.AddItem (heldItem, stacksize);
			GetComponent<SpriteRenderer> ().enabled = false;
		}

		void Update() {
			transform.Rotate (0, rotationSpeed, 0);
		}

		void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag ("Player") && !other.isTrigger) {
				PickUp (other.GetComponent<Inventory>());
			}
		}
	}
}

