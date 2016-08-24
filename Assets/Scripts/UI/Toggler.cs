using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.UI {
	public class Toggler : MonoBehaviour {
		bool active = false;
		[SerializeField] private GameObject togglePanel;
		[SerializeField] KeyCode hotkey;

		void Awake() {
			togglePanel.SetActive (active);
		}


		void Start() {
			this.GetComponent<Button> ().onClick.AddListener (Toggle);
		}

		// Use this for initialization
		public void Toggle() {
			togglePanel.SetActive (!active);
			active = !active;
		}

		void Update() {
			if(Input.GetKeyDown(hotkey)) {
				Toggle();
				Debug.Log ("Should toggle: " + togglePanel.name);
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				togglePanel.SetActive (false);
				active = false;
			}
		}
	}
}
