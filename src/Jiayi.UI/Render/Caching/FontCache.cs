using System.Numerics;
using Vortice.DirectWrite;
using static Vortice.DirectWrite.DWrite;

namespace Jiayi.UI.Render.Caching;

public class FontCache : IDisposable
{
	// pretty much everything in DWrite is device independent
	private readonly IDWriteFactory5 _writeFactory;
	private readonly IDWriteFontSetBuilder1 _fontSetBuilder;
	private IDWriteFontCollection1 _fontCollection = null!; // initialized in constructor but my IDE doesn't know that
	
	// none of the styling in this form is used, but it's required to create a text format
	private IDWriteTextFormat _defaultFormat;
	
	// cache values
	private Dictionary<int, IDWriteTextLayout> _textLayoutCache = new();
	private const float CACHE_THRESHOLD = 1.5f; // value to reach before increasing cache size
	private float _cacheLoad; // increased when the cache is cleared
	private int _maxCacheSize = 10; // initial cache size

	public FontCache()
	{
		_writeFactory = DWriteCreateFactory<IDWriteFactory5>();

		// create font set builder and add fonts stored in Resources/Fonts
		_fontSetBuilder = _writeFactory.CreateFontSetBuilder();
		if (Directory.Exists("Resources/Fonts"))
		{
			var files = Directory.GetFiles("Resources/Fonts", "*.ttf");
			foreach (var file in files)
			{
				AddFont(file);
			}
		}
		
		// add system fonts as well
		var systemFonts = _writeFactory.SystemFontSet;
		_fontSetBuilder.AddFontSet(systemFonts);
		
		Refresh();
		
		// create default text format
		_defaultFormat = _writeFactory.CreateTextFormat("Arial", _fontCollection, FontWeight.Normal, 
			FontStyle.Normal, FontStretch.Normal, 16f, "en-US");
	}

	public void AddFont(string path)
	{
		var fontFile = _writeFactory.CreateFontFileReference(Path.GetFullPath(path));
		_fontSetBuilder.AddFontFile(fontFile);
	}

	public void Refresh()
	{
		var fontSet = _fontSetBuilder.CreateFontSet();
		_fontCollection = _writeFactory.CreateFontCollectionFromFontSet(fontSet);
		
		// recreate format to use new font collection
		_textLayoutCache.Clear(); // layouts depend on format
		_defaultFormat = _writeFactory.CreateTextFormat("Arial", _fontCollection, FontWeight.Normal, 
			FontStyle.Normal, FontStretch.Normal, 16f, "en-US");
	}

	public void CleanupCache()
	{
		if (_cacheLoad > 0f)
		{
			_cacheLoad -= 0.1f;
		}

		if (_textLayoutCache.Count > _maxCacheSize)
		{
			var layoutsToRemove = _textLayoutCache.Count / 5;
			var keys = _textLayoutCache.Keys.Take(layoutsToRemove).ToArray();
			foreach (var key in keys)
			{
				_textLayoutCache[key].Dispose();
				_textLayoutCache.Remove(key);
			}
			
			_cacheLoad += 1f;
		}
		
		if (_cacheLoad > CACHE_THRESHOLD)
		{
			_maxCacheSize += 10;
			_cacheLoad = 0f;
		}
	}
	
	public bool FamilyExists(string familyName) => _fontCollection.FindFamilyName(familyName, out _);

	public IDWriteTextLayout GetLayout(string text, string fontFamily, float size, int weight, bool italic, Vector2 maxSize)
	{
		var everything = new TextRange(0, (uint)text.Length);
		
		if (_textLayoutCache.TryGetValue(text.GetHashCode(), out var layout))
		{
			layout.SetFontSize(size, everything);
			layout.SetFontWeight((FontWeight)weight, everything);
			layout.SetFontFamilyName(fontFamily, everything);
			layout.SetFontStyle(italic ? FontStyle.Italic : FontStyle.Normal, everything);
			layout.MaxWidth = maxSize.X;
			layout.MaxHeight = maxSize.Y;
			
			return layout;
		}
		
		layout = _writeFactory.CreateTextLayout(text, _defaultFormat, maxSize.X, maxSize.Y);
		layout.SetFontSize(size, everything);
		layout.SetFontWeight((FontWeight)weight, everything);
		layout.SetFontFamilyName(fontFamily, everything);
		layout.SetFontStyle(italic ? FontStyle.Italic : FontStyle.Normal, everything);
		
		_textLayoutCache.Add(text.GetHashCode(), layout);
		return _textLayoutCache[text.GetHashCode()];
	}
	
	// dispose pattern
	private bool _disposed;
	
	private void Dispose(bool disposing)
	{
		if (_disposed) return;
		
		if (disposing)
		{
			_defaultFormat.Dispose();
			_fontCollection.Dispose();
			_fontSetBuilder.Dispose();
			_writeFactory.Dispose();
			
			foreach (var layout in _textLayoutCache.Values)
			{
				layout.Dispose();
			}
		}
		
		_disposed = true;
	}
	
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	
	~FontCache()
	{
		Dispose(false);
	}
}