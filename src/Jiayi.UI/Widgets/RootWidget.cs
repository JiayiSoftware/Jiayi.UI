using System.Numerics;
using Jiayi.UI.Eventing.Arguments;
using Jiayi.UI.Interfaces;
using Jiayi.UI.Render;

namespace Jiayi.UI.Widgets;

public class RootWidget : Widget, IKeyboardListener, IMouseListener
{
	public override Window Window => _window;
	private Window _window;

	public RootWidget(Window window)
	{
		_window = window;
	}
	
	public override Vector2 GetAbsolutePosition() => Vector2.Zero;

	public void KeyDown(KeyEventArgs e)
	{
		foreach (var child in Children.Where(x => x.Visible))
		{
			if (child is not IKeyboardListener listener) continue;
			listener.KeyDown(e);
		}
	}

	public void KeyUp(KeyEventArgs e)
	{
		foreach (var child in Children.Where(x => x.Visible))
		{
			if (child is not IKeyboardListener listener) continue;
			listener.KeyUp(e);
		}
	}

	public void KeyChar(KeyCharEventArgs e)
	{
		foreach (var child in Children.Where(x => x.Visible))
		{
			if (child is not IKeyboardListener listener) continue;
			listener.KeyChar(e);
		}
	}

	public void MouseDown(MouseButtonEventArgs e)
	{
		foreach (var child in Children.Where(x => x.Visible))
		{
			if (child is not IMouseListener listener) continue;
			listener.MouseDown(e);
		}
	}

	public void MouseUp(MouseButtonEventArgs e)
	{
		foreach (var child in Children.Where(x => x.Visible))
		{
			if (child is not IMouseListener listener) continue;
			listener.MouseUp(e);
		}
	}

	public void MouseMove(MouseMoveEventArgs e)
	{
		foreach (var child in Children.Where(x => x.Visible))
		{
			if (child is not IMouseListener listener) continue;
			listener.MouseMove(e);
		}
	}

	public void MouseWheel(MouseWheelEventArgs e)
	{
		foreach (var child in Children.Where(x => x.Visible))
		{
			if (child is not IMouseListener listener) continue;
			listener.MouseWheel(e);
		}
	}

	public override void Render(Graphics g)
	{
		// TODO: size should be in DIPs
		Size = _window.Size;
		base.Render(g);
	}
}