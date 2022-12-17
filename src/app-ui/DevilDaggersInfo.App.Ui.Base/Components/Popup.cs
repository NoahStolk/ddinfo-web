using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Popup : AbstractComponent
{
	public Popup(ILayout parent, string text)
		: base(new NormalizedBounds(0, 0, 1, 1))
	{
		Depth = Constants.DepthMax - 1;

		const int buttonWidth = 128;
		const int buttonHeight = 32;

		Label label = new(new PixelBounds(0, Constants.NativeHeight / 3, Constants.NativeWidth, 32), text, GlobalStyles.PopupLabel)
		{
			Depth = Constants.DepthMax,
		};
		TextButton button = new(new PixelBounds(Constants.NativeWidth / 2 - buttonWidth / 2, Constants.NativeHeight / 2 - buttonHeight / 2, buttonWidth, buttonHeight), () => parent.NestingContext.Remove(this), GlobalStyles.DefaultButtonStyle, GlobalStyles.Popup, "OK")
		{
			Depth = Constants.DepthMax,
		};
		NestingContext.Add(label);
		NestingContext.Add(button);
	}

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		// Cancel other mouse hovers.
		_ = MouseUiContext.Contains(scrollOffset, Bounds);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Root.Game.RectangleRenderer.Schedule(Bounds.Size, scrollOffset + Bounds.Center, Depth, new(0, 0, 0, 95));
	}
}
