using System.Drawing;
using System.Numerics;
using Jiayi.UI;
using Jiayi.UI.Widgets;
using SharpGen.Runtime;

namespace MarkupApp;

// imagine all of this came from a source generator
public partial class MainWindow : Window
{
	// xml attributes translate to constructor parameters in C#
	public MainWindow() : base("Markup App", new Vector2(800, 600))
	{
	}

	// css properties are set in Initialize
	public override void Initialize()
	{
		MinimumSize = new Vector2(800, 600); // from min-width and min-height
		BackgroundColor = Color.FromArgb(0, 240, 240, 240); // from background-color
		
		// child elements are added in Initialize; no content can be added inside an xml element
		Children =
		[
			// xml attributes translate to constructor parameters here as well
			new StackPanel(Orientation.Vertical)
			{
				Children =
				[
					new Label("Hello, world!")
					{
						FontSize = 16, // maybe convert to pt if user uses pixels?
						FontWeight = 600,
						FontFamily = "Arial",
						Color = Color.FromArgb(0, 0, 0)
					},
					new Button("Click me!", OnButtonClick)
					{
						FontSize = 16,
						FontWeight = 600,
						Padding = new Vector4(5, 10, 5, 10)
					}
				]
			}
		];
	}
}