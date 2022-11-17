using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class MainLayoutBackButton : IconButton
{
	public MainLayoutBackButton(IBounds bounds, Action onClick)
		: base(bounds, onClick, GlobalStyles.DefaultButtonStyle, "Back", Textures.BackButton)
	{
	}
}
