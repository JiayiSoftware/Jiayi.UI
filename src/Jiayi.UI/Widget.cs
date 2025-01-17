using System.Numerics;
using Jiayi.UI.Render;
using Jiayi.UI.Widgets;

namespace Jiayi.UI;

public class Widget
{
	// positioning and sizing stuff
	public Vector2 Position { get; set; } // position in DIPs (device-independent pixels)
	
	// another way to read anchor point: <position> units from the <anchor> corner
	public Anchor Anchor { get; set; } = Anchor.TopLeft;
	public Vector2 Size { get; set; } // size in DIPs
	
	// other things
	public bool Visible { get; set; } = true;
	public Widget? Parent { get; set; } // null if root
	protected bool ClipChildren { get; set; } = true;

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
	public virtual Window Window => Parent?.Window ?? throw new InvalidOperationException("Widget is not attached to a window.");
	
	// no constructor, use object initializer

	public virtual Vector2 GetAbsolutePosition()
	{
		var parentPosition = Parent?.GetAbsolutePosition() ?? Vector2.Zero;
		var absolutePosition = parentPosition + Position;
		
		// calculate anchor
		return Anchor switch
		{
			Anchor.TopLeft =>
				// normal coordinates
				absolutePosition,
			Anchor.TopCenter =>
				// x is centered
				absolutePosition + new Vector2((Parent!.Size.X - Size.X) / 2, 0),
			Anchor.TopRight =>
				// x is flipped; positive x is left
				// the x value in absolutePosition can be made negative to make this easier
				new Vector2(-absolutePosition.X + Parent!.Size.X - Size.X, absolutePosition.Y),
			Anchor.MiddleLeft =>
				// y is centered
				absolutePosition + new Vector2(0, (Parent!.Size.Y - Size.Y) / 2),
			Anchor.MiddleCenter =>
				// both x and y are centered
				absolutePosition + (Parent!.Size - Size) / 2,
			Anchor.MiddleRight =>
				// x is flipped; positive x is left, y is centered
				new Vector2(-absolutePosition.X + Parent!.Size.X - Size.X, absolutePosition.Y + (Parent!.Size.Y - Size.Y) / 2),
			Anchor.BottomLeft =>
				// y is flipped; positive y is up
				new Vector2(absolutePosition.X, -absolutePosition.Y + Parent!.Size.Y - Size.Y),
			Anchor.BottomCenter =>
				// x is centered, y is flipped; positive y is up
				new Vector2(absolutePosition.X + (Parent!.Size.X - Size.X) / 2, -absolutePosition.Y + Parent!.Size.Y - Size.Y),
			Anchor.BottomRight =>
				// x is flipped; positive x is left, y is flipped; positive y is up
				-absolutePosition + Parent!.Size - Size,
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	public virtual void Render(Graphics g)
	{
		// render children
		foreach (var child in Children)
		{
			// TODO: don't set parent here i guess
			child.Parent = this;
			
			if (!child.Visible) continue;

			// skip drawing for children that are outside the parent's bounds, if clipping is enabled
			if ((ClipChildren && IsInside(child.Position, child.Size)) || !ClipChildren)
			{
				child.Render(g);
			}
		}
	}

	protected bool IsInside(Vector2 pos)
	{
		var absolutePosition = GetAbsolutePosition();
		return pos.X >= absolutePosition.X
		       && pos.Y >= absolutePosition.Y
		       && pos.X <= absolutePosition.X + Size.X
		       && pos.Y <= absolutePosition.Y + Size.Y;
	}

	protected bool IsInside(Vector2 pos, Vector2 size)
	{
		var absolutePosition = GetAbsolutePosition();
		return pos.X + size.X >= absolutePosition.X
		       || pos.Y + size.Y >= absolutePosition.Y
		       || pos.X <= absolutePosition.X + Size.X
		       || pos.Y <= absolutePosition.Y + Size.Y;
	}
}