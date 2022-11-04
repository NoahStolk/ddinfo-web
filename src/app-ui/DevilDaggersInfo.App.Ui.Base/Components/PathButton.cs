using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class PathButton : TextButton
{
	public PathButton(Rectangle metric, Action onClick, bool isDirectory, string text)
		: base(metric, onClick, GlobalStyles.DefaultButtonStyle, isDirectory ? GlobalStyles.PathDirectoryButton : GlobalStyles.PathFileButton, text)
	{
	}
}
