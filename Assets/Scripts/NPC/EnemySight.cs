using UnityEngine;
using System.Collections;
using Assets.Scripts;

namespace Assets.Scripts.NPC {
	public class EnemySight : MonoBehaviour {
		private Enemy enemy;

		void Start()
		{
			this.enemy = GetComponentInParent<Enemy> ();
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Player")
				enemy.Targets.Add(other.gameObject);
		}

		void OnTriggerExit2D(Collider2D other)
		{
			if (other.tag == "Player")
				enemy.Targets.Remove(other.gameObject);
		}
	}
}
