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
	public UploadResult(GetUploadResponseFirstScore firstScore) => FirstScore = firstScore;
	public UploadResult(GetUploadResponseHighscore highscore) => Highscore = highscore;
	public UploadResult(GetUploadResponseNoHighscore noHighscore) => NoHighscore = noHighscore;
	public UploadResult(GetUploadResponseCriteriaRejection criteriaRejection) => CriteriaRejection = criteriaRejection;

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

	private static void RenderFirstScore(GetUploadResponseFirstScore firstScore)
	{

	}

	private static void RenderHighscore(GetUploadResponseHighscore highscore)
	{

	}

	private static void RenderNoHighscore(GetUploadResponseNoHighscore noHighscore)
	{
		ImGui.Text("No new highscore.");

		AddStates(
			false, // TODO
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

	private static void RenderCriteriaRejection(GetUploadResponseCriteriaRejection criteriaRejection)
	{
		ImGui.TextColored(Color.Red, "Run was rejected.");

		ImGui.Text(criteriaRejection.CriteriaName);

		ImGui.Text($"Must be {criteriaRejection.CriteriaOperator.ToCore().ShortString()} {criteriaRejection.ExpectedValue}");

		ImGui.Text($"Value was {criteriaRejection.ActualValue}");
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
		Vector2 iconSize = new(16);

		ImGui.Spacing();
		ImGui.Image((IntPtr)Root.InternalResources.IconEyeTexture.Handle, iconSize); // TODO: Orange

		AddScoreState("Time", timeState, d => d.ToString(StringFormats.TimeFormat), i => $"{i:+0.0000;-0.0000;+0.0000}", !isAscending);
		AddLevelUpScoreState("Level 2", levelUpTime2State);
		AddLevelUpScoreState("Level 3", levelUpTime3State);
		AddLevelUpScoreState("Level 4", levelUpTime4State);

		// TODO: Add death type to API.
		// Death? death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, deathType);
		// AddDeath(death);

		ImGui.Spacing();
		ImGui.Image((IntPtr)Root.InternalResources.IconGemTexture.Handle, iconSize); // TODO: Red
		AddScoreState("Gems collected", gemsCollectedState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		AddScoreState("Gems despawned", gemsDespawnedState, i => i.ToString(), i => $"{i:+0;-0;+0}", false);
		AddScoreState("Gems eaten", gemsEatenState, i => i.ToString(), i => $"{i:+0;-0;+0}", false);
		AddScoreState("Gems total", gemsTotalState, i => i.ToString(), i => $"{i:+0;-0;+0}");

		ImGui.Spacing();
		ImGui.Image((IntPtr)Root.InternalResources.IconHomingTexture.Handle, iconSize);
		AddScoreState("Homing stored", homingStoredState, i => i.ToString(), i => $"{i:+0;-0;+0}");
		AddScoreState("Homing eaten", homingEatenState, i => i.ToString(), i => $"{i:+0;-0;+0}", false);

		ImGui.Spacing();
		ImGui.Image((IntPtr)Root.InternalResources.IconCrosshairTexture.Handle, iconSize); // TODO: Green
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
		ImGui.Image((IntPtr)Root.InternalResources.IconSkullTexture.Handle, iconSize); // TODO: EnemiesV3_2.Skull4.Color.ToEngineColor())
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
		ImGui.Text(formatter(scoreState.Value));
		ImGui.SameLine();
		ImGui.TextColored(color, formatterDifference(scoreState.ValueDifference));
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
