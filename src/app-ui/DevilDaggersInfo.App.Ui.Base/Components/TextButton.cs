using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.Common.Exceptions;
using Warp.Numerics;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class TextButton : Button
{
	public TextButton(Rectangle metric, Action onClick, ButtonStyle buttonStyle, TextButtonStyle textButtonStyle, string text)
		: base(metric, onClick, buttonStyle)
	{
		TextButtonStyle = textButtonStyle;
		Text = text;
	}

	public string Text { get; set; }
	public TextButtonStyle TextButtonStyle { get; set; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		if (Text.Length == 0)
			return;

		int padding = (int)MathF.Round((Metric.Y2 - Metric.Y1) / 4f);
		Vector2i<int> textPosition = TextButtonStyle.TextAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Metric.X1 + Metric.X2, Metric.Y1 + Metric.Y2) / 2,
			TextAlign.Left => new(Metric.X1 + padding, Metric.Y1 + padding),
			TextAlign.Right => new(Metric.X2 - padding, Metric.Y1 + padding),
			_ => throw new InvalidEnumConversionException(TextButtonStyle.TextAlign),
		};

		RenderBatchCollector.RenderMonoSpaceText(TextButtonStyle.FontSize, Vector2i<int>.One, parentPosition + textPosition, Depth + 2, TextButtonStyle.TextColor, Text, TextButtonStyle.TextAlign);
	}
}
