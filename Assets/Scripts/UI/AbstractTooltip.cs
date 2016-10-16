using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;

namespace Assets.Scripts.UI
{
    public abstract class AbstractTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        private Rect screenRect;
        [SerializeField]protected GUISkin skin;

        public string Tooltip
		{
			get;
			protected set;
		}

		public virtual void Start ()
		{
			screenRect = new Rect(0, 0, Screen.width, Screen.height);
		}

		protected bool showTooltip;

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
			showTooltip = true;
		}
		public virtual void OnPointerExit(PointerEventData ped)
		{
			showTooltip = false;
		}
    }
}