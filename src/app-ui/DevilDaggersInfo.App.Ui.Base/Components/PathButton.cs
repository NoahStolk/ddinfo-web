using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.Rendering.Text;
using DevilDaggersInfo.App.Ui.Base.Styling;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class PathButton : TextButton
{
	private static readonly TextButtonStyle _pathDirectoryButton = new(Color.Yellow, TextAlign.Left, FontSize.H12);
	private static readonly TextButtonStyle _pathFileButton = new(Color.White, TextAlign.Left, FontSize.H12);

	public PathButton(IBounds bounds, Action onClick, bool isDirectory, string text)
		: base(bounds, onClick, ButtonStyles.Default, isDirectory ? _pathDirectoryButton : _pathFileButton, text)
	{
	}
}
