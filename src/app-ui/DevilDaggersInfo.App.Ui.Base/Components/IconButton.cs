using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using System.Numerics;
using Warp.Extensions;
using Warp.Numerics;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class IconButton : Button
{
	private readonly string _tooltipText;
	private readonly Texture _texture;
	private readonly Vector2 _textureSize;

	public IconButton(Rectangle metric, Action onClick, Color backgroundColor, Color borderColor, Color hoverColor, int borderSize, string tooltipText, Texture texture)
		: base(metric, onClick, backgroundColor, borderColor, hoverColor, Color.Black, string.Empty, TextAlign.Middle, borderSize, FontSize.F8X8) // TODO: Remove text related stuff from Button base.
	{
		_tooltipText = tooltipText;
		_texture = texture;
		_textureSize = new(texture.Width, texture.Height);
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		if (Hover)
			Root.Game.TooltipText = _tooltipText;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> scale = Metric.Size;
		Vector2i<int> topLeft = Metric.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;
		RenderBatchCollector.RenderSprite(_textureSize, (parentPosition + center).ToVector2(), Depth + 2, _texture, Color.White);
	}
}