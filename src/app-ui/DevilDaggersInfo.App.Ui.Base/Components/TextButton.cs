using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class TextButton : Button
{
	public TextButton(Rectangle metric, Action onClick, Color backgroundColor, Color borderColor, Color hoverBackgroundColor, Color textColor, string text, TextAlign textAlign, int borderSize, FontSize fontSize)
		: base(metric, onClick, backgroundColor, borderColor, hoverBackgroundColor, borderSize)
	{
		TextColor = textColor;
		Text = text;
		TextAlign = textAlign;
		FontSize = fontSize;
	}

	public Color TextColor { get; set; }
	public string Text { get; set; }
	public TextAlign TextAlign { get; set; }
	public FontSize FontSize { get; set; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		if (Text.Length == 0)
			return;

		int padding = (int)MathF.Round((Metric.Y2 - Metric.Y1) / 4f);
		Vector2i<int> textPosition = TextAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Metric.X1 + Metric.X2, Metric.Y1 + Metric.Y2) / 2,
			TextAlign.Left => new(Metric.X1 + padding, Metric.Y1 + padding),
			TextAlign.Right => new(Metric.X2 - padding, Metric.Y1 + padding),
			_ => throw new InvalidOperationException("Invalid text align."),
		};

		RenderBatchCollector.RenderMonoSpaceText(FontSize, Vector2i<int>.One, parentPosition + textPosition, Depth + 2, TextColor, Text, TextAlign);
	}
}
