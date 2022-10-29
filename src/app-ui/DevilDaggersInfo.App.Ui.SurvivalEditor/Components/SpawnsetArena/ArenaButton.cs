using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Extensions;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaButton : Button
{
	private readonly string _tooltipText;
	private readonly Texture _texture;

	public ArenaButton(Rectangle metric, Action onClick, Color backgroundColor, Color textColor, FontSize fontSize, string tooltipText, Texture texture)
		: base(metric, onClick, backgroundColor, Color.Black, Color.Blue, textColor, string.Empty, TextAlign.Middle, 2, fontSize)
	{
		_tooltipText = tooltipText;
		_texture = texture;
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		if (Hover)
			Root.Game.TooltipText = _tooltipText;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> scale = Metric.Size;
		Vector2i<int> topLeft = Metric.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;
		RenderBatchCollector.RenderSprite(new(16), (parentPosition + center).ToVector2(), Depth + 2, _texture, Color.White);
	}
}
