using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ShrinkSlider : Slider
{
	public ShrinkSlider(Rectangle metric, Action<float> onChange, bool applyInstantly, float min, float max, float step, float defaultValue, int border, Color textColor)
		: base(metric, onChange, applyInstantly, min, max, step, defaultValue, border, textColor)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> topLeft = new(Metric.X1, Metric.Y1);
		NewHighlighter(CurrentValue / Max + Min, new(1, 1, 0));
		NewHighlighter(StateManager.SpawnsetState.Spawnset.GetShrinkEndTime() / Max + Min, new(0, 1, 1));

		void NewHighlighter(float percentage, Vector3 color)
		{
			const int width = 4;
			int height = Metric.Size.Y - Border;
			int position = (int)(percentage * (Metric.Size.X - Border * 2 - width / 2));
			Vector2i<int> origin = parentPosition + topLeft;
			Root.Game.UiRenderer.RenderRectangleTopLeft(new(width, height), origin + new Vector2i<int>(position + Border / 2, Border / 2), Depth + 2, color);
		}
	}
}
