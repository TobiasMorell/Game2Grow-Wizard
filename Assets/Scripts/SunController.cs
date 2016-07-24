using UnityEngine;
using System.Collections;

public class SunController : MonoBehaviour {
	[SerializeField] float minsForADay;
	float rotPrSec;

	// Use this for initialization
	void Start () {
		this.transform.rotation.Set(45f, -50f, 0, 0);
		rotPrSec = 360f / (minsForADay * 60);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (0, rotPrSec * Time.deltaTime, 0);
	}
}
