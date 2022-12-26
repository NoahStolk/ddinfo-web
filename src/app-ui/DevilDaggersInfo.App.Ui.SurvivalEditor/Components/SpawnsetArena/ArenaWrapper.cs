using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.Base.States.Actions;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;
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
		const int arenaSize = Arena.TileSize * SpawnsetBinary.ArenaDimensionMax;
		const int sliderHeight = 16;

		_arena = new(bounds.CreateNested(0, titleHeight, arenaSize, arenaSize));

		Label title = new(bounds.CreateNested(0, 0, arenaSize, titleHeight), "Arena", GlobalStyles.LabelTitle);
		ArenaHeightButtons arenaHeightButtons = new(bounds.CreateNested(arenaSize + 8, titleHeight, 80, 320));
		_shrinkSlider = new(bounds.CreateNested(0, titleHeight + arenaSize, arenaSize, sliderHeight), _arena.SetShrinkCurrent, true, 0, StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds(), 0.001f, 0, GlobalStyles.DefaultSliderStyle);
		ArenaToolsWrapper arenaToolsWrapper = new(bounds.CreateNested(0, titleHeight + arenaSize + sliderHeight, 304, 480));

		NestingContext.Add(title);
		NestingContext.Add(_arena);
		NestingContext.Add(arenaHeightButtons);
		NestingContext.Add(_shrinkSlider);
		NestingContext.Add(arenaToolsWrapper);

		TextButton button3d = new(bounds.CreateNested(0, 0, 64, 16), () => BaseStateManager.Dispatch(new SetLayout(Root.Game.SurvivalEditor3dLayout)), GlobalStyles.DefaultButtonStyle, GlobalStyles.View3dButton, "3D");
		NestingContext.Add(button3d);
	}

	public void SetSpawnset()
	{
		_shrinkSlider.Max = StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);
		_arena.SetShrinkCurrent(_shrinkSlider.CurrentValue);
	}
}
