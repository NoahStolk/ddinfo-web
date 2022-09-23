using Warp;
using Warp.Extensions;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base;

// TODO: Move to Warp.Ui.
public abstract class ScrollContent<TSelf, TParent> : AbstractComponent
	where TSelf : ScrollContent<TSelf, TParent>
	where TParent : ScrollViewer<TParent, TSelf>
{
	private const int _scrollMultiplier = 64;
	private readonly ScrollViewer<TParent, TSelf> _parent;

	protected ScrollContent(Rectangle metric, ScrollViewer<TParent, TSelf> parent)
		: base(metric)
	{
		_parent = parent;
	}

	public abstract int ContentHeightInPixels { get; }

	public void SetScrollOffset(Vector2i<int> scrollOffset)
	{
		NestingContext.ScrollOffset = Vector2i<int>.Clamp(scrollOffset, new(0, -ContentHeightInPixels + Metric.Size.Y), default);
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		bool hoverWithoutBlock = Metric.Contains(MouseUiContext.MousePosition.RoundToVector2Int32() - parentPosition);
		if (!hoverWithoutBlock)
			return;

		int scroll = Input.GetScroll();
		if (scroll != 0)
			_parent.SetScroll(scroll * _scrollMultiplier);
	}
}
