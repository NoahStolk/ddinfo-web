using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;

namespace DevilDaggersInfo.App.Ui.Base.Styling;

public static class LabelStyles
{
	public static LabelStyle Popup { get; } = new(Color.White, TextAlign.Middle, FontSize.H16);

	public static LabelStyle DefaultLeft { get; } = new(Color.White, TextAlign.Left, FontSize.H12);
	public static LabelStyle DefaultMiddle { get; } = new(Color.White, TextAlign.Middle, FontSize.H12);
	public static LabelStyle DefaultRight { get; } = new(Color.White, TextAlign.Right, FontSize.H12);

	public static LabelStyle Title { get; } = new(Color.White, TextAlign.Middle, FontSize.H24);
}
