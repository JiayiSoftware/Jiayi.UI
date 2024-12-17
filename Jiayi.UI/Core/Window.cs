using System.ComponentModel;
using System.Numerics;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Jiayi.UI.Render;
using static Windows.Win32.PInvoke;
using static Windows.Win32.UI.WindowsAndMessaging.SET_WINDOW_POS_FLAGS;
using static Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_EX_STYLE;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_STYLE;

namespace Jiayi.UI.Core;

public unsafe class Window
{
	public nint Handle { get; }
	public Graphics Graphics { get; } = new();
	
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
			
			return new string(title);
		}
		set => SetWindowText((HWND)Handle, value);
	}

	public Window(string title, Vector2 size)
	{
		var dpi = Graphics.DrawData.Dpi;

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
					CW_USEDEFAULT,
					CW_USEDEFAULT,
					(int)(size.X * dpi.X),
					(int)(size.Y * dpi.Y),
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
		Application.Current.Windows.Add(Handle, this);
	}
	
	internal bool WindowProc(uint msg, WPARAM wParam, LPARAM lParam)
	{
		return false;
	}

	public void Show()
	{
		ShowWindow((HWND)Handle, SW_NORMAL);
		UpdateWindow((HWND)Handle);
	}
}