using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Enums;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Slider : AbstractSlider
{
	private readonly Color _textColor;

	public Slider(Rectangle metric, Action<float> onChange, bool applyInstantly, float min, float max, float step, float defaultValue, int border, Color textColor)
		: base(metric, onChange, applyInstantly, min, max, step, defaultValue)
	{
		Border = border;
		_textColor = textColor;
	}

	protected int Border { get; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> borderVec = new(Border);
		Vector2i<int> scale = new(Metric.X2 - Metric.X1, Metric.Y2 - Metric.Y1);
		Vector2i<int> topLeft = new(Metric.X1, Metric.Y1);
		Vector2i<int> center = topLeft + scale / 2;

		Root.Game.UiRenderer.RenderRectangleCenter(scale, parentPosition + center, Depth, Color.White);
		Root.Game.UiRenderer.RenderRectangleCenter(scale - borderVec, parentPosition + center, Depth + 1, Hold ? Color.Gray(0.5f) : Hover ? Color.Gray(0.25f) : Color.Black);
	}

	public override void RenderText(Vector2i<int> parentPosition)
	{
		base.RenderText(parentPosition);

		Vector2i<int> centerPosition = new Vector2i<int>(Metric.X1 + Metric.X2, Metric.Y1 + Metric.Y2) / 2;
		Root.Game.FontRenderer8X8.Render(Vector2i<int>.One, parentPosition + centerPosition, Depth + 3, _textColor, CurrentValue.ToString("0.00"), TextAlign.Middle);
	}
}
