﻿using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Jiayi.UI.Eventing.Arguments;
using Jiayi.UI.Eventing.Handlers;
using Jiayi.UI.Interfaces;
using Jiayi.UI.Render;
using Jiayi.UI.Widgets;
using static Windows.Win32.PInvoke;
using static Windows.Win32.UI.WindowsAndMessaging.SET_WINDOW_POS_FLAGS;
using static Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_EX_STYLE;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_STYLE;
using EventHandler = Jiayi.UI.Eventing.EventHandler;

namespace Jiayi.UI;

public unsafe class Window : IKeyboardListener, IMouseListener
{
	public nint Handle { get; set; }
	public Graphics Graphics { get; } = new();
	public bool IsMainWindow => Application.Current.MainWindow == this;
	
	// cool properties
	public Vector2 Position
	{
		get
		{
			GetWindowRect((HWND)Handle, out var rect);
			return new Vector2(rect.left, rect.top);
		}
		set => SetWindowPos((HWND)Handle, HWND.HWND_TOP, (int)value.X, (int)value.Y, 0, 0, SWP_NOSIZE);
	}
	
	public Vector2 Size
	{
		get
		{
			GetWindowRect((HWND)Handle, out var rect);
			return new Vector2(rect.right - rect.left, rect.bottom - rect.top);
		}
		set => SetWindowPos((HWND)Handle, HWND.HWND_TOP, 0, 0, (int)value.X, (int)value.Y, SWP_NOMOVE);
	}
	
	public string Title
	{
		get
		{
			var length = GetWindowTextLength((HWND)Handle);
			var title = new char[length + 1];
			
			fixed (char* ptr = title)
			{
				GetWindowText((HWND)Handle, ptr, length + 1);
			}
			
			var t = new string(title);
			return t[..^1]; // remove null-terminator
		}
		set => SetWindowText((HWND)Handle, value);
	}
	
	public Color BackgroundColor { get; set; } = Color.White;
	public Vector2 MinimumSize { get; set; } = new(300, 300);
	public Vector2 MaximumSize { get; set; } = new(10000, 10000);
	
	public RootWidget RootWidget { get; private set; } = null!;

	public List<Widget> Children
	{
		get => RootWidget.Children;
		set
		{
			RootWidget.Children.Clear();
			RootWidget.Children.AddRange(value);
		}
	}
	
	// cool events
	private readonly List<EventHandler> _eventHandlers = new();

	public Window(string title, Vector2 size)
	{
		// add event handlers
		AddEventHandler<ExitHandler>();
		AddEventHandler<DrawHandler>();
		AddEventHandler<ResizeHandler>();
		AddEventHandler<SizeLimitsHandler>();
		AddEventHandler<KeyboardHandler>();
		AddEventHandler<MouseHandler>();
		
		// default position
		var position = new Vector2(CW_USEDEFAULT, CW_USEDEFAULT);
		
		FinishInitialize(title, position, size);
	}
	
	// constructor for custom position
	public Window(string title, Vector2 position, Vector2 size)
	{
		// add event handlers
		AddEventHandler<ExitHandler>();
		AddEventHandler<DrawHandler>();
		AddEventHandler<ResizeHandler>();
		AddEventHandler<SizeLimitsHandler>();
		AddEventHandler<KeyboardHandler>();
		AddEventHandler<MouseHandler>();
		
		FinishInitialize(title, position, size);
	}

	private void FinishInitialize(string title, Vector2 position, Vector2 size)
	{
		var dpi = Graphics.DrawData.Dpi;
		var scaledPosition = position.X == CW_USEDEFAULT ? position : position * dpi / 96;
		var scaledSize = size * dpi / 96;
		
		fixed (char* className = Application.Current.WindowClassName)
		{
			// stupid
			fixed (char* windowTitle = title)
			{
				Handle = CreateWindowEx(
					WS_EX_APPWINDOW,
					className,
					windowTitle, 
					WS_OVERLAPPEDWINDOW,
					(int)scaledPosition.X,
					(int)scaledPosition.Y,
					(int)scaledSize.X,
					(int)scaledSize.Y,
					HWND.Null,
					HMENU.Null,
					Application.Current.HInstance
				);
			}
		}

		if (Handle == nint.Zero)
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		Graphics.InitializeWindow(this);
		RootWidget = new RootWidget(this);
		
		Application.Current.Windows.Add(Handle, this);
	}

	private void AddEventHandler<T>() where T : EventHandler, new()
	{
		_eventHandlers.Add(new T());
	}
	
	internal bool WindowProc(uint msg, WPARAM wParam, LPARAM lParam)
	{
		foreach (var handler in _eventHandlers.Where(handler => handler.HandlesMessage(msg)))
		{
			handler.HandleMessage(this, msg, wParam, lParam);
			return true;
		}

		return false;
	}

	public void Show()
	{
		ShowWindow((HWND)Handle, SW_NORMAL);
		UpdateWindow((HWND)Handle);
	}
	
	public void Close()
	{
		var isMainWindow = IsMainWindow;
		
		DestroyWindow((HWND)Handle);
		Application.Current.Windows.Remove(Handle);
		
		if (isMainWindow)
		{
			Application.Current.Exit();
		}
	}
	
	// cool methods for cool inheritance
	public virtual void Initialize() {} // implement and call this in your constructor

	public virtual void KeyDown(KeyEventArgs e)
	{
		RootWidget.KeyDown(e);
	}
	
	public virtual void KeyUp(KeyEventArgs e)
	{
		RootWidget.KeyUp(e);
	}
	
	public virtual void KeyChar(KeyCharEventArgs e)
	{
		RootWidget.KeyChar(e);
	}
	
	public virtual void MouseDown(MouseButtonEventArgs e)
	{
		RootWidget.MouseDown(e);
	}
	
	public virtual void MouseUp(MouseButtonEventArgs e)
	{
		RootWidget.MouseUp(e);
	}
	
	public virtual void MouseMove(MouseMoveEventArgs e)
	{
		RootWidget.MouseMove(e);
	}
	
	public virtual void MouseWheel(MouseWheelEventArgs e)
	{
		RootWidget.MouseWheel(e);
	}
}