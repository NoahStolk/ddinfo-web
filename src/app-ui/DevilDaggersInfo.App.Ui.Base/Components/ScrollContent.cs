using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public abstract class ScrollContent<TSelf, TParent> : AbstractScrollContent<TSelf, TParent>
	where TSelf : AbstractScrollContent<TSelf, TParent>
	where TParent : AbstractScrollViewer<TParent, TSelf>
{
	protected ScrollContent(Rectangle metric, AbstractScrollViewer<TParent, TSelf> parent)
		: base(metric, parent)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		RenderBatchCollector.SetScissor(Scissor.FromComponent(Metric, parentPosition));

		base.Render(parentPosition);

		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, parentPosition + new Vector2i<int>(Metric.X1, Metric.Y1), Depth, Color.Black);

		RenderBatchCollector.UnsetScissor();
	}
}
