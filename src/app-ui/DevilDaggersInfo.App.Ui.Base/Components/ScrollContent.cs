using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Silk.NET.OpenGL;
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
		Gl.Enable(EnableCap.ScissorTest);
		SetScissor(parentPosition);

		Root.Game.UiRenderer.RenderTopLeft(Metric.Size, parentPosition + new Vector2i<int>(Metric.X1, Metric.Y1), Depth, Color.Black);

		base.Render(parentPosition);

		Gl.Disable(EnableCap.ScissorTest);
	}

	public override void RenderText(Vector2i<int> parentPosition)
	{
		Gl.Enable(EnableCap.ScissorTest);
		SetScissor(parentPosition);

		base.RenderText(parentPosition);

		Gl.Disable(EnableCap.ScissorTest);
	}

	private void SetScissor(Vector2i<int> parentPosition)
	{
		Vector2 viewportOffset = Root.Game.ViewportOffset;
		Vector2i<int> scaledSize = (Metric.Size.ToVector2() * Root.Game.UiScale).RoundToVector2Int32();
		Vector2i<int> scaledTopLeft = (Metric.TopLeft.ToVector2() * Root.Game.UiScale).RoundToVector2Int32();
		Vector2i<int> scaledParentPosition = (parentPosition.ToVector2() * Root.Game.UiScale).RoundToVector2Int32();
		Gl.Scissor(
			scaledTopLeft.X + (int)viewportOffset.X + scaledParentPosition.X,
			scaledTopLeft.Y + (int)viewportOffset.Y + scaledParentPosition.Y,
			(uint)scaledSize.X,
			(uint)scaledSize.Y);
	}
}
