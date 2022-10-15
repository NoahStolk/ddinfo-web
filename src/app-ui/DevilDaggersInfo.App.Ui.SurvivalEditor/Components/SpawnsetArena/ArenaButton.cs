using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Enums;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

// TODO: Move to ComponentBuilder.
public class ArenaButton : Button
{
	private readonly string _tooltipText;

	public ArenaButton(Rectangle metric, Action onClick, Color backgroundColor, Color textColor, string text, FontSize fontSize, string tooltipText)
		: base(metric, onClick, backgroundColor, Color.Black, Color.Blue, textColor, text, TextAlign.Middle, 2, fontSize)
	{
		_tooltipText = tooltipText;
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		if (Hover)
			Root.Game.TooltipText = _tooltipText;
	}
}
