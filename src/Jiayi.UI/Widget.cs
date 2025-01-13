using System.Numerics;
using Jiayi.UI.Widgets;

namespace Jiayi.UI;

public class Widget
{
	// positioning and sizing stuff
	public Vector2 Position { get; set; } // position in reference units
	
	// another way to read anchor point: <position> units from the <anchor> corner
	public Anchor Anchor { get; set; } = Anchor.TopLeft;
	public Vector2 Size { get; set; } // size in DIPs (device-independent pixels)
	
	// other things
	public bool Visible { get; set; } = true;
	public Widget? Parent { get; set; } // null if root

	public RootWidget Root
	{
		get
		{
			// find the root widget in parent chain
			var current = this;
			while (current.Parent != null)
			{
				current = current.Parent;
			}
			return (RootWidget)current;
		}
	}
	
	public List<Widget> Children { get; } = new();
	public Window Window => Parent?.Window ?? throw new InvalidOperationException("Widget is not attached to a window.");
	
	// empty constructor, use object initializer
	public Widget() { }

	public virtual Vector2 GetAbsolutePosition()
	{
		var parentPosition = Parent?.GetAbsolutePosition() ?? Vector2.Zero;
		var scaledPosition = Position * (Window.Size / Root.ReferenceResolution);
		
		// calculate anchor
		return Anchor switch
		{
			Anchor.TopLeft =>
				// normal coordinates
				parentPosition + scaledPosition,
			Anchor.TopCenter =>
				// x is centered
				parentPosition + new Vector2((Window.Size.X - Size.X) / 2, scaledPosition.Y),
			Anchor.TopRight =>
				// x is flipped; positive x is left
				parentPosition + new Vector2(Window.Size.X - Size.X - scaledPosition.X, scaledPosition.Y),
			Anchor.MiddleLeft =>
				// y is centered
				parentPosition + new Vector2(scaledPosition.X, (Window.Size.Y - Size.Y) / 2),
			Anchor.MiddleCenter =>
				// both x and y are centered
				parentPosition + (Window.Size - Size) / 2,
			Anchor.MiddleRight =>
				// x is flipped; positive x is left, y is centered
				parentPosition + new Vector2(Window.Size.X - Size.X - scaledPosition.X, (Window.Size.Y - Size.Y) / 2),
			Anchor.BottomLeft =>
				// y is flipped; positive y is up
				parentPosition + new Vector2(scaledPosition.X, Window.Size.Y - Size.Y - scaledPosition.Y),
			Anchor.BottomCenter =>
				// x is centered, y is flipped; positive y is up
				parentPosition + new Vector2((Window.Size.X - Size.X) / 2, Window.Size.Y - Size.Y - scaledPosition.Y),
			Anchor.BottomRight =>
				// x is flipped; positive x is left, y is flipped; positive y is up
				parentPosition + new Vector2(Window.Size.X - Size.X - scaledPosition.X,
					Window.Size.Y - Size.Y - scaledPosition.Y),
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}