using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public abstract class UITooltipSlot<T> : AbstractTooltip
	{
		/// <summary>
		/// A reference to the Image UI-element.
		/// </summary>
		[SerializeField]protected Image iconImage;

		/// <summary>
		/// Gets or sets the content of the slot.
		/// </summary>
		/// <value>The content.</value>
		public T Content
		{
			protected set;
			get;
		}

		public override void Start() {
			base.Start();
		}

		/// <summary>
		/// Handles the pointer enter event.
		/// </summary>
		/// <param name="ped">Not actually used as of now.</param>
		public override void OnPointerEnter(PointerEventData ped)
		{
			//If the content is present, show its tooltip.
			if (Content != null)
				showTooltip = true;
		}

		/// <summary>
		/// Place the specified content in the slot.
		/// </summary>
		/// <param name="content">Content to place (may be null).</param>
		public abstract void Place(T content);

		/// <summary>
		/// Removes the content from the slot.
		/// </summary>
		public virtual void RemoveContent() {
			//T may be a struct, so default it.
			Content = default(T);
			//Delete sprite and hide the Image-element to avoid white box.
			iconImage.sprite = null;
			iconImage.gameObject.SetActive(false);
		}
	}
}
