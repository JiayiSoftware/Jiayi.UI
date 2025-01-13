using System.Text;

namespace Jiayi.UI.Eventing.Arguments;

// best for special keys. if you need to handle text input, use KeyCharEventArgs
public class KeyEventArgs : EventArgs
{
	public KeyCode KeyCode { get; }
	public KeyModifier KeyModifiers { get; }
	public bool KeyDown { get; set; }
	
	public KeyEventArgs(KeyCode keyCode, KeyModifier keyModifiers, bool keyDown)
	{
		KeyCode = keyCode;
		KeyModifiers = keyModifiers;
		KeyDown = keyDown;
	}
}