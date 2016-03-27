using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class EnemySight : MonoBehaviour {
	private Enemy enemy;

	void Start()
	{
		this.enemy = GetComponentInParent<Enemy> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {
			enemy.Target = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
			enemy.Target = null;
	}
}
