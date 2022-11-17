using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.NET.Numerics;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Label : AbstractLabel
{
	private readonly Color _textColor;
	private readonly TextAlign _textAlign;
	private readonly FontSize _fontSize;

	public Label(IBounds bounds, Color textColor, string text, TextAlign textAlign, FontSize fontSize)
		: base(bounds, text)
	{
		_textColor = textColor;
		_textAlign = textAlign;
		_fontSize = fontSize;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		int padding = (int)MathF.Round((Bounds.Y2 - Bounds.Y1) / 4f);
		Vector2i<int> textPosition = _textAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2,
			TextAlign.Left => new(Bounds.X1 + padding, Bounds.Y1 + padding),
			TextAlign.Right => new(Bounds.X2 - padding, Bounds.Y1 + padding),
			_ => throw new InvalidOperationException("Invalid text align."),
		};

		RenderBatchCollector.RenderMonoSpaceText(_fontSize, Vector2i<int>.One, parentPosition + textPosition, Depth + 2, _textColor, Text, _textAlign);
	}
}
