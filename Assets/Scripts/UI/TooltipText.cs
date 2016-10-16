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
	}
}
