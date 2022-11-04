using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class MenuButton : TextButton
{
	public MenuButton(Rectangle metric, Action onClick, string text)
		: base(metric, onClick, GlobalStyles.DefaultButtonStyle, GlobalStyles.DefaultLeft, text)
	{
		Depth = 102;
		IsActive = false;
	}
}
