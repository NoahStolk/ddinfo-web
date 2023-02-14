using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording.Results;

public class RecordingResultScrollArea : ScrollArea
{
	public RecordingResultScrollArea(IBounds bounds)
		: base(bounds, 64, 16, ScrollAreaStyles.Default)
	{
		StateManager.Subscribe<SetSuccessfulUpload>(ShowResult);
	}

	private void ShowResult()
	{
		foreach (AbstractComponent component in NestingContext.OrderedComponents)
			NestingContext.Remove(component);

		GetUploadResponse? response = StateManager.UploadResponseState.UploadResponse;
		if (response == null)
		{
			Root.Dependencies.Log.Warning("ShowResult was called but state UploadResponse is null.");
			return;
		}

		if (response.NoHighscore != null)
			ShowNoHighscoreResult();
		else if (response.FirstScore != null)
			ShowFirstScoreResult(response.FirstScore);
		else if (response.Highscore != null)
			ShowHighscoreResult(response.Highscore, response.IsAscending);
		else if (response.Rejection != null)
			ShowRejectionResult(response.Rejection);
		else
			Root.Dependencies.NativeDialogService.ReportError("Invalid upload response from server.");
	}

	private void ShowNoHighscoreResult()
	{
		Label header = new(ContentBounds.CreateNested(0, 0, ContentBounds.Size.X, 16), "No new highscore.", LabelStyles.DefaultLeft) { Depth = Depth + 2 };
		NestingContext.Add(header);
	}

	private void ShowFirstScoreResult(GetUploadResponseFirstScore response)
	{
		RecordingResultFirstScore firstScore = new(ContentBounds.CreateNested(0, 0, ContentBounds.Size.X, 420), response) { Depth = Depth + 2 };
		NestingContext.Add(firstScore);
	}

	private void ShowHighscoreResult(GetUploadResponseHighscore response, bool isAscending)
	{
		RecordingResultHighscore highscore = new(ContentBounds.CreateNested(0, 0, ContentBounds.Size.X, 420), response, isAscending) { Depth = Depth + 2 };
		NestingContext.Add(highscore);
	}

	private void ShowRejectionResult(GetUploadResponseRejection response)
	{
		Label header = new(ContentBounds.CreateNested(0, 0, ContentBounds.Size.X, 16), response.Reason, LabelStyles.DefaultLeft with { TextColor = Color.Red }) { Depth = Depth + 2 };
		NestingContext.Add(header);
	}
}