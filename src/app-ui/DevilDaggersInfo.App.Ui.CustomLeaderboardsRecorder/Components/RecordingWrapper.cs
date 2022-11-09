using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class RecordingWrapper : AbstractComponent
{
	public RecordingWrapper(Rectangle metric)
		: base(metric)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 1;
		RenderBatchCollector.RenderRectangleTopLeft(Bounds.Size, Bounds.TopLeft + parentPosition, 0, Color.Purple);
		RenderBatchCollector.RenderRectangleTopLeft(Bounds.Size - new Vector2i<int>(border * 2), Bounds.TopLeft + parentPosition + new Vector2i<int>(border), 1, Color.Black);
	}
}
