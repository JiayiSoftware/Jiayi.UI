using Windows.Win32.Foundation;
using Jiayi.UI.Core;
using static Windows.Win32.PInvoke;

namespace Jiayi.UI.Eventing.Handlers;

public class ExitHandler() : EventHandler(WM_CLOSE)
{
	public override void HandleMessage(Window window, uint message, WPARAM wParam, LPARAM lParam)
	{
		var isMain = window.IsMainWindow;
		
		window.Close();
				
		// exit if main window is closed
		if (isMain)
		{
			Application.Current.Exit();
		}
	}
}