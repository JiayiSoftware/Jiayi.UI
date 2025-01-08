namespace Jiayi.UI.Core;

[Flags]
public enum KeyModifier : uint
{
	None = 0,
	Alt = 1,
	Control = 2,
	Shift = 4,
	Windows = 8
}