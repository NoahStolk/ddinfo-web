using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.CustomLeaderboardsRecorder;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.ReplayViewer;
using DevilDaggersInfo.App.Ui.Scene;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Replay.PostProcessing.PlayerMovement;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class ReplayViewer3dLayout : Layout, IReplayViewer3dLayout
{
	private readonly ShrinkSlider _shrinkSlider;
	private readonly ArenaScene _arenaScene = new();

	private float _currentTime;

	public ReplayViewer3dLayout()
	{
		_shrinkSlider = new(new PixelBounds(0, 752, 1024, 16), f => _currentTime = f, true, 0, 0, 0.001f, 0, GlobalStyles.DefaultSliderStyle, 0);
		NestingContext.Add(_shrinkSlider);
	}

	public void BuildScene(ReplayBinary<LocalReplayBinaryHeader>[] replayBinaries)
	{
		if (replayBinaries.Length == 0)
			throw new InvalidOperationException("Cannot build replay scene without replay binaries.");

		if (!SpawnsetBinary.TryParse(replayBinaries[0].Header.SpawnsetBuffer, out SpawnsetBinary? spawnset))
			throw new InvalidOperationException("Spawnset inside replay is invalid.");

		_currentTime = 0;

		_shrinkSlider.Max = replayBinaries.Max(rb => rb.EventsData.TickCount / 60f);
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);

		_arenaScene.BuildArena(spawnset);

		PlayerMovementTimeline playerMovementTimeline = PlayerMovementTimelineBuilder.Build(replayBinaries[0].EventsData);

		_arenaScene.BuildPlayerMovement(playerMovementTimeline);
	}

	public unsafe void Update()
	{
		_currentTime += Root.Game.Dt;
		_shrinkSlider.CurrentValue = _currentTime;

		_arenaScene.Update(_currentTime);

		if (Input.IsKeyPressed(Keys.Escape))
		{
			Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
			LayoutManager.ToCustomLeaderboardsRecorderMainLayout();
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
