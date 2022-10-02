using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using System.Numerics;
using Warp.Extensions;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;
using static Warp.Graphics;

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
		Vector2 viewportOffset = Root.Game.ViewportOffset;
		Vector2i<int> scaledSize = (Metric.Size.ToVector2() * Root.Game.UiScale).RoundToVector2Int32();
		Vector2i<int> scaledTopLeft = (Metric.TopLeft.ToVector2() * Root.Game.UiScale).RoundToVector2Int32();
		Vector2i<int> scaledParentPosition = (parentPosition.ToVector2() * Root.Game.UiScale).RoundToVector2Int32();
		RenderBatchCollector.SetScissor(new(
			scaledTopLeft.X + (int)viewportOffset.X + scaledParentPosition.X,
			WindowHeight - (scaledSize.Y + scaledParentPosition.Y) - (int)viewportOffset.Y,
			(uint)scaledSize.X,
			(uint)scaledSize.Y));

		base.Render(parentPosition);

		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, parentPosition + new Vector2i<int>(Metric.X1, Metric.Y1), Depth, Color.Black);

		RenderBatchCollector.UnsetScissor();
	}
}
