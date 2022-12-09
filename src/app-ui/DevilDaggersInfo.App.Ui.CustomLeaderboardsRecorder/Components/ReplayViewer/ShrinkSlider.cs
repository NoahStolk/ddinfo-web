using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.ReplayViewer;

public class ShrinkSlider : Slider
{
	private readonly float _shrinkEndTime;

	public ShrinkSlider(IBounds bounds, Action<float> onChange, bool applyInstantly, float min, float max, float step, float defaultValue, SliderStyle sliderStyle, float shrinkEndTime)
		: base(bounds, onChange, applyInstantly, min, max, step, defaultValue, sliderStyle)
	{
		_shrinkEndTime = shrinkEndTime;
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Vector2i<int> topLeft = new(Bounds.X1, Bounds.Y1);
		NewHighlighter(CurrentValue / Max + Min, Color.Yellow);
		NewHighlighter(_shrinkEndTime / Max + Min, Color.Aqua);

		void NewHighlighter(float percentage, Color color)
		{
			const int width = 4;
			int height = Bounds.Size.Y - SliderStyle.BorderSize;
			int position = (int)(percentage * (Bounds.Size.X - SliderStyle.BorderSize * 2 - width / 2));
			Vector2i<int> origin = scrollOffset + topLeft;
			Root.Game.RectangleRenderer.Schedule(new(width, height), origin + new Vector2i<int>(position + SliderStyle.BorderSize / 2, SliderStyle.BorderSize / 2) + new Vector2i<int>(width, height) / 2, Depth + 2, color);
		}
	}
}
