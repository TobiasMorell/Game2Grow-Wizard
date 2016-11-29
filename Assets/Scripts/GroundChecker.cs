using UnityEngine;


public class GroundChecker : MonoBehaviour
{
	Wizard wizard;

	void Awake() {
		wizard = GetComponentInParent<Wizard>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Platform")) {
			wizard.HitGround ();
		}
	}
}

