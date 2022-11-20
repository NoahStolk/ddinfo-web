using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class MainLayoutBackButton : IconButton
{
	public MainLayoutBackButton(IBounds bounds, Action onClick)
		: base(bounds, onClick, GlobalStyles.DefaultButtonStyle, Textures.BackButton)
	{
		// TODO: Tooltip "Back"
	}
}
