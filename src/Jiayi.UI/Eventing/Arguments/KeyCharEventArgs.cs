namespace Jiayi.UI.Eventing.Arguments;

// best for text input. if you need to handle special keys, use KeyEventArgs
public class KeyCharEventArgs : EventArgs
{
	public char KeyChar { get; }
	
	public KeyCharEventArgs(char keyChar)
	{
		KeyChar = keyChar;
	}
}