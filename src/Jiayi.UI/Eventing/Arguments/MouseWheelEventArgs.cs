using System.Numerics;

namespace Jiayi.UI.Eventing.Arguments;

public class MouseWheelEventArgs : EventArgs
{
	public Vector2 MousePosition { get; }
	public Vector2 WheelDelta { get; }
	
	public MouseWheelEventArgs(Vector2 mousePosition, Vector2 wheelDelta)
	{
		MousePosition = mousePosition;
		WheelDelta = wheelDelta;
	}
}