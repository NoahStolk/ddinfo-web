using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.State;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, IExtendedLayout
{
	private readonly StateWrapper _stateWrapper;
	private readonly RecordingScrollArea _recordingScrollArea;

	private int _recordingInterval;

	public CustomLeaderboardsRecorderMainLayout()
	{
		const int headerHeight = 24;
		const int stateWrapperBottom = 96;
		const int recordingScrollAreaBottom = 444;

		MainLayoutBackButton backButton = new(new PixelBounds(0, 0, 24, headerHeight), () => StateManager.Dispatch(new SetLayout(Root.Dependencies.MainLayout)));
		_stateWrapper = new(new PixelBounds(0, headerHeight, 256, stateWrapperBottom - headerHeight));
		_recordingScrollArea = new(new PixelBounds(0, stateWrapperBottom, 256, recordingScrollAreaBottom - stateWrapperBottom));
		LeaderboardListWrapper leaderboardListWrapper = new(new PixelBounds(256, headerHeight, 768, recordingScrollAreaBottom - headerHeight));
		LeaderboardWrapper leaderboardWrapper = new(new PixelBounds(0, recordingScrollAreaBottom, 1024, 768 - recordingScrollAreaBottom));

		NestingContext.Add(backButton);
		NestingContext.Add(_stateWrapper);
		NestingContext.Add(_recordingScrollArea);
		NestingContext.Add(leaderboardListWrapper);
		NestingContext.Add(leaderboardWrapper);

		StateManager.Subscribe<SetLayout>(Initialize);
	}

	private void Initialize()
	{
		if (StateManager.LayoutState.CurrentLayout != Root.Dependencies.CustomLeaderboardsRecorderMainLayout)
			return;

		_stateWrapper.Initialize();

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
		_recordingScrollArea.SetState();

		RecordingLogic.Handle();
	}

	public void Render3d()
	{
	}

	public void Render()
	{
	}
}
