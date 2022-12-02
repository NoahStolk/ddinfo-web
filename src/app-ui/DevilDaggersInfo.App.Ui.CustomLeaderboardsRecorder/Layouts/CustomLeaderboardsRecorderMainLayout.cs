using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, IExtendedLayout
{
	public CustomLeaderboardsRecorderMainLayout()
	{
		const int headerHeight = 24;
		MainLayoutBackButton backButton = new(new PixelBounds(0, 0, 24, headerHeight), LayoutManager.ToMainLayout);
		StateWrapper stateWrapper = new(new PixelBounds(0, headerHeight, 256, 128 - headerHeight));
		RecordingWrapper recordingWrapper = new(new PixelBounds(0, 128, 256, 384));
		LeaderboardList leaderboardList = new(new PixelBounds(256, headerHeight, 768, 512 - headerHeight));
		LeaderboardWrapper leaderboardWrapper = new(new PixelBounds(0, 512, 1024, 256));

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
