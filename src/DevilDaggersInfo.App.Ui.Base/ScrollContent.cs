using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base;

// TODO: Move to Warp.Ui.
public abstract class ScrollContent : AbstractComponent
{
	protected ScrollContent(Rectangle metric)
		: base(metric)
	{
	}

	public abstract int ContentHeightInPixels { get; }

	public abstract void SetScrollOffset(Vector2i<int> scrollOffset);
}
