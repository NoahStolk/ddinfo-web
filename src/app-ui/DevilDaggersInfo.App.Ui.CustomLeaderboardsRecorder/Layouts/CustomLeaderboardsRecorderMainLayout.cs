using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, IExtendedLayout
{
	public CustomLeaderboardsRecorderMainLayout()
		: base(Constants.Full)
	{
		const int headerHeight = 24;
		MainLayoutBackButton backButton = new(Rectangle.At(0, 0, 24, headerHeight), LayoutManager.ToMainLayout);
		StateWrapper stateWrapper = new(Rectangle.At(0, headerHeight, 256, 128 - headerHeight));
		RecordingWrapper recordingWrapper = new(Rectangle.At(0, 128, 256, 384));
		LeaderboardList leaderboardList = new(Rectangle.At(256, headerHeight, 768, 512 - headerHeight));
		LeaderboardWrapper leaderboardWrapper = new(Rectangle.At(0, 512, 1024, 256));

		NestingContext.Add(backButton);
		NestingContext.Add(stateWrapper);
		NestingContext.Add(recordingWrapper);
		NestingContext.Add(leaderboardList);
		NestingContext.Add(leaderboardWrapper);
	}

	public void Update()
	{
	}

	public void Render3d()
	{
	}

	public void Render()
	{
	}
}
