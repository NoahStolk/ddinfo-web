using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Dropdown : AbstractDropdown
{
	private readonly string _text;

	public Dropdown(IBounds bounds, string text)
		: base(bounds)
	{
		_text = text;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> textPosition = new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2;
		RenderBatchCollector.RenderRectangleTopLeft(Bounds.Size, parentPosition + Bounds.TopLeft, Depth, Hover ? Color.Purple : Color.Red);
		RenderBatchCollector.RenderMonoSpaceText(FontSize.F8X8, new(1), parentPosition + textPosition, Depth + 1, Color.White, _text, TextAlign.Middle);
	}
}
