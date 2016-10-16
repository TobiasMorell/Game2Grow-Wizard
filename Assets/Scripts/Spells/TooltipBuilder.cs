using System;
using System.Text;

namespace Assets.Scripts.UI
{
	public static class TooltipBuilder
	{
		public static void CreateHeadline(StringBuilder tooltipText, String headline)
		{
			//Name of item
			tooltipText.Append("<size=18>");
			tooltipText.Append("<b>");
			AppendColorOpen(tooltipText, "ffffff");
			tooltipText.Append(headline);
			AppendColorClosure(tooltipText);
			tooltipText.Append("</b>");
			tooltipText.Append("</size>");
		}
		public static void CreateDescription(StringBuilder tooltipText, string description)
		{
			AppendColorOpen(tooltipText, "E8E8E8");
			tooltipText.Append(description);
			AppendColorClosure(tooltipText);
		}

		public static void AppendColorOpen(StringBuilder tooltipText, string hexColor)
		{
			tooltipText.Append("<color=#");
			tooltipText.Append(hexColor);
			tooltipText.Append(">");

		}
		public static void AppendColorClosure(StringBuilder tooltipText)
		{
			tooltipText.Append("</color>");
		}
	}
}

