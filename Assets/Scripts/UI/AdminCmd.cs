using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class AdminCmd : MonoBehaviour
	{
		InputField cmdInput;
		Toggler toggler;

		delegate void AdminFunc(int id);
		Dictionary<string, AdminFunc> adminCommands;

		void Awake() {
			cmdInput = GetComponent<InputField> ();
			toggler = GetComponent<Toggler> ();

			adminCommands = new Dictionary<string, AdminFunc> ();
			adminCommands.Add (":ADDITEM", addItem);
		}

		void Update() {
			if (Input.GetKeyDown (KeyCode.Tab))
				GameRegistry.Typing = !GameRegistry.Typing;
		}

		public void ParseText() {
			string input = cmdInput.text;
			string[] split = input.Split ('(');

			//Trim all excess characters
			split [1] = split[1].TrimEnd (')');

			if (adminCommands.ContainsKey (split [0])) {
				adminCommands [split [0]].Invoke (int.Parse (split [1]));
			} else {
				ErrorLogUI.Instance.LogError ("Input not recognized. All input must be of format :ACTION(INT)");
			}
			toggler.Toggle ();
		}

		void addItem(int id) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Assets.Scripts.Inventory.Inventory> ().AddItem (id);
		}
	}
}

