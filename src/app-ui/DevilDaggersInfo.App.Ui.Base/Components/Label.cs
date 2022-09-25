using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.Common.Exceptions;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Label : AbstractLabel
{
	private readonly Color _textColor;
	private readonly TextAlign _textAlign;
	private readonly FontSize _fontSize;

	public Label(Rectangle metric, Color textColor, string text, TextAlign textAlign, FontSize fontSize)
		: base(metric, text)
	{
		_textColor = textColor;
		_textAlign = textAlign;
		_fontSize = fontSize;
	}

	public override void RenderText(Vector2i<int> parentPosition)
	{
		base.RenderText(parentPosition);

		int padding = (int)MathF.Round((Metric.Y2 - Metric.Y1) / 4f);
		Vector2i<int> textPosition = _textAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Metric.X1 + Metric.X2, Metric.Y1 + Metric.Y2) / 2,
			TextAlign.Left => new(Metric.X1 + padding, Metric.Y1 + padding),
			TextAlign.Right => new(Metric.X2 - padding, Metric.Y1 + padding),
			_ => throw new InvalidOperationException("Invalid text align."),
		};

		IMonoSpaceFontRenderer fontRenderer = _fontSize switch
		{
			FontSize.F4X6 => Root.Game.FontRenderer4X6,
			FontSize.F8X8 => Root.Game.FontRenderer8X8,
			FontSize.F12X12 => Root.Game.FontRenderer12X12,
			_ => throw new InvalidEnumConversionException(_fontSize),
		};
		fontRenderer.Render(Vector2i<int>.One, parentPosition + textPosition, Depth + 2, _textColor, Text, _textAlign);
	}
}
