using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using static Windows.Win32.PInvoke;
using static Windows.Win32.UI.WindowsAndMessaging.WNDCLASS_STYLES;

namespace Jiayi.UI;

public unsafe class Application
{
	public static Application Current { get; } = new();
	
	internal readonly string WindowClassName = "JiayiUIWindow";
	internal HINSTANCE HInstance;

	public Dictionary<nint, Window> Windows { get; } = new();
	public Window? MainWindow => Windows.Values.FirstOrDefault();
	
	public event Action? OnStartup;
	public event Action? OnExit;

	private Application()
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
		do
		{
			if (!GetMessage(out msg, default, 0, 0)) continue;
			
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		} while (msg.message != WM_QUIT);
		
		OnExit?.Invoke();
	}
	
	public void Exit()
	{
		fixed (char* className = WindowClassName) 
			UnregisterClass(className, HInstance);
		
		PostQuitMessage(0);
	}

	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
	private static LRESULT WindowProc(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam)
	{
		if (!Current.Windows.TryGetValue(hWnd, out var window)) return DefWindowProc(hWnd, msg, wParam, lParam);

		// did the window handle the message?
		return window.WindowProc(msg, wParam, lParam) ? new LRESULT(0) : DefWindowProc(hWnd, msg, wParam, lParam);
	}
}