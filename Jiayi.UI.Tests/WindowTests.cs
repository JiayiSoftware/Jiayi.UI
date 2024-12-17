using System.Numerics;
using Jiayi.UI.Core;

namespace Jiayi.UI.Tests;

public class WindowTests
{
	private Window _window;
	
	[SetUp]
	public void Setup()
	{
		Task.Run(() =>
		{
			_window = new Window("Jiayi UI Tests", new Vector2(800, 600));
			Application.Current.Run();
		});
	}

	[Test]
	public void WindowPosition()
	{
		_window.Position = new Vector2(100, 100);
		Assert.That(_window.Position, Is.EqualTo(new Vector2(100, 100)));
	}
	
	[Test]
	public void WindowSize()
	{
		_window.Size = new Vector2(400, 300);
		Assert.That(_window.Size, Is.EqualTo(new Vector2(400, 300)));
	}
	
	[Test]
	public void WindowTitle()
	{
		_window.Title = "Hello, World!";
		Assert.That(_window.Title, Is.EqualTo("Hello, World!"));
	}
	
	// literally just run the window for a bit
	[Test]
	public void RunWindow()
	{
		Thread.Sleep(5000);
	}
}