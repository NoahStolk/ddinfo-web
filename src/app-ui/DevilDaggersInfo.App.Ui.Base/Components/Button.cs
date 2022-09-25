using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.Common.Exceptions;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Button : AbstractButton
{
	public Color BackgroundColor { get; set; }
	public Color BorderColor { get; set; }
	public Color HoverBackgroundColor { get; set; }
	public Color TextColor { get; set; }
	public string Text { get; set; }
	public TextAlign TextAlign { get; set; }
	public int BorderSize { get; set; }
	public FontSize FontSize { get; set; }

	public Button(Rectangle metric, Action onClick, Color backgroundColor, Color borderColor, Color hoverBackgroundColor, Color textColor, string text, TextAlign textAlign, int borderSize, FontSize fontSize)
		: base(metric, onClick)
	{
		BackgroundColor = backgroundColor;
		BorderColor = borderColor;
		HoverBackgroundColor = hoverBackgroundColor;
		TextColor = textColor;
		Text = text;
		TextAlign = textAlign;
		BorderSize = borderSize;
		FontSize = fontSize;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> borderVec = new(BorderSize);
		Vector2i<int> scale = Metric.Size;
		Vector2i<int> topLeft = new(Metric.X1, Metric.Y1);
		Vector2i<int> center = topLeft + scale / 2;

		Root.Game.UiRenderer.RenderCenter(scale, parentPosition + center, Depth, BorderColor);
		Root.Game.UiRenderer.RenderCenter(scale - borderVec, parentPosition + center, Depth + 1, Hover ? HoverBackgroundColor : BackgroundColor);
	}

	public override void RenderText(Vector2i<int> parentPosition)
	{
		base.RenderText(parentPosition);

		int padding = (int)MathF.Round((Metric.Y2 - Metric.Y1) / 4f);
		Vector2i<int> textPosition = TextAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Metric.X1 + Metric.X2, Metric.Y1 + Metric.Y2) / 2,
			TextAlign.Left => new(Metric.X1 + padding, Metric.Y1 + padding),
			TextAlign.Right => new(Metric.X2 - padding, Metric.Y1 + padding),
			_ => throw new InvalidOperationException("Invalid text align."),
		};

		// TODO: Move to FontSize setter.
		IMonoSpaceFontRenderer fontRenderer = FontSize switch
		{
			FontSize.F4X6 => Root.Game.FontRenderer4X6,
			FontSize.F8X8 => Root.Game.FontRenderer8X8,
			FontSize.F12X12 => Root.Game.FontRenderer12X12,
			_ => throw new InvalidEnumConversionException(FontSize),
		};
		fontRenderer.Render(Vector2i<int>.One, parentPosition + textPosition, Depth + 2, TextColor, Text, TextAlign);
	}

	// TODO: Move to ComponentBuilder.
	public class MenuButton : Button
	{
		public MenuButton(Rectangle metric, Action onClick, string text)
			: base(metric, onClick, Color.Black, Color.White, Color.Gray(0.75f), Color.White, text, TextAlign.Left, 2, FontSize.F8X8)
		{
			Depth = 102;
			IsActive = false;
		}
	}

	// TODO: Move to ComponentBuilder.
	public class PathButton : Button
	{
		public PathButton(Rectangle metric, Action onClick, string text, Color textColor)
			: base(metric, onClick, Color.Black, Color.Gray(0.7f), Color.Gray(0.4f), textColor, text, TextAlign.Left, 2, FontSize.F8X8)
		{
		}
	}
}
