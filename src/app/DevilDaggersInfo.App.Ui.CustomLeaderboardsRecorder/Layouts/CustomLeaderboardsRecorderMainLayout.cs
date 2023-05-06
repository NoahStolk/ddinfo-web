using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Leaderboard;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording.Results;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.State;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, IExtendedLayout
{
	private const int _headerHeight = 24;

	private readonly RecordingResultScrollArea _recordingResultScrollArea;

	public CustomLeaderboardsRecorderMainLayout()
	{
		const int stateWrapperBottom = 96;
		const int recordingScrollAreaBottom = 444;

		_recordingResultScrollArea = new(new PixelBounds(0, stateWrapperBottom, 256, recordingScrollAreaBottom - stateWrapperBottom));
		LeaderboardListWrapper leaderboardListWrapper = new(new PixelBounds(256, _headerHeight, 768, recordingScrollAreaBottom - _headerHeight));
		LeaderboardWrapper leaderboardWrapper = new(new PixelBounds(0, recordingScrollAreaBottom, 1024, 768 - recordingScrollAreaBottom));

		NestingContext.Add(_recordingResultScrollArea);
		NestingContext.Add(leaderboardListWrapper);
		NestingContext.Add(leaderboardWrapper);

		StateManager.Subscribe<SetLayout>(Initialize);
	}

	private void Initialize()
	{
		StateManager.Dispatch(new LoadLeaderboardList());
	}
}
