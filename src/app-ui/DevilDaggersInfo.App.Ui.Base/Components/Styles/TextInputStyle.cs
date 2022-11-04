using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.Common.Exceptions;
using Warp.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.Components.Styles;

public readonly record struct TextInputStyle(Color BackgroundColor, Color BorderColor, Color HoverBackgroundColor, Color TextColor, Color ActiveBorderColor, Color CursorColor, Color SelectionColor, int BorderSize, FontSize FontSize, int TextRenderingHorizontalOffset)
{
	public int CharWidth { get; } = FontSize switch
	{
		FontSize.F4X6 => 4,
		FontSize.F8X8 => 8,
		FontSize.F12X12 => 12,
		_ => throw new InvalidEnumConversionException(FontSize),
	};
}
