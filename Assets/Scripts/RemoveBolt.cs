using UnityEngine;
using System.Collections;

public class RemoveBolt : MonoBehaviour {

	void OnTriggerExit(Collider other)
	{
		if (other.tag.Equals("Projectile"))
			Destroy (other.gameObject);
	}
}
