using System.Drawing;
using System.Numerics;
using Windows.Win32.Foundation;
using Jiayi.UI.Core;
using Jiayi.UI.Eventing.Arguments;
using Vortice.Direct2D1;
using static Windows.Win32.PInvoke;

namespace Jiayi.UI.Eventing.Handlers;

public class KeyboardHandler() : EventHandler(WM_KEYUP, WM_SYSKEYUP, WM_KEYDOWN, WM_SYSKEYDOWN, WM_CHAR)
{
	// current key modifiers
	private KeyModifier _modifiers;
	
	public override void HandleMessage(Window window, uint message, WPARAM wParam, LPARAM lParam)
	{
		switch (message)
		{
			// key up
			case WM_KEYUP:
			case WM_SYSKEYUP:
				var code = (KeyCode)wParam.Value;
				_modifiers &= ~KeyCodeToModifier(code);
				window.KeyUp(new KeyEventArgs(code, _modifiers, false));
				break;
			
			// key down
			case WM_KEYDOWN:
			case WM_SYSKEYDOWN:
				code = (KeyCode)wParam.Value;
				_modifiers |= KeyCodeToModifier(code);
				window.KeyDown(new KeyEventArgs(code, _modifiers, true));
				break;
			
			// character input
			case WM_CHAR:
				window.KeyChar(new KeyCharEventArgs((char)wParam.Value));
				break;
		}
	}
	
	// this syntax is funny
	private KeyModifier KeyCodeToModifier(KeyCode code) => code switch
	{
		KeyCode.Shift or KeyCode.LeftShift or KeyCode.RightShift => KeyModifier.Shift,
		KeyCode.Control or KeyCode.LeftControl or KeyCode.RightControl => KeyModifier.Control,
		KeyCode.Alt or KeyCode.LeftAlt or KeyCode.RightAlt => KeyModifier.Alt,
		KeyCode.Windows => KeyModifier.Windows,
		_ => KeyModifier.None
	};
}