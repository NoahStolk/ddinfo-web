using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.ReplayEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using DevilDaggersInfo.App.Ui.Scene;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.ReplayEditor.Layouts;

public class ReplayEditor3dLayout : Layout, IExtendedLayout
{
	private readonly Slider _timeSlider;
	private readonly ArenaScene _arenaScene = new();

	private int _currentTick;
	private int _maxTick;

	public ReplayEditor3dLayout()
	{
		_timeSlider = new(new PixelBounds(0, 752, 1024, 16), f => _currentTick = (int)(f * 60), true, 0, 0, 0.001f, 0, SliderStyles.Default);
		NestingContext.Add(_timeSlider);

		StateManager.Subscribe<BuildReplayScene>(BuildScene);
	}

	private void BuildScene()
	{
		if (!SpawnsetBinary.TryParse(StateManager.ReplayState.Replay.Header.SpawnsetBuffer, out SpawnsetBinary? spawnset))
			throw new InvalidOperationException("Spawnset inside replay is invalid.");

		_currentTick = 0;
		_maxTick = StateManager.ReplayState.Replay.EventsData.TickCount;

		_timeSlider.Max = _maxTick / 60f;
		_timeSlider.CurrentValue = Math.Clamp(_timeSlider.CurrentValue, 0, _timeSlider.Max);

		_arenaScene.BuildSpawnset(spawnset);

		ReplaySimulation replaySimulation = ReplaySimulationBuilder.Build(spawnset, StateManager.ReplayState.Replay);

		_arenaScene.BuildPlayerMovement(replaySimulation);
	}

	public unsafe void Update()
	{
		if (_currentTick > _maxTick)
			_currentTick = 0;

		_currentTick++;
		_timeSlider.CurrentValue = _currentTick / 60f;

		_arenaScene.Update(_currentTick);

		if (Input.IsKeyPressed(Keys.Escape))
		{
			Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
			StateManager.Dispatch(new SetLayout(Root.Dependencies.ReplayEditorMainLayout));
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
