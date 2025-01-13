using System.Numerics;
using Windows.Win32.Foundation;
using static Windows.Win32.PInvoke;

namespace Jiayi.UI.Eventing.Handlers;

public class ResizeHandler() : EventHandler(WM_SIZE)
{
	public override void HandleMessage(Window window, uint message, WPARAM wParam, LPARAM lParam)
	{
		var width = (short)((ulong)lParam.Value & 0xFFFF);
		var height = (short)(((ulong)lParam.Value >> 16) & 0xFFFF);
		
		window.Graphics.Resize(new Vector2(width, height));
	}
}