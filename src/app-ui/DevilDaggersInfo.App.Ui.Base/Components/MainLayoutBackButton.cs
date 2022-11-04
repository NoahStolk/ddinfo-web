using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class MainLayoutBackButton : IconButton
{
	public MainLayoutBackButton(Rectangle metric, Action onClick)
		: base(metric, onClick, GlobalStyles.DefaultButtonStyle, "Back", Textures.BackButton)
	{
	}
}
