using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.UI {
	public class Toggler : MonoBehaviour {
		bool active = false;
		[SerializeField] private CanvasGroup togglePanel;
		[SerializeField] KeyCode hotkey;

		void Start() {
			HideCanvas ();
			this.GetComponent<Button> ().onClick.AddListener (Toggle);
		}

		// Use this for initialization
		public void Toggle() {
			if (active) {
				HideCanvas ();
				active = false;
			} else {
				ShowCanvas ();
				active = true;
			}
		}

		void ShowCanvas() {
			togglePanel.alpha = 1f;
			togglePanel.blocksRaycasts = true;
		}
		void HideCanvas() {
			togglePanel.alpha = 0f;
			togglePanel.blocksRaycasts = false;
		}

		void Update() {
			if(Input.GetKeyDown(hotkey)) {
				Toggle();
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				HideCanvas ();
				active = false;
			}
		}
	}
}
