using System.Text;
using Jiayi.UI.Core;

namespace Jiayi.UI.Eventing.Arguments;

// best for special keys. if you need to handle text input, use KeyCharEventArgs
public class KeyEventArgs : EventArgs
{
	public KeyCode KeyCode { get; }
	public KeyModifier Modifiers { get; }
	public bool KeyDown { get; set; }
	
	public KeyEventArgs(KeyCode keyCode, KeyModifier modifiers, bool keyDown)
	{
		KeyCode = keyCode;
		Modifiers = modifiers;
		KeyDown = keyDown;
	}
}