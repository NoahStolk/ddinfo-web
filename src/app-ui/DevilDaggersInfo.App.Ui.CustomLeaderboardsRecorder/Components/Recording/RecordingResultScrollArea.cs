using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using DevilDaggersInfo.Common;
using System.Diagnostics;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording;

public class RecordingResultScrollArea : ScrollArea
{
	public RecordingResultScrollArea(IBounds bounds)
		: base(bounds, 64, 16, ScrollAreaStyles.Default)
	{
		StateManager.Subscribe<SetSuccessfulUpload>(ShowResult);
		StateManager.Subscribe<SetFailedUpload>(ShowResult);
	}

	private void ShowResult()
	{
		foreach (AbstractComponent component in NestingContext.OrderedComponents)
			NestingContext.Remove(component);

		string? error = StateManager.UploadSuccessState.UploadError;
		GetUploadSuccess? success = StateManager.UploadSuccessState.UploadSuccess;

		string message = success != null ? success.SubmissionType switch
		{
			SubmissionType.NoHighscore => "No new highscore.",
			SubmissionType.NewHighscore => "NEW HIGHSCORE!",
			SubmissionType.FirstScore => "First score!",
			_ => throw new UnreachableException(),
		} : error ?? "No error specified.";

		Label header = new(ContentBounds.CreateNested(0, 0, ContentBounds.Size.X, 16), message, LabelStyles.DefaultLeft) { Depth = Depth + 2 };
		NestingContext.Add(header);

		if (success == null || success.SubmissionType == SubmissionType.NoHighscore)
			return;

		bool isHighscore = success.SubmissionType == SubmissionType.NewHighscore;

		int y = 24;
		Add(ref y, isHighscore, "Rank", success.RankState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Time", success.TimeState, d => d.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;0.0000}");
		Add(ref y, isHighscore, "Gems collected", success.GemsCollectedState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Enemies killed", success.EnemiesKilledState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Daggers fired", success.DaggersFiredState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Daggers hit", success.DaggersHitState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Enemies alive", success.EnemiesAliveState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Homing stored", success.HomingStoredState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Homing eaten", success.HomingEatenState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Gems despawned", success.GemsDespawnedState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Gems eaten", success.GemsEatenState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Gems total", success.GemsTotalState, i => i.ToString(), i => $"{i:+0;-0;0}");
		Add(ref y, isHighscore, "Level up 2", success.LevelUpTime2State, i => i.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;0.0000}");
		Add(ref y, isHighscore, "Level up 3", success.LevelUpTime3State, i => i.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;0.0000}");
		Add(ref y, isHighscore, "Level up 4", success.LevelUpTime4State, i => i.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;0.0000}");
	}

	private void Add<T>(ref int y, bool isHighscore, string label, GetScoreState<T> scoreState, Func<T, string> formatter, Func<T, string> formatterDifference)
		where T : struct
	{
		Label left = new(ContentBounds.CreateNested(0, y, ContentBounds.Size.X / 2, 16), label, LabelStyles.DefaultLeft) { Depth = Depth + 2 };
		Label right = new(ContentBounds.CreateNested(ContentBounds.Size.X / 2, y, ContentBounds.Size.X / 2, 16), GetScoreState(scoreState, formatter, formatterDifference, isHighscore), LabelStyles.DefaultRight) { Depth = Depth + 2 };
		NestingContext.Add(left);
		NestingContext.Add(right);

		y += 16;
	}

	private static string GetScoreState<T>(GetScoreState<T> scoreState, Func<T, string> formatter, Func<T, string> formatterDifference, bool isHighscore)
		where T : struct
	{
		return isHighscore ? $"{formatter(scoreState.Value)} ({formatterDifference(scoreState.ValueDifference)})" : formatter(scoreState.Value);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(ContentBounds.Size, ContentBounds.Center + scrollOffset, Depth, Color.Orange);
		Root.Game.RectangleRenderer.Schedule(ContentBounds.Size - new Vector2i<int>(border * 2), ContentBounds.Center + scrollOffset, 1, Color.Black);
	}
}
