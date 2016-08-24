using UnityEngine;
using System.Collections;
using Spells;
using System;
using System.Text;
using UnityEngine.UI;

namespace Assets.Scripts.UI {
	public class SpellSlot : UITooltipSlot<Spell> {
		Slider cooldownSlider;

		public override void Place(Spell content)
		{
			Content = content;
			if (content != null)
			{
				createTooltip(content);
				//Change icon of item slot and activate the image-component
				iconImage.sprite = content.Icon;
				iconImage.gameObject.SetActive(true);
				cooldownSlider.maxValue = content.Cooldown;
			}
		}

		protected override void createTooltip(Spell content)
		{
			StringBuilder tooltipSB = new StringBuilder();
			createHeadline(tooltipSB, content.Name);

			tooltipSB.Append("\n");

			//Show mana cost
			appendColorOpen(tooltipSB, "0021FF");
			tooltipSB.Append(content.Cost);
			tooltipSB.Append(" mana");
			appendColorClosure(tooltipSB);
			tooltipSB.Append("\n");
			//Show cooldown

			createDescription(tooltipSB, content.Cooldown + "\n\n" + content.Description);

			Tooltip = tooltipSB.ToString();
		}

		public override void Start()
		{
			base.Start();
			cooldownSlider = GetComponentInParent<Slider> ();
		}

		void Update() {
			cooldownSlider.value = Content.cooldownTimer;
		}
	}
}
