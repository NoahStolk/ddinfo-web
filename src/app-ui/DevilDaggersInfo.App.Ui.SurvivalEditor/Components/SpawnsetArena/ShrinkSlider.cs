using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ShrinkSlider : Slider
{
	public ShrinkSlider(IBounds bounds, Action<float> onChange, bool applyInstantly, float min, float max, float step, float defaultValue, SliderStyle sliderStyle)
		: base(bounds, onChange, applyInstantly, min, max, step, defaultValue, sliderStyle)
	{
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Vector2i<int> topLeft = new(Bounds.X1, Bounds.Y1);
		NewHighlighter(CurrentValue / Max + Min, Color.Yellow);
		NewHighlighter(StateManager.SpawnsetState.Spawnset.GetShrinkEndTime() / Max + Min, Color.Aqua);

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
