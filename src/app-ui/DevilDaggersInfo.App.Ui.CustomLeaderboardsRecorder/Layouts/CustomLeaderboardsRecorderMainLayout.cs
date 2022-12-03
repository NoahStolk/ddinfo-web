using DevilDaggersInfo.Api.Ddcl.ProcessMemory;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.CustomLeaderboardsRecorder;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, ICustomLeaderboardsRecorderMainLayout
{
	private readonly StateWrapper _stateWrapper;
	private readonly RecordingWrapper _recordingWrapper;
	private readonly LeaderboardList _leaderboardList;
	private readonly LeaderboardWrapper _leaderboardWrapper;

	private int _recordingInterval;

	public CustomLeaderboardsRecorderMainLayout()
	{
		const int headerHeight = 24;
		MainLayoutBackButton backButton = new(new PixelBounds(0, 0, 24, headerHeight), LayoutManager.ToMainLayout);
		_stateWrapper = new(new PixelBounds(0, headerHeight, 256, 128 - headerHeight));
		_recordingWrapper = new(new PixelBounds(0, 128, 256, 384));
		_leaderboardList = new(new PixelBounds(256, headerHeight, 768, 512 - headerHeight));
		_leaderboardWrapper = new(new PixelBounds(0, 512, 1024, 256));

		NestingContext.Add(backButton);
		NestingContext.Add(_stateWrapper);
		NestingContext.Add(_recordingWrapper);
		NestingContext.Add(_leaderboardList);
		NestingContext.Add(_leaderboardWrapper);
	}

	public void Initialize()
	{
		StateManager.RefreshActiveSpawnset();

		_leaderboardList.Load();
	}

	public void SetCustomLeaderboard()
	{
		_leaderboardWrapper.SetCustomLeaderboard();
	}

	public void Update()
	{
		_recordingInterval++;
		if (_recordingInterval < 5)
			return;

		GameMemoryService service = Root.Game.GameMemoryService;

		if (!StateManager.MarkerState.Marker.HasValue)
		{
			AsyncHandler.Run(SetMarker, () => FetchMarker.HandleAsync(Root.Game.SupportedOperatingSystem));

			void SetMarker(GetMarker? getMarker)
			{
				if (getMarker == null)
				{
					// TODO: Show error.
				}
				else
				{
					StateManager.SetMarker(getMarker.Value);
				}
			}
		}
		else
		{
			// Always initialize the process so we detach properly when the game exits.
			service.Initialize(StateManager.MarkerState.Marker.Value);
		}

		service.Scan();

		_stateWrapper.SetState();
		_recordingWrapper.SetState();

		_recordingInterval = 0;
	}

	public void Render3d()
	{
	}

	public void Render()
	{
	}
}
