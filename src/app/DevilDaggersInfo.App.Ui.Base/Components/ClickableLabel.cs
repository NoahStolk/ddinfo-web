using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Silk.NET.GLFW;
using System.Diagnostics;
using Warp.NET;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class ClickableLabel : AbstractComponent
{
	private readonly string _text;
	private readonly Action _onClick;
	private readonly ClickableLabelStyle _clickableLabelStyle;

	private bool _hover;

	public ClickableLabel(IBounds bounds, string text, Action onClick, ClickableLabelStyle clickableLabelStyle)
		: base(bounds)
	{
		_text = text;
		_onClick = onClick;
		_clickableLabelStyle = clickableLabelStyle;
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		_hover = MouseUiContext.Contains(scrollOffset, Bounds);
		if (!_hover || !Input.IsButtonPressed(MouseButton.Left))
			return;

		_onClick();
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		if (_text.Length == 0)
			return;

		// TODO: Fix padding for hyperlinks.
		int padding = (int)MathF.Round((Bounds.Y2 - Bounds.Y1) / 4f);
		Vector2i<int> textPosition = _clickableLabelStyle.TextAlign switch
		{
			TextAlign.Middle => new Vector2i<int>(Bounds.X1 + Bounds.X2, Bounds.Y1 + Bounds.Y2) / 2,
			TextAlign.Left => new(Bounds.X1 + padding, Bounds.Y1 + padding),
			TextAlign.Right => new(Bounds.X2 - padding, Bounds.Y1 + padding),
			_ => throw new InvalidOperationException("Invalid text align."),
		};

		Color textColor = _hover ? _clickableLabelStyle.HoverTextColor : _clickableLabelStyle.TextColor;
		Root.Game.GetFontRenderer(_clickableLabelStyle.FontSize).Schedule(Vector2i<int>.One, scrollOffset + textPosition, Depth, textColor, _text, _clickableLabelStyle.TextAlign);
	}
}
