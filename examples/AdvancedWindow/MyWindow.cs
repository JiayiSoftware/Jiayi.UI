using System.Drawing;
using System.Numerics;
using Jiayi.UI;
using Jiayi.UI.Widgets;

namespace AdvancedWindow;

public class MyWindow() : Window("Advanced Window", new Vector2(800, 600))
{
	public override void Initialize()
	{
		BackgroundColor = Color.CornflowerBlue;

		Children =
		[
			new Label
			{
				Text = "The quick brown fox jumps over the lazy dog",
				Position = new Vector2(0, 10),
				Anchor = Anchor.TopRight,
				FontName = "Segoe UI",
				FontSize = 24,
				TextColor = Color.White
			}
		];
	}
}