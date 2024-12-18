using Windows.Win32.Foundation;
using Jiayi.UI.Core;

namespace Jiayi.UI.Eventing;

public abstract class EventHandler
{
	private readonly uint[] _messages;

	protected EventHandler(params uint[] messages)
	{
		_messages = messages;
	}
	
	public bool HandlesMessage(uint message) => _messages.Any(m => m == message);
	public abstract void HandleMessage(Window window, uint message, WPARAM wParam, LPARAM lParam);
}