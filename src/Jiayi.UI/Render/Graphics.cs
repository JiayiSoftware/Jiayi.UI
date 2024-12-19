using System.ComponentModel;
using System.Numerics;
using Windows.Win32.Foundation;
using Jiayi.UI.Core;
using Jiayi.UI.Extensions;
using Vortice;
using Vortice.Direct2D1;
using Vortice.Mathematics;
using static Windows.Win32.PInvoke;
using static Vortice.Direct2D1.D2D1;
using Color = System.Drawing.Color;
using RenderTargetProperties = Vortice.Direct2D1.RenderTargetProperties;
using ResultCode = Vortice.Direct2D1.ResultCode;

namespace Jiayi.UI.Render;

public sealed class Graphics : IDisposable
{
	public DrawData DrawData { get; } = new();
	
	// NOTE: when any of these resources get lost,
	// the resource and anything below it must be recreated
	// in other words, all variables are ordered by dependency
	
	// device independent resources
	private readonly ID2D1Factory _d2dFactory;
	
	// device dependent resources
	private ID2D1HwndRenderTarget? _renderTarget;
	private readonly Dictionary<int, ID2D1SolidColorBrush> _brushes = new();
	
	// other things (dependency ordering stops here)
	private nint _windowHandle = nint.Zero;

	public Graphics()
	{
		// create device independent resources
		_d2dFactory = D2D1CreateFactory<ID2D1Factory>();
		
		// we can only fill dpi here
		DrawData.Dpi = _d2dFactory.DesktopDpi;
	}

	public void InitializeWindow(Window window)
	{
		_windowHandle = window.Handle;
		CreateDeviceResources();
	}

	private void CreateDeviceResources()
	{
		if (_renderTarget != null) return;
		
		GetClientRect((HWND)_windowHandle, out var rect);
		var size = new SizeI(rect.right - rect.left, rect.bottom - rect.top);

		var rtp = new HwndRenderTargetProperties
		{
			Hwnd = _windowHandle,
			PixelSize = size,
			PresentOptions = PresentOptions.None
		};
		_renderTarget = _d2dFactory.CreateHwndRenderTarget(new RenderTargetProperties(), rtp);

		// initialize fonts and images here
	}

	private void DiscardDeviceResources()
	{
		_brushes.Clear();
		_renderTarget?.Dispose();
		_renderTarget = null;
	}

	public void Begin()
	{
		CreateDeviceResources();
		
		_renderTarget!.BeginDraw();
		_renderTarget.Transform = Matrix3x2.Identity;
		
		var size = _renderTarget.PixelSize;
		DrawData.ViewportSize = new Vector2(size.Width, size.Height);
	}
	
	public void End()
	{
		var result = _renderTarget!.EndDraw();
		
		if (result == ResultCode.RecreateTarget)
		{
			// d3d device lost, recreate resources
			DiscardDeviceResources();
		}
		else if (result.Failure)
		{
			throw new Win32Exception(result.Code);
		}
	}
	
	public void Clear(Color color)
	{
		_renderTarget!.Clear(color.ToColor4());
	}

	public void Resize(Vector2 newSize)
	{
		_renderTarget!.Resize(new SizeI((int)newSize.X, (int)newSize.Y));
	}

	public WindowState GetWindowState() => _renderTarget!.CheckWindowState();

	private ID2D1SolidColorBrush GetBrush(Color color)
	{
		var hash = color.GetHashCode();
		if (_brushes.TryGetValue(hash, out var brush))
		{
			return brush;
		}

		var newBrush = _renderTarget!.CreateSolidColorBrush(color.ToColor4());
		_brushes.Add(hash, newBrush);
		return newBrush;
	}

	public void DrawRect(Vector2 pos, Vector2 size, Color color, float thickness)
	{
		_renderTarget!.DrawRectangle(new RawRectF(pos.X, pos.Y, pos.X + size.X, pos.Y + size.Y), 
			GetBrush(color), thickness);
	}
	
	public void FillRect(Vector2 pos, Vector2 size, Color color)
	{
		_renderTarget!.FillRectangle(new RawRectF(pos.X, pos.Y, pos.X + size.X, pos.Y + size.Y), GetBrush(color));
	}
	
	public void DrawRoundedRect(Vector2 pos, Vector2 size, float radius, Color color, float thickness)
	{
		var roundedRect = new RoundedRectangle
		{
			Rect = new RawRectF(pos.X, pos.Y, pos.X + size.X, pos.Y + size.Y),
			RadiusX = radius,
			RadiusY = radius
		};
		
		_renderTarget!.DrawRoundedRectangle(roundedRect, GetBrush(color), thickness);
	}
	
	public void FillRoundedRect(Vector2 pos, Vector2 size, float radius, Color color)
	{
		var roundedRect = new RoundedRectangle
		{
			Rect = new RawRectF(pos.X, pos.Y, pos.X + size.X, pos.Y + size.Y),
			RadiusX = radius,
			RadiusY = radius
		};
		
		_renderTarget!.FillRoundedRectangle(roundedRect, GetBrush(color));
	}
	
	public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness)
	{
		_renderTarget!.DrawLine(start, end, GetBrush(color), thickness);
	}
	
	public void DrawCircle(Vector2 center, float radius, Color color, float thickness)
	{
		_renderTarget!.DrawEllipse(new Ellipse(center, radius, radius), GetBrush(color), thickness);
	}
	
	public void FillCircle(Vector2 center, float radius, Color color)
	{
		_renderTarget!.FillEllipse(new Ellipse(center, radius, radius), GetBrush(color));
	}
	
	public void DrawBitmap(ID2D1Bitmap bitmap, Vector2 pos, Vector2 size, float opacity = 1.0f)
	{
		_renderTarget!.DrawBitmap(bitmap, new RawRectF(pos.X, pos.Y, pos.X + size.X, pos.Y + size.Y), 
			opacity, BitmapInterpolationMode.Linear, null);
	}
	
	// dispose pattern
	private bool _disposed;

	private void Dispose(bool disposing)
	{
		if (_disposed) return;
		
		if (disposing)
		{
			DiscardDeviceResources();
			_d2dFactory.Dispose();
		}
		
		_windowHandle = nint.Zero;
			
		_disposed = true;
	}
	
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	
	~Graphics()
	{
		Dispose(false);
	}
}