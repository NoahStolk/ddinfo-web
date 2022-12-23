using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class MainLayoutBackButton : TooltipIconButton
{
	public MainLayoutBackButton(IBounds bounds, Action onClick)
		: base(bounds, onClick, GlobalStyles.DefaultButtonStyle, Textures.BackButton, "Back", Color.HalfTransparentWhite, Color.White)
	{
	}
}
