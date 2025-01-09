using Vortice.DirectWrite;

namespace Jiayi.UI.Render.Caching;

public class FontCache
{
	// pretty much everything in DWrite is device independent
	private IDWriteFactory5 _writeFactory;
	private IDWriteFontCollection1 _fontCollection;
	private Dictionary<int, IDWriteTextFormat> _textFormats;
	private Dictionary<int, IDWriteTextLayout> _textLayouts;
}