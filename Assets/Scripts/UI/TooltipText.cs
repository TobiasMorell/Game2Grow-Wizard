using UnityEngine;
using System.Collections;

namespace Assets.Scripts.UI
{
	public class TooltipText : AbstractTooltip {
		[SerializeField] string description;

		public override void Start() {
			base.Start();
			Tooltip = description;
		}

		public void SetDescription(string newDesc) {
			description = newDesc;
		}

		public override void OnPointerEnter (UnityEngine.EventSystems.PointerEventData ped)
		{
			if (description != string.Empty) {
				showTooltip = true;
			}
		}
	}
}
