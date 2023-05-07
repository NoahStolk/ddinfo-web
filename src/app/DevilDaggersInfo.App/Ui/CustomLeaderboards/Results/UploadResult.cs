using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Ui.Base.Extensions;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.CriteriaExpression.Extensions;
using DevilDaggersInfo.Core.Wiki.Objects;
using ImGuiNET;
using System.Diagnostics;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards.Results;

// TODO: Rewrite to use discriminated unions if they ever get added to C#.
public class UploadResult
{
	private static readonly Vector2 _iconSize = new(16);

	private readonly bool _isAscending;
	private readonly string _spawnsetName;

	public UploadResult(GetUploadResponseFirstScore firstScore, bool isAscending, string spawnsetName, DateTime submittedAt)
		: this(isAscending, spawnsetName, submittedAt) => FirstScore = firstScore;
	public UploadResult(GetUploadResponseHighscore highscore, bool isAscending, string spawnsetName, DateTime submittedAt)
		: this(isAscending, spawnsetName, submittedAt) => Highscore = highscore;
	public UploadResult(GetUploadResponseNoHighscore noHighscore, bool isAscending, string spawnsetName, DateTime submittedAt)
		: this(isAscending, spawnsetName, submittedAt) => NoHighscore = noHighscore;
	public UploadResult(GetUploadResponseCriteriaRejection criteriaRejection, bool isAscending, string spawnsetName, DateTime submittedAt)
		: this(isAscending, spawnsetName, submittedAt) => CriteriaRejection = criteriaRejection;

	private UploadResult(bool isAscending, string spawnsetName, DateTime submittedAt)
	{
		_isAscending = isAscending;
		_spawnsetName = spawnsetName;
		SubmittedAt = submittedAt;
	}

	public DateTime SubmittedAt { get; }
	public bool IsExpanded { get; set; } = true;

	public GetUploadResponseFirstScore? FirstScore { get; }
	public GetUploadResponseHighscore? Highscore { get; }
	public GetUploadResponseNoHighscore? NoHighscore { get; }
	public GetUploadResponseCriteriaRejection? CriteriaRejection { get; }

	public void Render()
	{
		if (FirstScore != null)
			RenderFirstScore(FirstScore);
		else if (Highscore != null)
			RenderHighscore(Highscore);
		else if (NoHighscore != null)
			RenderNoHighscore(NoHighscore);
		else if (CriteriaRejection != null)
			RenderCriteriaRejection(CriteriaRejection);
		else
			throw new UnreachableException();
	}

	private void RenderFirstScore(GetUploadResponseFirstScore firstScore)
	{
		if (RenderHeader(Color.Aqua, "First score!"))
		{
			Add("Rank", firstScore.Rank, i => i.ToString());

			ImGui.Spacing();
			ImGui.Image((IntPtr)Root.InternalResources.IconEyeTexture.Handle, _iconSize); // TODO: Orange
			Add("Time", firstScore.Time, d => d.ToString(StringFormats.TimeFormat));
			Add("Level 2", firstScore.LevelUpTime2, i => i.ToString(StringFormats.TimeFormat));
			Add("Level 3", firstScore.LevelUpTime3, i => i.ToString(StringFormats.TimeFormat));
			Add("Level 4", firstScore.LevelUpTime4, i => i.ToString(StringFormats.TimeFormat));
			//AddDeath();

			ImGui.Spacing();
			ImGui.Image((IntPtr)Root.InternalResources.IconGemTexture.Handle, _iconSize); // TODO: Red
			Add("Gems collected", firstScore.GemsCollected, i => i.ToString());
			Add("Gems despawned", firstScore.GemsDespawned, i => i.ToString());
			Add("Gems eaten", firstScore.GemsEaten, i => i.ToString());
			Add("Gems total", firstScore.GemsTotal, i => i.ToString());

			ImGui.Spacing();
			ImGui.Image((IntPtr)Root.InternalResources.IconHomingTexture.Handle, _iconSize);
			Add("Homing stored", firstScore.HomingStored, i => i.ToString());
			Add("Homing eaten", firstScore.HomingEaten, i => i.ToString());

			ImGui.Spacing();
			ImGui.Image((IntPtr)Root.InternalResources.IconCrosshairTexture.Handle, _iconSize); // TODO: Green
			Add("Daggers fired", firstScore.DaggersFired, i => i.ToString());
			Add("Daggers hit", firstScore.DaggersHit, i => i.ToString());

			double accuracy = firstScore.DaggersFired == 0 ? 0 : firstScore.DaggersHit / (double)firstScore.DaggersFired;
			Add("Accuracy", accuracy, i => i.ToString(StringFormats.AccuracyFormat));

			ImGui.Spacing();
			ImGui.Image((IntPtr)Root.InternalResources.IconSkullTexture.Handle, _iconSize); // TODO: EnemiesV3_2.Skull4.Color.ToEngineColor())
			Add("Enemies killed", firstScore.EnemiesKilled, i => i.ToString());
			Add("Enemies alive", firstScore.EnemiesAlive, i => i.ToString());

			void Add<T>(string label, T value, Func<T, string> formatter)
				where T : struct
			{
				ImGui.Text(label);
				ImGui.SameLine();
				ImGui.TextUnformatted(formatter(value));
			}
		}
	}

	private void RenderHighscore(GetUploadResponseHighscore highscore)
	{
		if (RenderHeader(Color.Green, "NEW HIGHSCORE!"))
		{
			AddScoreState("Rank", highscore.RankState, i => i.ToString(), i => $"{i:+0;-0;+0}");

			AddStates(
				_isAscending,
				highscore.TimeState,
				highscore.LevelUpTime2State,
				highscore.LevelUpTime3State,
				highscore.LevelUpTime4State,
				highscore.EnemiesKilledState,
				highscore.EnemiesAliveState,
				highscore.GemsCollectedState,
				highscore.GemsDespawnedState,
				highscore.GemsEatenState,
				highscore.GemsTotalState,
				highscore.HomingStoredState,
				highscore.HomingEatenState,
				highscore.DaggersFiredState,
				highscore.DaggersHitState);
		}
	}

	private void RenderNoHighscore(GetUploadResponseNoHighscore noHighscore)
	{
		if (RenderHeader(Color.White, "No new highscore."))
		{
			AddStates(
				_isAscending,
				noHighscore.TimeState,
				noHighscore.LevelUpTime2State,
				noHighscore.LevelUpTime3State,
				noHighscore.LevelUpTime4State,
				noHighscore.EnemiesKilledState,
				noHighscore.EnemiesAliveState,
				noHighscore.GemsCollectedState,
				noHighscore.GemsDespawnedState,
				noHighscore.GemsEatenState,
				noHighscore.GemsTotalState,
				noHighscore.HomingStoredState,
				noHighscore.HomingEatenState,
				noHighscore.DaggersFiredState,
				noHighscore.DaggersHitState);
		}
	}

	private void RenderCriteriaRejection(GetUploadResponseCriteriaRejection criteriaRejection)
	{
		if (RenderHeader(Color.Red, "Rejected score."))
		{
			ImGui.Text(criteriaRejection.CriteriaName);
			ImGui.Text($"Must be {criteriaRejection.CriteriaOperator.ToCore().ShortString()} {criteriaRejection.ExpectedValue}");
			ImGui.Text($"Value was {criteriaRejection.ActualValue}");
		}
	}

	private bool RenderHeader(Color color, string title)
	{
		ImGui.PushStyleColor(ImGuiCol.Text, color);
		if (ImGui.Button($"{SubmittedAt:HH:mm:ss} - {_spawnsetName} - {title}", new(320, 48)))
			IsExpanded = !IsExpanded;
		ImGui.PopStyleColor();
		return IsExpanded;
	}

	private static void AddStates(
		bool isAscending,
		GetScoreState<double> timeState,
		GetScoreState<double> levelUpTime2State,
		GetScoreState<double> levelUpTime3State,
		GetScoreState<double> levelUpTime4State,
		GetScoreState<int> enemiesKilledState,
		GetScoreState<int> enemiesAliveState,
		GetScoreState<int> gemsCollectedState,
		GetScoreState<int> gemsDespawnedState,
		GetScoreState<int> gemsEatenState,
		GetScoreState<int> gemsTotalState,
		GetScoreState<int> homingStoredState,
		GetScoreState<int> homingEatenState,
		GetScoreState<int> daggersFiredState,
		GetScoreState<int> daggersHitState)
	{
		ImGui.Spacing();
		ImGui.Image((IntPtr)Root.InternalResources.IconEyeTexture.Handle, _iconSize); // TODO: Orange

		AddScoreState("Time", timeState, d => d.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;+0.0000}", !isAscending);
		AddLevelUpScoreState("Level 2", levelUpTime2State);
		AddLevelUpScoreState("Level 3", levelUpTime3State);
		AddLevelUpScoreState("Level 4", levelUpTime4State);

		// TODO: Add death type to API.
		// Death? death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, deathType);
		// AddDeath(death);

		ImGui.Spacing();
		ImGui.Image((IntPtr)Root.InternalResources.IconGemTexture.Handle, _iconSize); // TODO: Red
		AddScoreState("Gems collected", gemsCollectedState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		AddScoreState("Gems despawned", gemsDespawnedState, i => i.ToString(), i => $"{i:+0;-0;+0}", false);
		AddScoreState("Gems eaten", gemsEatenState, i => i.ToString(), i => $"{i:+0;-0;+0}", false);
		AddScoreState("Gems total", gemsTotalState, i => i.ToString(), i => $"{i:+0;-0;+0}");

		ImGui.Spacing();
		ImGui.Image((IntPtr)Root.InternalResources.IconHomingTexture.Handle, _iconSize);
		AddScoreState("Homing stored", homingStoredState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		AddScoreState("Homing eaten", homingEatenState, i => i.ToString(), i => $"{i:+0;-0;+0}", false);

		ImGui.Spacing();
		ImGui.Image((IntPtr)Root.InternalResources.IconCrosshairTexture.Handle, _iconSize); // TODO: Green
		AddScoreState("Daggers fired", daggersFiredState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		AddScoreState("Daggers hit", daggersHitState, i => i.ToString(), i => $"{i:+0;-0;+0}");

		static double GetAccuracy(int daggersFired, int daggersHit)
			=> daggersFired == 0 ? 0 : daggersHit / (double)daggersFired;

		double accuracy = GetAccuracy(daggersFiredState.Value, daggersHitState.Value);
		double oldAccuracy = GetAccuracy(daggersFiredState.Value - daggersFiredState.ValueDifference, daggersHitState.Value - daggersHitState.ValueDifference);
		GetScoreState<double> accuracyState = new()
		{
			Value = accuracy,
			ValueDifference = accuracy - oldAccuracy,
		};
		AddScoreState("Accuracy", accuracyState, i => i.ToString(StringFormats.AccuracyFormat), i => $"{i:+0.00%;-0.00%;+0.00%}");

		ImGui.Spacing();
		ImGui.Image((IntPtr)Root.InternalResources.IconSkullTexture.Handle, _iconSize); // TODO: EnemiesV3_2.Skull4.Color.ToEngineColor())
		AddScoreState("Enemies killed", enemiesKilledState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		AddScoreState("Enemies alive", enemiesAliveState, i => i.ToString(), i => $"{i:+0;-0;+0}");
	}

	private static void AddScoreState<T>(string label, GetScoreState<T> scoreState, Func<T, string> formatter, Func<T, string> formatterDifference, bool higherIsBetter = true)
		where T : struct, INumber<T>
	{
		int comparison = scoreState.ValueDifference.CompareTo(T.Zero);
		if (!higherIsBetter)
			comparison = -comparison;
		Color color = comparison switch
		{
			-1 => Color.Red,
			0 => Color.White,
			1 => Color.Green,
			_ => throw new UnreachableException(),
		};

		ImGui.Text(label);
		ImGui.SameLine();
		ImGui.TextUnformatted(formatter(scoreState.Value));
		ImGui.SameLine();
		ImGui.PushStyleColor(ImGuiCol.Text, color);
		ImGui.TextUnformatted(formatterDifference(scoreState.ValueDifference));
		ImGui.PopStyleColor();
	}

	private static void AddLevelUpScoreState(string label, GetScoreState<double> scoreState)
	{
		int comparison = -scoreState.ValueDifference.CompareTo(0);
		Color color = comparison switch
		{
			-1 => Color.Red,
			0 => Color.White,
			1 => Color.Green,
			_ => throw new UnreachableException(),
		};

		const double tolerance = 0.000001;

		// Show N/A for level up time when level was not achieved, for both value and difference.
		// Also, if the previous level up time was 0, but the level was achieved this time, the difference is equal to the value. Show N/A in this case as well.
		bool levelWasNotAchieved = scoreState.Value <= tolerance;
		bool diffIsSameAsValue = Math.Abs(scoreState.Value - scoreState.ValueDifference) <= tolerance;
		bool hideDifference = levelWasNotAchieved || diffIsSameAsValue;

		string levelUp = levelWasNotAchieved ? "N/A" : scoreState.Value.ToString(StringFormats.TimeFormat);
		string levelUpDifference = hideDifference ? "N/A" : scoreState.ValueDifference.ToString("+0.0000;-0.0000;+0.0000");

		ImGui.Text(label);
		ImGui.SameLine();
		ImGui.Text(levelUp);
		ImGui.SameLine();
		ImGui.TextColored(hideDifference ? Color.White : color, levelUpDifference);
	}

	private static void AddDeath(Death? death)
	{
		ImGui.Text("Death");
		ImGui.SameLine();
		ImGui.TextColored(death?.Color.ToEngineColor() ?? Color.White, death?.Name ?? "Unknown");
	}
}
