using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using DevilDaggersInfo.Common;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording;

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
		Label header = new(ContentBounds.CreateNested(0, 0, ContentBounds.Size.X, 16), "First score!", LabelStyles.DefaultLeft) { Depth = Depth + 2 };
		NestingContext.Add(header);

		int y = 24;
		Add("Rank", response.Rank, i => i.ToString());
		Add("Time", response.Time, d => d.ToString(StringFormats.TimeFormat));
		Add("Gems collected", response.GemsCollected, i => i.ToString());
		Add("Enemies killed", response.EnemiesKilled, i => i.ToString());
		Add("Daggers fired", response.DaggersFired, i => i.ToString());
		Add("Daggers hit", response.DaggersHit, i => i.ToString());
		Add("Enemies alive", response.EnemiesAlive, i => i.ToString());
		Add("Homing stored", response.HomingStored, i => i.ToString());
		Add("Homing eaten", response.HomingEaten, i => i.ToString());
		Add("Gems despawned", response.GemsDespawned, i => i.ToString());
		Add("Gems eaten", response.GemsEaten, i => i.ToString());
		Add("Gems total", response.GemsTotal, i => i.ToString());
		Add("Level up 2", response.LevelUpTime2, i => i.ToString(StringFormats.TimeFormat));
		Add("Level up 3", response.LevelUpTime3, i => i.ToString(StringFormats.TimeFormat));
		Add("Level up 4", response.LevelUpTime4, i => i.ToString(StringFormats.TimeFormat));

		void Add<T>(string label, T value, Func<T, string> formatter)
			where T : struct
		{
			Label left = new(ContentBounds.CreateNested(0, y, ContentBounds.Size.X / 2, 16), label, LabelStyles.DefaultLeft) { Depth = Depth + 2 };
			Label right = new(ContentBounds.CreateNested(ContentBounds.Size.X / 2, y, ContentBounds.Size.X / 2, 16), formatter(value), LabelStyles.DefaultRight) { Depth = Depth + 2 };
			NestingContext.Add(left);
			NestingContext.Add(right);

			y += 16;
		}
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

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(ContentBounds.Size, ContentBounds.Center + scrollOffset, Depth, Color.Orange);
		Root.Game.RectangleRenderer.Schedule(ContentBounds.Size - new Vector2i<int>(border * 2), ContentBounds.Center + scrollOffset, 1, Color.Black);
	}
}
