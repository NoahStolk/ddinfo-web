using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class PathsCloseButton : IconButton
{
	public PathsCloseButton(Rectangle metric, Action onClick)
		: base(metric, onClick, GlobalStyles.DefaultButtonStyle, "Close", Textures.CloseButton)
	{
	}
}
