using Vortice.Mathematics;
using Color = System.Drawing.Color;

namespace Jiayi.UI.Extensions;

internal static class ColorExtensions
{
	public static Color4 ToColor4(this Color color)
	{
		return new Color4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
	}
}