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
		[SerializeField]protected Image iconImage;
		
		public T Content
		{
			protected set;
			get;
		}

		public override void Start() {
			base.Start();
		}

		public virtual void OnPointerEnter(PointerEventData ped)
		{
			if (Content != null)
				showTooltip = true;
		}

		public abstract void Place(T content);
		public virtual void RemoveContent() {
			Content = default(T);
			iconImage.sprite = null;
			iconImage.gameObject.SetActive(false);
		}
	}
}
