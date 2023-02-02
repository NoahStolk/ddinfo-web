using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Slider : AbstractSlider
{
	public Slider(IBounds bounds, Action<float> onChange, bool applyInstantly, float min, float max, float step, float defaultValue, SliderStyle sliderStyle)
		: base(bounds, onChange, applyInstantly, min, max, step, defaultValue)
	{
		SliderStyle = sliderStyle;
	}

	public SliderStyle SliderStyle { get; set; }

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Vector2i<int> borderVec = new(SliderStyle.BorderSize);

		Root.Game.RectangleRenderer.Schedule(Bounds.Size, scrollOffset + Bounds.Center, Depth, Color.White);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - borderVec, scrollOffset + Bounds.Center, Depth + 1, Hold ? Color.Gray(0.5f) : Hover ? Color.Gray(0.25f) : Color.Black);

		Root.Game.GetFontRenderer(SliderStyle.FontSize).Schedule(Vector2i<int>.One, scrollOffset + Bounds.Center, Depth + 3, SliderStyle.TextColor, CurrentValue.ToString("0.00"), SliderStyle.TextAlign);
	}
}
