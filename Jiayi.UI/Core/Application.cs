using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using static Windows.Win32.PInvoke;
using static Windows.Win32.UI.WindowsAndMessaging.PEEK_MESSAGE_REMOVE_TYPE;
using static Windows.Win32.UI.WindowsAndMessaging.WNDCLASS_STYLES;

namespace Jiayi.UI.Core;

public unsafe class Application
{
	public static Application Current { get; } = new();
	
	internal readonly string WindowClassName = "JiayiUIWindow";
	internal HINSTANCE HInstance;

	public Dictionary<nint, Window> Windows { get; } = new();
	public Window? MainWindow => Windows.Values.FirstOrDefault();
	
	public event Action? OnStartup;
	public event Action? OnExit;

	public Application()
	{
		#nullable disable
		HInstance = (HINSTANCE)GetModuleHandle((string)null).DangerousGetHandle();
		#nullable restore
		
		var modulePath = stackalloc char[(int)MAX_PATH];
		GetModuleFileName(HInstance, modulePath, MAX_PATH);
		
		var icon = ExtractIcon(HInstance, modulePath, 0);
		
		fixed (char* className = WindowClassName)
		{
			var windowClass = new WNDCLASSEXW
			{
				cbSize = (uint)Unsafe.SizeOf<WNDCLASSEXW>(),
				style = CS_HREDRAW | CS_VREDRAW,
				lpfnWndProc = &WindowProc,
				hInstance = HInstance,
				hCursor = LoadCursor(default, IDC_ARROW),
				hbrBackground = default,
				hIcon = icon,
				lpszClassName = className
			};
			
			var atom = RegisterClassEx(in windowClass);
			if (atom == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
		}
	}

	public void Run()
	{
		if (MainWindow == null)
		{
			throw new InvalidOperationException("No main window is set. You must create a window before calling Run.");
		}
		
		MainWindow.Show();
		OnStartup?.Invoke();
		
		MSG msg = default;
		while (msg.message != WM_QUIT)
		{
			if (PeekMessage(out msg, default, 0, 0, PM_REMOVE))
			{
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			}
		}
		
		OnExit?.Invoke();
	}
	
	public void Exit() => PostQuitMessage(0);
	
	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
	private static LRESULT WindowProc(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam)
	{
		if (!Current.Windows.TryGetValue(hWnd, out var window)) return DefWindowProc(hWnd, msg, wParam, lParam);

		// did the window handle the message?
		return window.WindowProc(msg, wParam, lParam) ? new LRESULT(0) : DefWindowProc(hWnd, msg, wParam, lParam);
	}
}