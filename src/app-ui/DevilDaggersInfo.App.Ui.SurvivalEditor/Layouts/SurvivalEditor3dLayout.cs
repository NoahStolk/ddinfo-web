using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using DevilDaggersInfo.App.Ui.Scene;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Ui;
using StateManager = DevilDaggersInfo.App.Ui.Base.StateManagement.StateManager;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditor3dLayout : Layout, IExtendedLayout
{
	private readonly ShrinkSlider _shrinkSlider;
	private readonly ArenaScene _arenaScene = new();

	private int _currentTick;

	public SurvivalEditor3dLayout()
	{
		_shrinkSlider = new(new PixelBounds(0, 752, 1024, 16), f => _currentTick = (int)(f * 60), true, 0, 0, 0.1f, 0, GlobalStyles.DefaultSliderStyle);
		NestingContext.Add(_shrinkSlider);

		StateManager.Subscribe<SetLayout>(BuildScene);
	}

	private void BuildScene()
	{
		if (StateManager.LayoutState.CurrentLayout != Root.Game.SurvivalEditor3dLayout)
			return;

		_currentTick = 0;

		_shrinkSlider.Max = StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);

		_arenaScene.BuildArena(StateManager.SpawnsetState.Spawnset);
	}

	public unsafe void Update()
	{
		_currentTick++;
		_shrinkSlider.CurrentValue = _currentTick / 60f;

		_arenaScene.Update(_currentTick);

		if (Input.IsKeyPressed(Keys.Escape))
		{
			Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
			StateManager.Dispatch(new SetLayout(Root.Game.SurvivalEditorMainLayout));
		}
	}

	public void Render3d()
	{
		_arenaScene.Render();
	}

	public void Render()
	{
	}
}
