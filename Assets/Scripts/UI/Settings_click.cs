using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Settings_click : MonoBehaviour {
	bool active = false;

	void Awake() {
		this.gameObject.SetActive (active);
	}

	// Use this for initialization
	public void Toggle() {
		this.gameObject.SetActive (!active);
		active = !active;
	}
}
