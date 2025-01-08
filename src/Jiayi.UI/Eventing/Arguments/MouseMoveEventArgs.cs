using System.Numerics;

namespace Jiayi.UI.Eventing.Arguments;

public class MouseMoveEventArgs : EventArgs
{
	public Vector2 MousePosition { get; }
	public Vector2 MouseDelta { get; }
	
	public MouseMoveEventArgs(Vector2 mousePosition, Vector2 mouseDelta)
	{
		MousePosition = mousePosition;
		MouseDelta = mouseDelta;
	}
}