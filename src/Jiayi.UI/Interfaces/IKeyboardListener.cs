using Jiayi.UI.Eventing.Arguments;

namespace Jiayi.UI.Interfaces;

public interface IKeyboardListener
{
	public void KeyDown(KeyEventArgs e);
	public void KeyUp(KeyEventArgs e);
	public void KeyChar(KeyCharEventArgs e);
}