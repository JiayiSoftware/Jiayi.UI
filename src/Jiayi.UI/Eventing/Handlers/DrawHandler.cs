using System.Drawing;
using System.Numerics;
using Windows.Win32.Foundation;
using Jiayi.UI.Core;
using Vortice.Direct2D1;
using static Windows.Win32.PInvoke;

namespace Jiayi.UI.Eventing.Handlers;

public class DrawHandler() : EventHandler(WM_PAINT, WM_DISPLAYCHANGE)
{
	public override void HandleMessage(Window window, uint message, WPARAM wParam, LPARAM lParam)
	{
		if (window.Graphics.GetWindowState() == WindowState.Occluded)
		{
			// no draw
			return;
		}
		
		window.Graphics.Begin();
		window.Graphics.Clear(window.BackgroundColor);
				
		// draw widgets here
		
		//window.Graphics.DrawText("Hello, World!", "Arial", new Vector2(10, 10), Color.Black, 96);
			
		window.Graphics.End();
	}
}