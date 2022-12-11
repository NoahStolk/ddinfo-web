using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class TooltipIconButton : IconButton
{
	private readonly string _tooltipText;

	public TooltipIconButton(IBounds bounds, Action onClick, ButtonStyle buttonStyle, Texture texture, string tooltipText)
		: base(bounds, onClick, buttonStyle, texture)
	{
		_tooltipText = tooltipText;
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		if (Hover && !IsDisabled)
			Root.Game.TooltipText = _tooltipText;
	}
}
