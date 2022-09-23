using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base;

// TODO: Move to Warp.Ui.
public abstract class ScrollViewer<TSelf, TContent> : AbstractComponent
	where TSelf : ScrollViewer<TSelf, TContent>
	where TContent : ScrollContent<TContent, TSelf>
{
	protected ScrollViewer(Rectangle metric)
		: base(metric)
	{
	}

	protected abstract Scrollbar Scrollbar { get; }
	protected abstract TContent Content { get; }

	protected void ScrollbarOnChange(float percentage)
	{
		Content.SetScrollOffset(new(0, (int)MathF.Round(percentage * -Content.ContentHeightInPixels)));
	}

	public virtual void InitializeContent()
	{
		Scrollbar.ThumbPercentageSize = Content.ContentHeightInPixels == 0 ? 0 : Math.Clamp(Content.Metric.Size.Y / (float)Content.ContentHeightInPixels, 0, 1);
		Scrollbar.TopPercentage = 0;
	}

	public void SetScroll(int relativeScrollPixels)
	{
		Content.SetScrollOffset(Content.NestingContext.ScrollOffset + new Vector2i<int>(0, relativeScrollPixels));
		float topPercentage = -Content.NestingContext.ScrollOffset.Y / (float)Content.ContentHeightInPixels;
		Scrollbar.TopPercentage = topPercentage;
	}
}
