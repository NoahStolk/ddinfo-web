using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.Rendering.Text;
using Warp.NET.Text;

namespace DevilDaggersInfo.App.Ui.Base.Styling;

public static class TextButtonStyles
{
	public static TextButtonStyle DefaultMiddle { get; } = new(Color.White, TextAlign.Middle, FontSize.H12);

	public static TextButtonStyle Popup { get; } = new(Color.White, TextAlign.Middle, FontSize.H16);
}
