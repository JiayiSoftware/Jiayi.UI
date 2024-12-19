using Windows.Win32.Foundation;
using Jiayi.UI.Core;

namespace Jiayi.UI.Eventing;

public abstract class EventHandler
{
	private readonly List<uint> _messages;

	protected EventHandler(params List<uint> messages)
	{
		_messages = messages;
	}
	
	public bool HandlesMessage(uint message) => _messages.Contains(message);
	public abstract void HandleMessage(Window window, uint message, WPARAM wParam, LPARAM lParam);
}