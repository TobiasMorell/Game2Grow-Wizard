using UnityEngine;
using System.Collections;

namespace Assets.Scripts.UI
{
public class TooltipText : AbstractTooltip {
	[SerializeField] string description;

	public override void Start() {
		base.Start();
		createTooltip(description);
	}


	protected override void createTooltip(System.Object content) {
		this.Tooltip = content.ToString();
	}
	
}
}
