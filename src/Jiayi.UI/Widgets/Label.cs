using System.Drawing;
using Jiayi.UI.Render;

namespace Jiayi.UI.Widgets;

public class Label : Widget
{
	public string Text { get; set; } = string.Empty;
	public Color TextColor { get; set; } = Color.Black;
	public string FontName { get; set; } = "Arial";
	public float FontSize { get; set; } = 12;
	public int FontWeight { get; set; } = 400;
	public bool Italic { get; set; } = false;
	public bool AutoSize { get; set; } = true;

	public override void Render(Graphics g)
	{
		if (AutoSize)
		{
			var size = g.MeasureText(Text, FontName, FontSize, FontWeight, Italic);
			Size = size;
		}
		
		g.DrawText(Text, FontName, GetAbsolutePosition(), TextColor, FontSize, FontWeight, Italic, Size);
		
		// debug
		g.DrawRect(GetAbsolutePosition(), Size, Color.Red, 5);
	}
}