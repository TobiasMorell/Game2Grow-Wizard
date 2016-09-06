using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public abstract class UITooltipSlot<T> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]protected Image iconImage;
		[SerializeField]protected GUISkin skin;
		private Rect screenRect;

		public virtual void Start ()
		{
			screenRect = new Rect(0, 0, Screen.width, Screen.height);
		}

		protected bool showTooltip;
		public T Content
		{
			protected set;
			get;
		}

		public string Tooltip
		{
			get;
			protected set;
		}

		public void OnGUI()
		{
			if (showTooltip)
			{
				GUIStyle ttStyle = skin.GetStyle("Tooltip");
				float dynamicHeight = ttStyle.CalcHeight(new GUIContent(Tooltip), 200);

				Rect ttBox = new Rect(Event.current.mousePosition.x + 10f, Event.current.mousePosition.y, 200, dynamicHeight);
				
				//Find out if the box should be moved down or left
				if(ttBox.x < screenRect.x)
					ttBox.x = screenRect.x;
				if(ttBox.y < screenRect.y)
					ttBox.y = screenRect.y;

				//Find out if the box should be moved up or right
				float exceedingHeight = (ttBox.y + ttBox.height) - (screenRect.y + screenRect.height);
				if (exceedingHeight > 0)
					ttBox.y = ttBox.y - exceedingHeight;
				float exceedingWidth = (ttBox.x + ttBox.width) - (screenRect.x + screenRect.width);
				if (exceedingWidth > 0)
					ttBox.x = ttBox.x - exceedingWidth;

				GUI.Box(ttBox, Tooltip, ttStyle);
			}
		}

		public virtual void OnPointerEnter(PointerEventData ped)
		{
			if (Content != null)
			{
				showTooltip = true;
			}
		}
		public virtual void OnPointerExit(PointerEventData ped)
		{
			showTooltip = false;
		}

		protected abstract void createTooltip(T content);
		public abstract void Place(T content);
		public virtual void RemoveContent() {
			Content = default(T);
			iconImage.sprite = null;
			iconImage.gameObject.SetActive(false);
		}

		protected virtual void createHeadline(StringBuilder tooltipText, string headline)
		{
			//Name of item
			tooltipText.Append("<size=18>");
			tooltipText.Append("<b>");
			appendColorOpen(tooltipText, "ffffff");
			tooltipText.Append(headline);
			appendColorClosure(tooltipText);
			tooltipText.Append("</b>");
			tooltipText.Append("</size>");
		}
		protected void createDescription(StringBuilder tooltipText, string desc)
		{
			appendColorOpen(tooltipText, "E8E8E8");
			tooltipText.Append(desc);
			appendColorClosure(tooltipText);
		}

		protected void appendColorOpen(StringBuilder tooltipText, string hexColor)
		{
			tooltipText.Append("<color=#");
			tooltipText.Append(hexColor);
			tooltipText.Append(">");

		}
		protected void appendColorClosure(StringBuilder tooltipText)
		{
			tooltipText.Append("</color>");
		}
	}
}
