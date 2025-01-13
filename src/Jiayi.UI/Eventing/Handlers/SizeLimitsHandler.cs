using System.Drawing;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using static Windows.Win32.PInvoke;

namespace Jiayi.UI.Eventing.Handlers;

public unsafe class SizeLimitsHandler() : EventHandler(WM_GETMINMAXINFO)
{
	public override void HandleMessage(Window window, uint message, WPARAM wParam, LPARAM lParam)
	{
		var info = (MINMAXINFO*)lParam.Value;
		info->ptMinTrackSize = new Point((int)window.MinimumSize.X, (int)window.MinimumSize.Y);
		info->ptMaxTrackSize = new Point((int)window.MaximumSize.X, (int)window.MaximumSize.Y);
	}
}