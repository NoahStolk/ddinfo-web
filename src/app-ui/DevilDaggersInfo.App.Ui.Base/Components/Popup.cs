using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Popup : AbstractComponent
{
	public Popup(ILayout parent, string text)
		: base(Constants.Full)
	{
		Depth = Constants.DepthMax - 1;

		const int buttonWidth = 128;
		const int buttonHeight = 32;

		Label label = new(Rectangle.At(0, Constants.NativeHeight / 3, Constants.NativeWidth, 32), Color.White, text, TextAlign.Middle, FontSize.F12X12)
		{
			Depth = Constants.DepthMax,
		};
		TextButton button = new(Rectangle.At(Constants.NativeWidth / 2 - buttonWidth / 2, Constants.NativeHeight / 2 - buttonHeight / 2, buttonWidth, buttonHeight), () => parent.NestingContext.Remove(this), Color.Black, Color.White, Color.Gray(0.25f), Color.White, "OK", TextAlign.Middle, 2, FontSize.F12X12)
		{
			Depth = Constants.DepthMax,
		};
		NestingContext.Add(label);
		NestingContext.Add(button);
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		// Cancel other mouse hovers.
		_ = MouseUiContext.Contains(parentPosition, Metric);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, parentPosition + Metric.TopLeft, Depth, new(0, 0, 0, 95));
	}
}
