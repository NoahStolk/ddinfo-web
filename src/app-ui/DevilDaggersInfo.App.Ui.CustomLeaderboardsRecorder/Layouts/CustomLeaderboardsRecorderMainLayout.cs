using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;
using Warp.Numerics;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, IExtendedLayout
{
	public CustomLeaderboardsRecorderMainLayout()
		: base(Constants.Full)
	{
		const int headerHeight = 24;
		IconButton backButton = new(Rectangle.At(0, 0, 24, headerHeight), LayoutManager.ToMainLayout, Color.Black, Color.Gray(0.5f), Color.Gray(0.25f), 4, "Back", Textures.BackButton);
		ProcessAttachmentWrapper processAttachmentWrapper = new(Rectangle.At(0, headerHeight, 256, 256 - headerHeight));
		RecordingWrapper recordingWrapper = new(Rectangle.At(0, 256, 256, 256));
		SubmissionWrapper submissionWrapper = new(Rectangle.At(0, 512, 256, 256));
		LeaderboardList leaderboardList = new(Rectangle.At(256, headerHeight, 768, 768 - headerHeight));

		NestingContext.Add(backButton);
		NestingContext.Add(processAttachmentWrapper);
		NestingContext.Add(recordingWrapper);
		NestingContext.Add(submissionWrapper);
		NestingContext.Add(leaderboardList);
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
