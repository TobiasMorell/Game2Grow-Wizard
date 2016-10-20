using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class ErrorLogUI : MonoBehaviour
	{
		public static ErrorLogUI Instance;
		[SerializeField]
		Text errorText;

		void Start()
		{
			Instance = this;
			errorText.enabled = false;
		}

		public void LogError(string error)
		{
			errorText.text = error;
			errorText.enabled = true;
			StartCoroutine(hideError());
		}

		IEnumerator hideError()
		{
			yield return new WaitForSeconds(2);
			errorText.enabled = false;
		}
	}
}
