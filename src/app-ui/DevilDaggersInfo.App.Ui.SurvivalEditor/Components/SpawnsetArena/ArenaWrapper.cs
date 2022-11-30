using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaWrapper : AbstractComponent
{
	private readonly ShrinkSlider _shrinkSlider;
	private readonly Arena _arena;

	public ArenaWrapper(IBounds bounds)
		: base(bounds)
	{
		const int titleHeight = 48;

		_arena = new(new(0, titleHeight), 6);

		Label title = new(new PixelBounds(0, 0, _arena.Bounds.Size.X, titleHeight), "Arena", GlobalStyles.LabelDefaultMiddle);
		ArenaHeightButtons arenaHeightButtons = new(new PixelBounds(_arena.Bounds.Size.X + 8, titleHeight, 80, 320));
		_shrinkSlider = new(new PixelBounds(0, titleHeight + _arena.Bounds.Size.Y, _arena.Bounds.Size.X, 16), _arena.SetShrinkCurrent, true, 0, StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds(), 0.001f, 0, GlobalStyles.DefaultSliderStyle);
		ArenaToolsWrapper arenaToolsWrapper = new(new PixelBounds(0, titleHeight + _arena.Bounds.Size.Y + _shrinkSlider.Bounds.Size.Y, 304, 480));

		NestingContext.Add(title);
		NestingContext.Add(_arena);
		NestingContext.Add(arenaHeightButtons);
		NestingContext.Add(_shrinkSlider);
		NestingContext.Add(arenaToolsWrapper);

		TextButton button3d = new(new PixelBounds(0, 0, 64, 16), LayoutManager.ToSurvivalEditor3dLayout, GlobalStyles.DefaultButtonStyle, GlobalStyles.View3dButton, "3D");
		NestingContext.Add(button3d);
	}

	public void SetSpawnset()
	{
		_shrinkSlider.Max = StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);
		_arena.SetShrinkCurrent(_shrinkSlider.CurrentValue);
	}
}
