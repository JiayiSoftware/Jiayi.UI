namespace Jiayi.UI.Core;

// meant to mirror win32 virtual key codes... so some of these are not used
public enum KeyCode : byte
{
	None = 0x00,
	LeftButton,
	RightButton,
	Cancel,
	MiddleButton,
	SideButton1, // or page up
	SideButton2, // or page down
	
	// 0x07 is reserved
	
	Back = 0x08,
	Tab,
	
	// 0x0A-0x0B are reserved
	
	Clear = 0x0C,
	Return,
	
	// 0x0E-0x0F are unassigned
	
	Shift = 0x10,
	Control,
	Alt, // or menu
	Pause,
	CapsLock, // or capital
	Kana, // or hangul
	ImeOn,
	Junja,
	ImeFinal,
	Kanji, // or hanja
	ImeOff,
	Escape,
	ImeConvert,
	ImeNonConvert,
	ImeAccept,
	ImeModeChange,
	Space,
	PageUp, // or prior
	PageDown, // or next
	End,
	Home,
	LeftArrow,
	UpArrow,
	RightArrow,
	DownArrow,
	Select,
	Print,
	Execute,
	PrintScreen, // or snapshot
	Insert,
	Delete,
	Help,
	
	// alphanumeric keys
	Alpha0 = 0x30,
	Alpha1,
	Alpha2,
	Alpha3,
	Alpha4,
	Alpha5,
	Alpha6,
	Alpha7,
	Alpha8,
	Alpha9,
	
	// 0x3A-0x40 are undefined
	
	A = 0x41,
	B,
	C,
	D,
	E,
	F,
	G,
	H,
	I,
	J,
	K,
	L,
	M,
	N,
	O,
	P,
	Q,
	R,
	S,
	T,
	U,
	V,
	W,
	X,
	Y,
	Z,
	Windows, // left windows key (the only one that matters)
	
	// basically... skip the right windows key
	
	Application = 0x5D, // or context menu
	
	// 0x5E is reserved
	
	Sleep = 0x5F,
	
	// numpad keys
	Numpad0 = 0x60,
	Numpad1,
	Numpad2,
	Numpad3,
	Numpad4,
	Numpad5,
	Numpad6,
	Numpad7,
	Numpad8,
	Numpad9,
	Multiply,
	Add,
	Separator,
	Subtract,
	Decimal, // or period, although it's not really a period
	Divide,
	
	// function keys
	F1 = 0x70,
	F2,
	F3,
	F4,
	F5,
	F6,
	F7,
	F8,
	F9,
	F10,
	F11,
	F12,
	
	// now the rest of the function keys aren't on a standard keyboard, but they're here anyway
	F13,
	F14,
	F15,
	F16,
	F17,
	F18,
	F19,
	F20,
	F21,
	F22,
	F23,
	F24,
	
	// 0x88-0x8F are reserved
	
	NumLock = 0x90,
	ScrollLock,
	
	// 0x92-0x96 are OEM specific, and 0x97-0x9F are unassigned
	
	LeftShift = 0xA0,
	RightShift,
	LeftControl,
	RightControl,
	LeftAlt, // or left menu
	RightAlt, // or right menu
	
	// browser keys idk if i've ever seen these
	BrowserBack,
	BrowserForward,
	BrowserRefresh,
	BrowserStop,
	BrowserSearch,
	BrowserFavorites,
	BrowserHome,
	
	// media keys
	VolumeMute,
	VolumeDown,
	VolumeUp,
	MediaNext,
	MediaPrevious,
	MediaStop,
	MediaPlay,
	
	LaunchMail,
	LaunchMediaSelect,
	LaunchApp1,
	LaunchApp2,
	
	// 0xB8-0xB9 are reserved
	
	Oem1 = 0xBA, // semicolon key for US keyboards
	OemPlus, // plus key for any region
	OemComma, // comma key for any region
	OemMinus, // minus key for any region
	OemPeriod, // period key for any region
	Oem2, // slash key for US keyboards
	Oem3, // tilde key for US keyboards
	
	// 0xC1-0xDA are reserved
	
	Oem4 = 0xDB, // left bracket key for US keyboards
	Oem5, // backslash key for US keyboards
	Oem6, // right bracket key for US keyboards
	Oem7, // single quote key for US keyboards
	Oem8, // this could literally be anything
	
	// 0xE0 is reserved and 0xE1 is OEM specific
	
	Oem102 = 0xE2, // backslash key for any region that isn't the US; for US keyboards, it's the angle bracket key
	
	// 0xE3 and 0xE4 are OEM specific
	
	ImeProcess = 0xE5,
	
	// 0xE6 is OEM specific
	
	Packet = 0xE7,
	
	// 0xE8 is unassigned and 0xE9-0xF5 are OEM specific
	
	Attention = 0xF6, // attn key
	CrSel,
	ExSel,
	EraseEof,
	Play,
	Zoom,
	
	// 0xFC technically has a key code, but it's reserved
	
	Pa1 = 0xFD,
	OemClear
}