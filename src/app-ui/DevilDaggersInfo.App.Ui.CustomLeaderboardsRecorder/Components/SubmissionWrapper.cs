using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class SubmissionWrapper : AbstractComponent
{
	public SubmissionWrapper(Rectangle metric)
		: base(metric)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, Metric.TopLeft + parentPosition, 0, Color.Purple);
	}
}
