using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaWrapper : AbstractComponent
{
	private readonly ShrinkSlider _shrinkSlider;
	private readonly Arena _arena;

	public ArenaWrapper(Rectangle metric)
		: base(metric)
	{
		const int titleHeight = 48;

		_arena = new(new(0, titleHeight), 6);

		Label title = new(Rectangle.At(0, 0, _arena.Metric.Size.X, titleHeight), Color.White, "Arena", TextAlign.Middle, FontSize.F12X12);
		ArenaHeightButtons arenaHeightButtons = new(Rectangle.At(_arena.Metric.Size.X + 8, titleHeight, 80, 320));
		_shrinkSlider = new(Rectangle.At(0, _arena.Metric.TopLeft.Y + _arena.Metric.Size.Y + 8, _arena.Metric.Size.X, 16), _arena.SetShrinkCurrent, true, 0, StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds(), 0.001f, 0, 2, Color.White);

		NestingContext.Add(title);
		NestingContext.Add(_arena);
		NestingContext.Add(arenaHeightButtons);
		NestingContext.Add(_shrinkSlider);

		Button button3d = new(Rectangle.At(0, 0, 64, 16), LayoutManager.ToSurvivalEditor3dLayout, Color.Black, Color.White, Color.Aqua, Color.White, "3D", TextAlign.Middle, 2, FontSize.F12X12);
		NestingContext.Add(button3d);
	}

	public void SetSpawnset()
	{
		_shrinkSlider.Max = StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);
		_arena.SetShrinkCurrent(_shrinkSlider.CurrentValue);
	}
}
