using System.Numerics;
using Jiayi.UI.Core;

namespace Jiayi.UI.Eventing.Arguments;

public class MouseButtonEventArgs : EventArgs
{
	public MouseButton ButtonPressed { get; }
	public Vector2 MousePosition { get; }
	public bool ButtonDown { get; }
	
	public MouseButtonEventArgs(MouseButton buttonPressed, Vector2 mousePosition, bool buttonDown)
	{
		ButtonPressed = buttonPressed;
		MousePosition = mousePosition;
		ButtonDown = buttonDown;
	}
}