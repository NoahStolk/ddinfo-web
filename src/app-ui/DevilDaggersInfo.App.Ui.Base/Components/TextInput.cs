using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.Common.Exceptions;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class TextInput : AbstractTextInput
{
	private const float _cursorTimerSwitch = 0.45f;

	public TextInput(
		Rectangle metric,
		bool isNumeric,
		Action<string>? onEnter,
		Action<string>? onDeselect,
		Action<string>? onInput,
		Color backgroundColor,
		Color borderColor,
		Color hoverBackgroundColor,
		Color textColor,
		Color activeBorderColor,
		Color cursorColor,
		Color selectionColor,
		int borderSize,
		FontSize fontSize,
		int charWidth,
		int textRenderingHorizontalOffset)
		: base(metric, isNumeric, onEnter, onDeselect, onInput)
	{
		BackgroundColor = backgroundColor;
		BorderColor = borderColor;
		HoverBackgroundColor = hoverBackgroundColor;
		TextColor = textColor;
		ActiveBorderColor = activeBorderColor;
		CursorColor = cursorColor;
		SelectionColor = selectionColor;
		BorderSize = borderSize;
		FontSize = fontSize;
		CharWidth = charWidth;
		TextRenderingHorizontalOffset = textRenderingHorizontalOffset;
	}

	public Color BackgroundColor { get; set; }
	public Color BorderColor { get; set; }
	public Color HoverBackgroundColor { get; set; }
	public Color TextColor { get; set; }
	public Color ActiveBorderColor { get; set; }
	public Color CursorColor { get; set; }
	public Color SelectionColor { get; set; }
	public int BorderSize { get; set; }
	public FontSize FontSize { get; set; }

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		int padding = (int)MathF.Round((Metric.Y2 - Metric.Y1) / 4f);
		Vector2i<int> borderVec = new(BorderSize);
		Vector2i<int> scale = new(Metric.X2 - Metric.X1, Metric.Y2 - Metric.Y1);
		Vector2i<int> topLeft = new(Metric.X1, Metric.Y1);

		RenderBatchCollector.RenderRectangleTopLeft(scale, topLeft + parentPosition, Depth, IsSelected ? ActiveBorderColor : BorderColor);
		RenderBatchCollector.RenderRectangleTopLeft(scale - borderVec, topLeft + parentPosition + borderVec / 2, Depth + 1, Hover ? HoverBackgroundColor : BackgroundColor);

		// TODO: FontSize extension method.
		int charWidth = FontSize switch
		{
			FontSize.F4X6 => 4,
			FontSize.F8X8 => 8,
			FontSize.F12X12 => 12,
			_ => throw new InvalidEnumConversionException(FontSize),
		};

		bool hasSelection = GetSelectionLength() > 0;
		if (CursorPositionStart == CursorPositionEnd && CursorTimer <= _cursorTimerSwitch && IsSelected || hasSelection)
		{
			int selectionStart = Math.Min(CursorPositionStart, CursorPositionEnd);
			int cursorSelectionStartX = Metric.X1 + selectionStart * charWidth + padding;
			RenderBatchCollector.RenderRectangleTopLeft(new(GetSelectionLength() * charWidth + 1, Metric.Size.Y - borderVec.Y), GetCursorPosition(cursorSelectionStartX), Depth + 2, hasSelection ? SelectionColor : CursorColor);
		}

		Vector2i<int> GetCursorPosition(int cursorStartX)
		{
			return parentPosition + new Vector2i<int>(cursorStartX, Metric.Y1 + borderVec.Y / 2);
		}

		Vector2i<int> position = new(Metric.X1 + padding, Metric.Y1 + padding);

		RenderBatchCollector.RenderMonoSpaceText(FontSize, Vector2i<int>.One, parentPosition + position, Depth + 3, TextColor, Value, TextAlign.Left);
	}
}
