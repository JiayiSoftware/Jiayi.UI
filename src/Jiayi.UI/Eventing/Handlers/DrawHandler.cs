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
		// test squares for now
		window.Graphics.FillRect(new Vector2(100, 100), new Vector2(100, 100), Color.Red);
		window.Graphics.FillRect(new Vector2(200, 200), new Vector2(100, 100), Color.Green);
		window.Graphics.FillRect(new Vector2(300, 300), new Vector2(100, 100), Color.Blue);
			
		window.Graphics.End();
	}
}