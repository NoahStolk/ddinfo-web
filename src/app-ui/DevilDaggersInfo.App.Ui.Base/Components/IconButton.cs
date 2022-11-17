using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using System.Numerics;
using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class IconButton : Button
{
	private readonly string _tooltipText;
	private readonly Texture _texture;
	private readonly Vector2 _textureSize;

	public IconButton(IBounds bounds, Action onClick, ButtonStyle buttonStyle, string tooltipText, Texture texture)
		: base(bounds, onClick, buttonStyle)
	{
		_tooltipText = tooltipText;
		_texture = texture;
		_textureSize = new(texture.Width, texture.Height);
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		if (Hover && !IsDisabled)
			Root.Game.TooltipText = _tooltipText;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> scale = Bounds.Size;
		Vector2i<int> topLeft = Bounds.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;
		RenderBatchCollector.RenderSprite(_textureSize, (parentPosition + center).ToVector2(), Depth + 2, _texture, IsDisabled ? GlobalColors.HalfTransparentWhite : Color.White);
	}
}
