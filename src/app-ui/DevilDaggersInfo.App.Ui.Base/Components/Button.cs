using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Button : AbstractButton
{
	public Button(Rectangle metric, Action onClick, Color backgroundColor, Color borderColor, Color hoverBackgroundColor, int borderSize)
		: base(metric, onClick)
	{
		BackgroundColor = backgroundColor;
		BorderColor = borderColor;
		HoverBackgroundColor = hoverBackgroundColor;
		BorderSize = borderSize;
	}

	public Color BackgroundColor { get; set; }
	public Color BorderColor { get; set; }
	public Color HoverBackgroundColor { get; set; }
	public int BorderSize { get; set; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> borderVec = new(BorderSize);
		Vector2i<int> scale = Metric.Size;
		Vector2i<int> topLeft = Metric.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;

		RenderBatchCollector.RenderRectangleCenter(scale, parentPosition + center, Depth, BorderColor);
		RenderBatchCollector.RenderRectangleCenter(scale - borderVec, parentPosition + center, Depth + 1, Hover ? HoverBackgroundColor : BackgroundColor);
	}

	// TODO: Move to ComponentBuilder.
	public class MenuButton : TextButton
	{
		public MenuButton(Rectangle metric, Action onClick, string text)
			: base(metric, onClick, Color.Black, Color.White, Color.Gray(0.75f), Color.White, text, TextAlign.Left, 2, FontSize.F8X8)
		{
			Depth = 102;
			IsActive = false;
		}
	}

	// TODO: Move to ComponentBuilder.
	public class PathButton : TextButton
	{
		public PathButton(Rectangle metric, Action onClick, string text, Color textColor)
			: base(metric, onClick, Color.Black, Color.Gray(0.7f), Color.Gray(0.4f), textColor, text, TextAlign.Left, 2, FontSize.F8X8)
		{
		}
	}
}
