using UnityEngine;
using System.Collections;
using Assets.Scripts.Spells;
using Assets.Scripts.UI;
using System;
using System.Text;
using UnityEngine.UI;

public class SpellSlot : UITooltipSlot<Spell> {
	private Spell spell;

	public override void Place(Spell content)
	{
		Content = content;
		if (content != null)
		{
			createTooltip(content);
			//Change icon of item slot and activate the image-component
			iconImage.sprite = content.Icon;
			iconImage.gameObject.SetActive(true);
		}

		Debug.Log("Placed " + content.Name + " in a spellslot.");
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
	}
}
