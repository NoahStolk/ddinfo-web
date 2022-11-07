using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Button : AbstractButton
{
	public Button(Rectangle metric, Action onClick, ButtonStyle buttonStyle)
		: base(metric, onClick)
	{
		ButtonStyle = buttonStyle;
	}

	public ButtonStyle ButtonStyle { get; set; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> borderVec = new(ButtonStyle.BorderSize);
		Vector2i<int> scale = Metric.Size;
		Vector2i<int> topLeft = Metric.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;

		RenderBatchCollector.RenderRectangleCenter(scale, parentPosition + center, Depth, ButtonStyle.BorderColor);
		RenderBatchCollector.RenderRectangleCenter(scale - borderVec * 2, parentPosition + center, Depth + 1, Hover && !IsDisabled ? ButtonStyle.HoverBackgroundColor : ButtonStyle.BackgroundColor);
	}
}
