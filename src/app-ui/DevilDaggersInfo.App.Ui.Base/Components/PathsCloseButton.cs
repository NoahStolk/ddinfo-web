using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class PathsCloseButton : IconButton
{
	public PathsCloseButton(IBounds bounds, Action onClick)
		: base(bounds, onClick, GlobalStyles.DefaultButtonStyle, "Close", Textures.CloseButton)
	{
	}
}
