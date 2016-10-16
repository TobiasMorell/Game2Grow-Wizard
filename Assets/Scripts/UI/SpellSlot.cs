using UnityEngine;
using System.Collections;
using Spells;
using System;
using System.Text;
using UnityEngine.UI;

namespace Assets.Scripts.UI {
	public class SpellSlot : UITooltipSlot<Spell> {
		//A reference to the slider component used to display cooldown.
		Slider cooldownSlider;

		/// <summary>
		/// Place the specified spell in the slot.
		/// </summary>
		/// <param name="content">Spell to place (may be null).</param>
		public override void Place(Spell content)
		{
			Content = content;
			if (content != null) {
				Tooltip = content.Tooltip ();
				//Change icon of item slot and activate the image-component
				iconImage.sprite = content.Icon;
				iconImage.gameObject.SetActive (true);
				cooldownSlider.maxValue = content.Cooldown;
			} else {
				RemoveContent ();
			}
		}

		public override void Start()
		{
			base.Start();
			cooldownSlider = GetComponentInParent<Slider> ();
		}

		void Update() {
			if(Content != null)
				cooldownSlider.value = Content.cooldownTimer;
		}
	}
}
