using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.State;
using System.Security.Cryptography;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, IExtendedLayout
{
	private readonly StateWrapper _stateWrapper;
	private readonly RecordingWrapper _recordingWrapper;

	private int _recordingInterval;

	public CustomLeaderboardsRecorderMainLayout()
	{
		const int headerHeight = 24;
		MainLayoutBackButton backButton = new(new PixelBounds(0, 0, 24, headerHeight), () => StateManager.Dispatch(new SetLayout(Root.Game.MainLayout)));
		_stateWrapper = new(new PixelBounds(0, headerHeight, 256, 96 - headerHeight));
		_recordingWrapper = new(new PixelBounds(0, 96, 256, 416));
		LeaderboardListWrapper leaderboardListWrapper = new(new PixelBounds(256, headerHeight, 768, 512 - headerHeight));
		LeaderboardWrapper leaderboardWrapper = new(new PixelBounds(0, 512, 1024, 256));

		NestingContext.Add(backButton);
		NestingContext.Add(_stateWrapper);
		NestingContext.Add(_recordingWrapper);
		NestingContext.Add(leaderboardListWrapper);
		NestingContext.Add(leaderboardWrapper);

		StateManager.Subscribe<SetLayout>(Initialize);
	}

	private static void Initialize(SetLayout setLayout)
	{
		if (setLayout.Layout != Root.Game.CustomLeaderboardsRecorderMainLayout)
			return;

		if (!File.Exists(UserSettings.ModsSurvivalPath))
		{
			StateManager.Dispatch(new SetActiveSpawnset(null));
		}
		else
		{
			byte[] fileContents = File.ReadAllBytes(UserSettings.ModsSurvivalPath);
			byte[] fileHash = MD5.HashData(fileContents);
			AsyncHandler.Run(s => StateManager.Dispatch(new SetActiveSpawnset(s?.Name)), () => FetchSpawnsetByHash.HandleAsync(fileHash));
		}

		StateManager.Dispatch(new LoadLeaderboardList());
	}

	public void Update()
	{
		_recordingInterval++;
		if (_recordingInterval < 5)
			return;

		_recordingInterval = 0;
		if (!RecordingLogic.Scan())
			return;

		_stateWrapper.SetState();
		_recordingWrapper.SetState();

		RecordingLogic.Handle();
	}

	public void Render3d()
	{
	}

	public void Render()
	{
	}
}
