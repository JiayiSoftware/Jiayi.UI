using Jiayi.UI.Eventing.Arguments;

namespace Jiayi.UI.Interfaces;

public interface IMouseListener
{
	public void MouseDown(MouseButtonEventArgs e);
	public void MouseUp(MouseButtonEventArgs e);
	public void MouseMove(MouseMoveEventArgs e);
	public void MouseWheel(MouseWheelEventArgs e);
}