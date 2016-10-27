using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class BarHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		Text barText;
		Slider barSlider;

		void Awake() {
			barText = GetComponentInChildren<Text> ();
			barSlider = GetComponent<Slider> ();
			barText.enabled = false;
		}

		public void OnPointerEnter(PointerEventData ped) {
			barText.enabled = true;
			barText.text = barSlider.value.ToString () + '/' + barSlider.maxValue.ToString ();
		}

		public void OnPointerExit(PointerEventData ped) {
			barText.enabled = false;
		}
	}
}

