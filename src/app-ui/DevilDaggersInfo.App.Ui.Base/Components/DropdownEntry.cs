using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.Common.Exceptions;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class DropdownEntry : AbstractDropdownEntry
{
	private readonly string _text;
	private readonly TextAlign _textAlign;

	public DropdownEntry(IBounds bounds, AbstractDropdown parent, Action onClick, string text, TextAlign textAlign = TextAlign.Left)
		: base(bounds, parent, onClick)
	{
		_text = text;
		_textAlign = textAlign;
		Depth = 102;
		IsActive = false;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		int padding = (int)MathF.Round(Bounds.Size.Y / 4f);
		Vector2i<int> textPosition = _textAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2,
			TextAlign.Left => new(Bounds.X1 + padding, Bounds.Y1 + padding),
			TextAlign.Right => new(Bounds.X2 - padding, Bounds.Y1 + padding),
			_ => throw new InvalidEnumConversionException(_textAlign),
		};

		RenderBatchCollector.RenderRectangleTopLeft(Bounds.Size, parentPosition + Bounds.TopLeft, Depth, Hover ? Color.Purple : Color.Red);
		RenderBatchCollector.RenderMonoSpaceText(FontSize.F8X8, new(1), parentPosition + textPosition, Depth + 1, Color.White, _text, _textAlign);
	}
}
