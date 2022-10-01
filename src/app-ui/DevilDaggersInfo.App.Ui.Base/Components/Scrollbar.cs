using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using System.Numerics;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Scrollbar : AbstractScrollbar
{
	public Scrollbar(Rectangle metric, Action<float> onChange)
		: base(metric, onChange)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 4;

		Vector2i<int> borderVec = new(border);
		Vector2i<int> scale = Metric.Size;
		Vector2i<int> topLeft = new(Metric.X1, Metric.Y1);

		Color thumbColor = Color.Gray(0.75f);
		Root.Game.UiRenderer.RenderRectangleTopLeft(scale, parentPosition + topLeft, Depth, thumbColor);
		Root.Game.UiRenderer.RenderRectangleTopLeft(scale - borderVec, parentPosition + topLeft + new Vector2i<int>(border / 2), Depth + 1, Hold ? Color.Gray(0.5f) : Hover ? Color.Gray(0.25f) : Color.Black);

		const int thumbPadding = 4;
		Vector2i<int> thumbScale = new(scale.X - thumbPadding, (int)MathF.Ceiling(scale.Y * ThumbPercentageSize) - thumbPadding + 1); // + 1 needed for scaled UI for some reason.
		float percentageForRendering = Math.Clamp(TopPercentage, 0, 1 - ThumbPercentageSize);
		Root.Game.UiRenderer.RenderRectangleTopLeft(thumbScale, parentPosition + topLeft + new Vector2i<int>(thumbPadding / 2, (int)MathF.Round(percentageForRendering * scale.Y) + thumbPadding / 2), Depth + 2, thumbColor);
	}
}
