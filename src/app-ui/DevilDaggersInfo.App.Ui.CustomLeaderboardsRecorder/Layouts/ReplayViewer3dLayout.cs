using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.ReplayViewer;
using DevilDaggersInfo.App.Ui.Scene;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class ReplayViewer3dLayout : Layout, IExtendedLayout
{
	private readonly ShrinkSlider _shrinkSlider;
	private readonly ArenaScene _arenaScene = new();

	private int _currentTick;

	public ReplayViewer3dLayout()
	{
		_shrinkSlider = new(new PixelBounds(0, 752, 1024, 16), f => _currentTick = (int)(f * 60), true, 0, 0, 0.001f, 0, GlobalStyles.DefaultSliderStyle, 0);
		NestingContext.Add(_shrinkSlider);

		StateManager.Subscribe<BuildReplayScene>(BuildScene);
	}

	private void BuildScene(BuildReplayScene buildReplayScene)
	{
		if (buildReplayScene.ReplayBinaries.Length == 0)
			throw new InvalidOperationException("Cannot build replay scene without replay binaries.");

		if (!SpawnsetBinary.TryParse(buildReplayScene.ReplayBinaries[0].Header.SpawnsetBuffer, out SpawnsetBinary? spawnset))
			throw new InvalidOperationException("Spawnset inside replay is invalid.");

		_currentTick = 0;

		_shrinkSlider.Max = buildReplayScene.ReplayBinaries.Max(rb => rb.EventsData.TickCount / 60f);
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);

		_arenaScene.BuildArena(spawnset);

		ReplaySimulation replaySimulation = ReplaySimulationBuilder.Build(spawnset, buildReplayScene.ReplayBinaries[0]);

		_arenaScene.BuildPlayerMovement(replaySimulation);
	}

	public unsafe void Update()
	{
		_currentTick++;
		_shrinkSlider.CurrentValue = _currentTick / 60f;

		_arenaScene.Update(_currentTick);

		if (Input.IsKeyPressed(Keys.Escape))
		{
			Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
			StateManager.Dispatch(new SetLayout(Root.Game.CustomLeaderboardsRecorderMainLayout));
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
