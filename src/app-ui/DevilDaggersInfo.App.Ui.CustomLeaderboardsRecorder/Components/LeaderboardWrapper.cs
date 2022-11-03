using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardWrapper : AbstractComponent
{
	public LeaderboardWrapper(Rectangle metric)
		: base(metric)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 1;
		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, Metric.TopLeft + parentPosition, 0, new(255, 127, 0, 255));
		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size - new Vector2i<int>(border * 2), Metric.TopLeft + parentPosition + new Vector2i<int>(border), 1, Color.Black);
	}
}
