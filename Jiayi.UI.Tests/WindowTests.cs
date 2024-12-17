using System.Numerics;
using Jiayi.UI.Core;

namespace Jiayi.UI.Tests;

public class WindowTests
{
	private Window _window;
	
	[SetUp]
	public void Setup()
	{
		_window = new Window("Jiayi UI Tests", new Vector2(800, 600));
		Run();
	}

	private void Run()
	{
		// run on another thread to prevent blocking the main thread
		Task.Run(() =>
		{
			Application.Current.Run();
		});
	}
	
	[TearDown]
	public void TearDown()
	{
		Application.Current.Exit();
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
}