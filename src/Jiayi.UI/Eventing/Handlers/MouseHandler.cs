using System.Numerics;
using Windows.Win32.Foundation;
using Jiayi.UI.Core;
using Jiayi.UI.Eventing.Arguments;
using static Windows.Win32.PInvoke;

namespace Jiayi.UI.Eventing.Handlers;

public class MouseHandler() : EventHandler(
	WM_LBUTTONUP, WM_LBUTTONDOWN,  // left button
	WM_MBUTTONUP, WM_MBUTTONDOWN,  // middle button
	WM_RBUTTONUP, WM_RBUTTONDOWN,  // right button
	WM_XBUTTONUP, WM_XBUTTONDOWN,  // extra buttons (typically on the side of the mouse)
	WM_MOUSEMOVE,                  // mouse movement
	WM_MOUSEWHEEL, WM_MOUSEHWHEEL) // mouse wheel
{
	// last mouse position since the last move event for delta
	private Vector2 _lastMousePosition = Vector2.Zero;
	
	public override void HandleMessage(Window window, uint message, WPARAM wParam, LPARAM lParam)
	{
		switch (message)
		{
			// mouse buttons
			case WM_LBUTTONUP:
				window.MouseUp(new MouseButtonEventArgs(MouseButton.Left, GetMousePosition(lParam), false));
				break;
			case WM_LBUTTONDOWN:
				window.MouseDown(new MouseButtonEventArgs(MouseButton.Left, GetMousePosition(lParam), true));
				break;
			case WM_MBUTTONUP:
				window.MouseUp(new MouseButtonEventArgs(MouseButton.Middle, GetMousePosition(lParam), false));
				break;
			case WM_MBUTTONDOWN:
				window.MouseDown(new MouseButtonEventArgs(MouseButton.Middle, GetMousePosition(lParam), true));
				break;
			case WM_RBUTTONUP:
				window.MouseUp(new MouseButtonEventArgs(MouseButton.Right, GetMousePosition(lParam), false));
				break;
			case WM_RBUTTONDOWN:
				window.MouseDown(new MouseButtonEventArgs(MouseButton.Right, GetMousePosition(lParam), true));
				break;
			case WM_XBUTTONUP:
				window.MouseUp(new MouseButtonEventArgs(GetXButton(wParam), GetMousePosition(lParam), false));
				break;
			case WM_XBUTTONDOWN:
				window.MouseDown(new MouseButtonEventArgs(GetXButton(wParam), GetMousePosition(lParam), true));
				break;
			
			// mouse movement
			case WM_MOUSEMOVE:
				var position = GetMousePosition(lParam);
				var delta = position - _lastMousePosition;
				_lastMousePosition = position;
				window.MouseMove(new MouseMoveEventArgs(position, delta));
				break;
			
			// mouse wheel
			case WM_MOUSEWHEEL: // y axis
				var wheelY = (short)(wParam.Value >> 16);
				var pos = GetMousePosition(lParam);
				window.MouseWheel(new MouseWheelEventArgs(pos, new Vector2(0, wheelY)));
				break;
			case WM_MOUSEHWHEEL: // x axis
				var wheelX = (short)(wParam.Value >> 16);
				pos = GetMousePosition(lParam);
				window.MouseWheel(new MouseWheelEventArgs(pos, new Vector2(wheelX, 0)));
				break;
		}
	}
	
	private Vector2 GetMousePosition(LPARAM lParam)
	{
		var x = (short)(lParam.Value & 0xFFFF);
		var y = (short)((lParam.Value >> 16) & 0xFFFF);
		return new Vector2(x, y);
	}
	
	private MouseButton GetXButton(WPARAM wParam)
	{
		var flag = (wParam.Value >> 16) & 0xFFFF;
		return flag == 1 ? MouseButton.XButton1 : MouseButton.XButton2;
	}
}