using UnityEngine;

namespace Phoenix
{
	public class PhoenixSight : MonoBehaviour
	{
		private PhoenixController phoenix;

		void Start()
		{
			this.phoenix = GetComponentInParent<PhoenixController> ();
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Hostile"))
				phoenix.Targets.Add(other.gameObject);
		}

		void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag("Hostile"))
				phoenix.Targets.Remove (other.gameObject);
		}
	}
}

