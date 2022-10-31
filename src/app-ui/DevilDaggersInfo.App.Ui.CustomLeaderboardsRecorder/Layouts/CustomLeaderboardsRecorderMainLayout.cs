using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Layouts;

public class CustomLeaderboardsRecorderMainLayout : Layout, IExtendedLayout
{
	public CustomLeaderboardsRecorderMainLayout()
		: base(Constants.Full)
	{
		Menu menu = new(new(0, 0, 1024, 16));
		ProcessAttachmentWrapper processAttachmentWrapper = new(Rectangle.At(0, 16, 256, 240));
		RecordingWrapper recordingWrapper = new(Rectangle.At(0, 256, 256, 256));
		SubmissionWrapper submissionWrapper = new(Rectangle.At(0, 512, 256, 256));
		LeaderboardList leaderboardList = new(Rectangle.At(256, 16, 768, 752));

		NestingContext.Add(menu);
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
