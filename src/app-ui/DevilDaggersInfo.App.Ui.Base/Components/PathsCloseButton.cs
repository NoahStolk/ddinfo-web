using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class PathsCloseButton : TooltipIconButton
{
	public PathsCloseButton(IBounds bounds, Action onClick)
		: base(bounds, onClick, GlobalStyles.DefaultButtonStyle, Textures.CloseButton, "Close")
	{
	}
}
