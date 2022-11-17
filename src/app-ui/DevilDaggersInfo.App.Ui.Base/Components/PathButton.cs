using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class PathButton : TextButton
{
	public PathButton(IBounds bounds, Action onClick, bool isDirectory, string text)
		: base(bounds, onClick, GlobalStyles.DefaultButtonStyle, isDirectory ? GlobalStyles.PathDirectoryButton : GlobalStyles.PathFileButton, text)
	{
	}
}
