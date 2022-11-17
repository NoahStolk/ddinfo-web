using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ShrinkSlider : Slider
{
	public ShrinkSlider(IBounds bounds, Action<float> onChange, bool applyInstantly, float min, float max, float step, float defaultValue, int border, Color textColor)
		: base(bounds, onChange, applyInstantly, min, max, step, defaultValue, border, textColor)
	{
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> topLeft = new(Bounds.X1, Bounds.Y1);
		NewHighlighter(CurrentValue / Max + Min, Color.Yellow);
		NewHighlighter(StateManager.SpawnsetState.Spawnset.GetShrinkEndTime() / Max + Min, Color.Aqua);

		void NewHighlighter(float percentage, Color color)
		{
			const int width = 4;
			int height = Bounds.Size.Y - Border;
			int position = (int)(percentage * (Bounds.Size.X - Border * 2 - width / 2));
			Vector2i<int> origin = parentPosition + topLeft;
			RenderBatchCollector.RenderRectangleTopLeft(new(width, height), origin + new Vector2i<int>(position + Border / 2, Border / 2), Depth + 2, color);
		}
	}
}
