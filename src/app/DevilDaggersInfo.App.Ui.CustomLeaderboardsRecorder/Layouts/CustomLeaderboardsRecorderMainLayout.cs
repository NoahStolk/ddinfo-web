using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording.Results;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.State;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, IExtendedLayout
{
	private const int _headerHeight = 24;

	private readonly StateWrapper _stateWrapper;
	private readonly RecordingScrollArea _recordingScrollArea;
	private readonly RecordingResultScrollArea _recordingResultScrollArea;

	public CustomLeaderboardsRecorderMainLayout()
	{
		const int stateWrapperBottom = 96;
		const int recordingScrollAreaBottom = 444;

		MainLayoutBackButton backButton = new(new PixelBounds(0, 0, 24, _headerHeight), () => StateManager.Dispatch(new SetLayout(Root.Dependencies.MainLayout)));
		_stateWrapper = new(new PixelBounds(0, _headerHeight, 256, stateWrapperBottom - _headerHeight));
		_recordingScrollArea = new(new PixelBounds(0, stateWrapperBottom, 256, recordingScrollAreaBottom - stateWrapperBottom));
		_recordingResultScrollArea = new(new PixelBounds(0, stateWrapperBottom, 256, recordingScrollAreaBottom - stateWrapperBottom));
		LeaderboardListWrapper leaderboardListWrapper = new(new PixelBounds(256, _headerHeight, 768, recordingScrollAreaBottom - _headerHeight));
		LeaderboardWrapper leaderboardWrapper = new(new PixelBounds(0, recordingScrollAreaBottom, 1024, 768 - recordingScrollAreaBottom));

		NestingContext.Add(backButton);
		NestingContext.Add(_stateWrapper);
		NestingContext.Add(_recordingScrollArea);
		NestingContext.Add(_recordingResultScrollArea);
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
}
