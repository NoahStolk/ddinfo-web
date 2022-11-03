using Warp.Numerics;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class MainLayoutBackButton : IconButton
{
	public MainLayoutBackButton(Rectangle metric, Action onClick, string tooltipText, Texture texture)
		: base(metric, onClick, Color.Black, Color.White, Color.Gray(0.5f), 1, tooltipText, texture)
	{
	}
}
