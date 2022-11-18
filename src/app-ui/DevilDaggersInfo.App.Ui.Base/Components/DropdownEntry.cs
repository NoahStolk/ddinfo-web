using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.Common.Exceptions;
using Warp.NET.Numerics;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

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

		Vector2i<int> borderVec = new(1);
		Vector2i<int> scale = Bounds.Size;
		Vector2i<int> topLeft = Bounds.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;
		RenderBatchCollector.RenderRectangleCenter(Bounds.Size, parentPosition + center, Depth, Color.White);
		RenderBatchCollector.RenderRectangleCenter(Bounds.Size - borderVec * 2, parentPosition + center, Depth + 1, Hover && !IsDisabled ? Color.Gray(0.5f) : Color.Black);

		int padding = (int)MathF.Round(Bounds.Size.Y / 4f);
		Vector2i<int> textPosition = _textAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2,
			TextAlign.Left => new(Bounds.X1 + padding, Bounds.Y1 + padding),
			TextAlign.Right => new(Bounds.X2 - padding, Bounds.Y1 + padding),
			_ => throw new InvalidEnumConversionException(_textAlign),
		};
		RenderBatchCollector.RenderMonoSpaceText(FontSize.F8X8, new(1), parentPosition + textPosition, Depth + 2, Color.White, _text, _textAlign);
	}
}
