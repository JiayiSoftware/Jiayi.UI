using System.Drawing;
using System.Numerics;
using Jiayi.UI;
using Jiayi.UI.Widgets;

namespace MarkupApp;

// imagine all of this came from a source generator
// xml attributes translate to constructor parameters in C#
public partial class MainWindow() : Window("Markup App", new Vector2(800, 600))
{
	// css properties are set in Initialize
	public override void Initialize()
	{
		BackgroundColor = GetCssProperties().GetColor("background-color", Color.White);
		MinimumSize = GetCssProperties().GetVector2("min-size", new Vector2(300, 300));
		MaximumSize = GetCssProperties().GetVector2("max-size", new Vector2(10000, 10000));
		
		// child elements are added in Initialize; no content can be added inside an xml element
		Children =
		[
			new StackPanel(Orientation.Vertical)
			{
				Children =
				[
					new Label("Hello, world!")
					{
						FontSize = GetCssProperties().GetAbsolute("font-size", 24),
						FontWeight = GetCssProperties().GetNumber("font-weight", 400),
						FontFamily = GetCssProperties().GetString("font-family", "Segoe UI"),
						TextColor = GetCssProperties().GetColor("color", Color.Black)
					},
					new Button("Click me!", OnButtonClick)
					{
						FontSize = GetCssProperties().GetAbsolute("font-size", 16),
						FontWeight = GetCssProperties().GetNumber("font-weight", 400),
						BackgroundColor = GetCssProperties().GetColor("background-color", Color.White),
						BorderColor = GetCssProperties().GetColor("border-color", Color.Black),
						BorderWidth = GetCssProperties().GetAbsolute("border-width", 1),
						BorderRadius = GetCssProperties().GetAbsolute("border-radius", 0),
						Padding = GetCssProperties().GetVector4("padding", new Vector4(8, 4, 8, 4))
					}
				]
			}
		];
	}
}