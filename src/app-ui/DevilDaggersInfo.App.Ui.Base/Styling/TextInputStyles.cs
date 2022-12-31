using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;

namespace DevilDaggersInfo.App.Ui.Base.Styling;

public static class TextInputStyles
{
	public static TextInputStyle Default { get; } = new(Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 2, 4, FontSize.H12);
}