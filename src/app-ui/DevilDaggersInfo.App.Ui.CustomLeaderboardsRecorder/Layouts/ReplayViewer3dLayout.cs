using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.CustomLeaderboardsRecorder;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.Base.States.Actions;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.ReplayViewer;
using DevilDaggersInfo.App.Ui.Scene;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class ReplayViewer3dLayout : Layout, IReplayViewer3dLayout
{
	private readonly ShrinkSlider _shrinkSlider;
	private readonly ArenaScene _arenaScene = new();

	private int _currentTick;

	public ReplayViewer3dLayout()
	{
		_shrinkSlider = new(new PixelBounds(0, 752, 1024, 16), f => _currentTick = (int)(f * 60), true, 0, 0, 0.001f, 0, GlobalStyles.DefaultSliderStyle, 0);
		NestingContext.Add(_shrinkSlider);
	}

	public void BuildScene(ReplayBinary<LocalReplayBinaryHeader>[] replayBinaries)
	{
		if (replayBinaries.Length == 0)
			throw new InvalidOperationException("Cannot build replay scene without replay binaries.");

		if (!SpawnsetBinary.TryParse(replayBinaries[0].Header.SpawnsetBuffer, out SpawnsetBinary? spawnset))
			throw new InvalidOperationException("Spawnset inside replay is invalid.");

		_currentTick = 0;

		_shrinkSlider.Max = replayBinaries.Max(rb => rb.EventsData.TickCount / 60f);
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);

		_arenaScene.BuildArena(spawnset);

		ReplaySimulation replaySimulation = ReplaySimulationBuilder.Build(spawnset, replayBinaries[0]);

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
			BaseStateManager.Dispatch(new SetLayout(Root.Game.CustomLeaderboardsRecorderMainLayout));
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
