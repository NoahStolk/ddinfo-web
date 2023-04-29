using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using System.Diagnostics;
using AppApi = DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;

public static class CustomLeaderboardConverters
{
	public static AppApi.GetUploadResponse ToAppApi(this UploadResponse uploadResponse)
	{
		if (uploadResponse.Success != null)
			return uploadResponse.Success.ToAppApi(uploadResponse);

		if (uploadResponse.Rejection != null)
			return uploadResponse.Rejection.ToAppApi(uploadResponse);

		throw new InvalidOperationException("Invalid upload response. Both Success and Rejection are null.");
	}

	private static AppApi.GetUploadResponse ToAppApi(this UploadCriteriaRejection uploadCriteriaRejection, UploadResponse uploadResponse)
	{
		return new()
		{
			SpawnsetId = uploadResponse.Leaderboard.SpawnsetId,
			SpawnsetName = uploadResponse.Leaderboard.SpawnsetName,
			CustomLeaderboardId = uploadResponse.Leaderboard.Id,
			CriteriaRejection = new()
			{
				ActualValue = uploadCriteriaRejection.ActualValue,
				ExpectedValue = uploadCriteriaRejection.ExpectedValue,
				CriteriaName = uploadCriteriaRejection.CriteriaName,
				CriteriaOperator = uploadCriteriaRejection.CriteriaOperator.ToAppApi(),
			},
			NewSortedEntries = null,
			IsAscending = uploadResponse.Leaderboard.Category.IsAscending(),
		};
	}

	private static AppApi.GetUploadResponse ToAppApi(this SuccessfulUploadResponse successfulUploadResponse, UploadResponse uploadResponse)
	{
		List<AppApi.GetCustomEntry> sortedEntries = successfulUploadResponse.SortedEntries.ConvertAll(ce => ce.ToAppApi());

		return successfulUploadResponse.SubmissionType switch
		{
			SubmissionType.FirstScore => new()
			{
				SpawnsetId = uploadResponse.Leaderboard.SpawnsetId,
				SpawnsetName = uploadResponse.Leaderboard.SpawnsetName,
				CustomLeaderboardId = uploadResponse.Leaderboard.Id,
				FirstScore = new()
				{
					Rank = successfulUploadResponse.RankState.Value,
					Time = successfulUploadResponse.TimeState.Value,
					DaggersFired = successfulUploadResponse.DaggersFiredState.Value,
					DaggersHit = successfulUploadResponse.DaggersHitState.Value,
					EnemiesAlive = successfulUploadResponse.EnemiesAliveState.Value,
					EnemiesKilled = successfulUploadResponse.EnemiesKilledState.Value,
					GemsCollected = successfulUploadResponse.GemsCollectedState.Value,
					GemsDespawned = successfulUploadResponse.GemsDespawnedState.Value,
					GemsEaten = successfulUploadResponse.GemsEatenState.Value,
					GemsTotal = successfulUploadResponse.GemsTotalState.Value,
					HomingEaten = successfulUploadResponse.HomingEatenState.Value,
					HomingStored = successfulUploadResponse.HomingStoredState.Value,
					LevelUpTime2 = successfulUploadResponse.LevelUpTime2State.Value,
					LevelUpTime3 = successfulUploadResponse.LevelUpTime3State.Value,
					LevelUpTime4 = successfulUploadResponse.LevelUpTime4State.Value,
				},
				NewSortedEntries = sortedEntries,
				IsAscending = uploadResponse.Leaderboard.Category.IsAscending(),
			},
			SubmissionType.NewHighscore => new()
			{
				SpawnsetId = uploadResponse.Leaderboard.SpawnsetId,
				SpawnsetName = uploadResponse.Leaderboard.SpawnsetName,
				CustomLeaderboardId = uploadResponse.Leaderboard.Id,
				Highscore = new()
				{
					DaggersFiredState = successfulUploadResponse.DaggersFiredState.ToAppApi(),
					DaggersHitState = successfulUploadResponse.DaggersHitState.ToAppApi(),
					EnemiesAliveState = successfulUploadResponse.EnemiesAliveState.ToAppApi(),
					EnemiesKilledState = successfulUploadResponse.EnemiesKilledState.ToAppApi(),
					GemsCollectedState = successfulUploadResponse.GemsCollectedState.ToAppApi(),
					GemsDespawnedState = successfulUploadResponse.GemsDespawnedState.ToAppApi(),
					GemsEatenState = successfulUploadResponse.GemsEatenState.ToAppApi(),
					GemsTotalState = successfulUploadResponse.GemsTotalState.ToAppApi(),
					HomingEatenState = successfulUploadResponse.HomingEatenState.ToAppApi(),
					HomingStoredState = successfulUploadResponse.HomingStoredState.ToAppApi(),
					LevelUpTime2State = successfulUploadResponse.LevelUpTime2State.ToAppApi(),
					LevelUpTime3State = successfulUploadResponse.LevelUpTime3State.ToAppApi(),
					LevelUpTime4State = successfulUploadResponse.LevelUpTime4State.ToAppApi(),
					RankState = successfulUploadResponse.RankState.ToAppApi(),
					TimeState = successfulUploadResponse.TimeState.ToAppApi(),
				},
				NewSortedEntries = sortedEntries,
				IsAscending = uploadResponse.Leaderboard.Category.IsAscending(),
			},
			_ => new()
			{
				SpawnsetId = uploadResponse.Leaderboard.SpawnsetId,
				SpawnsetName = uploadResponse.Leaderboard.SpawnsetName,
				CustomLeaderboardId = uploadResponse.Leaderboard.Id,
				NoHighscore = new()
				{
					DaggersFiredState = successfulUploadResponse.DaggersFiredState.ToAppApi(),
					DaggersHitState = successfulUploadResponse.DaggersHitState.ToAppApi(),
					EnemiesAliveState = successfulUploadResponse.EnemiesAliveState.ToAppApi(),
					EnemiesKilledState = successfulUploadResponse.EnemiesKilledState.ToAppApi(),
					GemsCollectedState = successfulUploadResponse.GemsCollectedState.ToAppApi(),
					GemsDespawnedState = successfulUploadResponse.GemsDespawnedState.ToAppApi(),
					GemsEatenState = successfulUploadResponse.GemsEatenState.ToAppApi(),
					GemsTotalState = successfulUploadResponse.GemsTotalState.ToAppApi(),
					HomingEatenState = successfulUploadResponse.HomingEatenState.ToAppApi(),
					HomingStoredState = successfulUploadResponse.HomingStoredState.ToAppApi(),
					LevelUpTime2State = successfulUploadResponse.LevelUpTime2State.ToAppApi(),
					LevelUpTime3State = successfulUploadResponse.LevelUpTime3State.ToAppApi(),
					LevelUpTime4State = successfulUploadResponse.LevelUpTime4State.ToAppApi(),
					TimeState = successfulUploadResponse.TimeState.ToAppApi(),
				},
				NewSortedEntries = sortedEntries,
				IsAscending = uploadResponse.Leaderboard.Category.IsAscending(),
			},
		};
	}

	public static AppApi.GetCustomLeaderboard ToAppApi(this SortedCustomLeaderboard sortedCustomLeaderboard) => new()
	{
		Category = GetCategory(sortedCustomLeaderboard.RankSorting, sortedCustomLeaderboard.GameMode),
		Criteria = sortedCustomLeaderboard.Criteria.ConvertAll(c => c.ToAppApi()),
		Daggers = sortedCustomLeaderboard.Daggers?.ToAppApi(),
		IsAscending = sortedCustomLeaderboard.RankSorting.IsAscending(),
		SortedEntries = sortedCustomLeaderboard.CustomEntries.ConvertAll(ce => ce.ToAppApi()),
		SpawnsetName = sortedCustomLeaderboard.SpawnsetName,
	};

	public static AppApi.GetCustomLeaderboardForOverview ToAppApi(this CustomLeaderboardOverview customLeaderboard) => new()
	{
		Category = GetCategory(customLeaderboard.RankSorting, customLeaderboard.GameMode),
		Daggers = customLeaderboard.Daggers?.ToAppApi(),
		Id = customLeaderboard.Id,
		PlayerCount = customLeaderboard.PlayerCount,
		SelectedPlayerStats = customLeaderboard.SelectedPlayerStats?.ToAppApi(),
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetName = customLeaderboard.SpawnsetName,
		SpawnsetAuthorId = customLeaderboard.SpawnsetAuthorId,
		SpawnsetAuthorName = customLeaderboard.SpawnsetAuthorName,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		WorldRecord = customLeaderboard.WorldRecord?.ToAppApi(),
		Criteria = customLeaderboard.Criteria.ConvertAll(c => c.ToAppApi()),
	};

	private static AppApi.GetCustomLeaderboardCriteria ToAppApi(this CustomLeaderboardCriteria customLeaderboardCriteria) => new()
	{
		Type = customLeaderboardCriteria.Type.ToAppApi(),
		Expression = customLeaderboardCriteria.Expression,
		Operator = customLeaderboardCriteria.Operator.ToAppApi(),
	};

	private static AppApi.GetCustomLeaderboardDaggers ToAppApi(this CustomLeaderboardDaggers customLeaderboardDaggers) => new()
	{
		Bronze = customLeaderboardDaggers.Bronze.ToSecondsTime(),
		Silver = customLeaderboardDaggers.Silver.ToSecondsTime(),
		Golden = customLeaderboardDaggers.Golden.ToSecondsTime(),
		Devil = customLeaderboardDaggers.Devil.ToSecondsTime(),
		Leviathan = customLeaderboardDaggers.Leviathan.ToSecondsTime(),
	};

	private static AppApi.GetCustomLeaderboardSelectedPlayerStats ToAppApi(this CustomLeaderboardOverviewSelectedPlayerStats customLeaderboardOverviewSelectedPlayerStats) => new()
	{
		Dagger = customLeaderboardOverviewSelectedPlayerStats.Dagger?.ToAppApi(),
		Rank = customLeaderboardOverviewSelectedPlayerStats.Rank,
		Time = customLeaderboardOverviewSelectedPlayerStats.Time.ToSecondsTime(),
		NextDagger = customLeaderboardOverviewSelectedPlayerStats.NextDagger == null ? null : new()
		{
			Dagger = customLeaderboardOverviewSelectedPlayerStats.NextDagger.Dagger.ToAppApi(),
			Time = customLeaderboardOverviewSelectedPlayerStats.NextDagger.Time.ToSecondsTime(),
		},
	};

	private static AppApi.GetCustomLeaderboardWorldRecord ToAppApi(this CustomLeaderboardOverviewWorldRecord customLeaderboardOverviewWorldRecord) => new()
	{
		Dagger = customLeaderboardOverviewWorldRecord.Dagger?.ToAppApi(),
		Time = customLeaderboardOverviewWorldRecord.Time.ToSecondsTime(),
	};

	private static AppApi.GetScoreState<T> ToAppApi<T>(this UploadResponseScoreState<T> scoreState)
		where T : struct
		=> new(scoreState.Value, scoreState.ValueDifference);

	private static AppApi.GetCustomEntry ToAppApi(this CustomEntry customEntry) => new()
	{
		DaggersFired = customEntry.DaggersFired,
		DaggersHit = customEntry.DaggersHit,
		DeathType = customEntry.DeathType,
		EnemiesAlive = customEntry.EnemiesAlive,
		EnemiesKilled = customEntry.EnemiesKilled,
		GemsCollected = customEntry.GemsCollected,
		GemsDespawned = customEntry.GemsDespawned,
		GemsEaten = customEntry.GemsEaten,
		GemsTotal = customEntry.GemsTotal,
		HasReplay = customEntry.HasReplay,
		HomingEaten = customEntry.HomingEaten,
		HomingStored = customEntry.HomingStored,
		Id = customEntry.Id,
		LevelUpTime2InSeconds = customEntry.LevelUpTime2.ToSecondsTime(),
		LevelUpTime3InSeconds = customEntry.LevelUpTime3.ToSecondsTime(),
		LevelUpTime4InSeconds = customEntry.LevelUpTime4.ToSecondsTime(),
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.PlayerName,
		Rank = customEntry.Rank,
		SubmitDate = customEntry.SubmitDate,
		TimeInSeconds = customEntry.Time.ToSecondsTime(),
		CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger?.ToAppApi(),
	};

	private static AppApi.CustomLeaderboardDagger ToAppApi(this CustomLeaderboardDagger dagger) => dagger switch
	{
		CustomLeaderboardDagger.Default => AppApi.CustomLeaderboardDagger.Default,
		CustomLeaderboardDagger.Bronze => AppApi.CustomLeaderboardDagger.Bronze,
		CustomLeaderboardDagger.Silver => AppApi.CustomLeaderboardDagger.Silver,
		CustomLeaderboardDagger.Golden => AppApi.CustomLeaderboardDagger.Golden,
		CustomLeaderboardDagger.Devil => AppApi.CustomLeaderboardDagger.Devil,
		CustomLeaderboardDagger.Leviathan => AppApi.CustomLeaderboardDagger.Leviathan,
		_ => throw new UnreachableException(),
	};

	private static AppApi.CustomLeaderboardCriteriaType ToAppApi(this CustomLeaderboardCriteriaType criteriaType) => criteriaType switch
	{
		CustomLeaderboardCriteriaType.GemsCollected => AppApi.CustomLeaderboardCriteriaType.GemsCollected,
		CustomLeaderboardCriteriaType.GemsDespawned => AppApi.CustomLeaderboardCriteriaType.GemsDespawned,
		CustomLeaderboardCriteriaType.GemsEaten => AppApi.CustomLeaderboardCriteriaType.GemsEaten,
		CustomLeaderboardCriteriaType.EnemiesKilled => AppApi.CustomLeaderboardCriteriaType.EnemiesKilled,
		CustomLeaderboardCriteriaType.DaggersFired => AppApi.CustomLeaderboardCriteriaType.DaggersFired,
		CustomLeaderboardCriteriaType.DaggersHit => AppApi.CustomLeaderboardCriteriaType.DaggersHit,
		CustomLeaderboardCriteriaType.HomingStored => AppApi.CustomLeaderboardCriteriaType.HomingStored,
		CustomLeaderboardCriteriaType.HomingEaten => AppApi.CustomLeaderboardCriteriaType.HomingEaten,
		CustomLeaderboardCriteriaType.Skull1Kills => AppApi.CustomLeaderboardCriteriaType.Skull1Kills,
		CustomLeaderboardCriteriaType.Skull2Kills => AppApi.CustomLeaderboardCriteriaType.Skull2Kills,
		CustomLeaderboardCriteriaType.Skull3Kills => AppApi.CustomLeaderboardCriteriaType.Skull3Kills,
		CustomLeaderboardCriteriaType.Skull4Kills => AppApi.CustomLeaderboardCriteriaType.Skull4Kills,
		CustomLeaderboardCriteriaType.SpiderlingKills => AppApi.CustomLeaderboardCriteriaType.SpiderlingKills,
		CustomLeaderboardCriteriaType.SpiderEggKills => AppApi.CustomLeaderboardCriteriaType.SpiderEggKills,
		CustomLeaderboardCriteriaType.Squid1Kills => AppApi.CustomLeaderboardCriteriaType.Squid1Kills,
		CustomLeaderboardCriteriaType.Squid2Kills => AppApi.CustomLeaderboardCriteriaType.Squid2Kills,
		CustomLeaderboardCriteriaType.Squid3Kills => AppApi.CustomLeaderboardCriteriaType.Squid3Kills,
		CustomLeaderboardCriteriaType.CentipedeKills => AppApi.CustomLeaderboardCriteriaType.CentipedeKills,
		CustomLeaderboardCriteriaType.GigapedeKills => AppApi.CustomLeaderboardCriteriaType.GigapedeKills,
		CustomLeaderboardCriteriaType.GhostpedeKills => AppApi.CustomLeaderboardCriteriaType.GhostpedeKills,
		CustomLeaderboardCriteriaType.Spider1Kills => AppApi.CustomLeaderboardCriteriaType.Spider1Kills,
		CustomLeaderboardCriteriaType.Spider2Kills => AppApi.CustomLeaderboardCriteriaType.Spider2Kills,
		CustomLeaderboardCriteriaType.LeviathanKills => AppApi.CustomLeaderboardCriteriaType.LeviathanKills,
		CustomLeaderboardCriteriaType.OrbKills => AppApi.CustomLeaderboardCriteriaType.OrbKills,
		CustomLeaderboardCriteriaType.ThornKills => AppApi.CustomLeaderboardCriteriaType.ThornKills,
		CustomLeaderboardCriteriaType.Skull1sAlive => AppApi.CustomLeaderboardCriteriaType.Skull1sAlive,
		CustomLeaderboardCriteriaType.Skull2sAlive => AppApi.CustomLeaderboardCriteriaType.Skull2sAlive,
		CustomLeaderboardCriteriaType.Skull3sAlive => AppApi.CustomLeaderboardCriteriaType.Skull3sAlive,
		CustomLeaderboardCriteriaType.Skull4sAlive => AppApi.CustomLeaderboardCriteriaType.Skull4sAlive,
		CustomLeaderboardCriteriaType.SpiderlingsAlive => AppApi.CustomLeaderboardCriteriaType.SpiderlingsAlive,
		CustomLeaderboardCriteriaType.SpiderEggsAlive => AppApi.CustomLeaderboardCriteriaType.SpiderEggsAlive,
		CustomLeaderboardCriteriaType.Squid1sAlive => AppApi.CustomLeaderboardCriteriaType.Squid1sAlive,
		CustomLeaderboardCriteriaType.Squid2sAlive => AppApi.CustomLeaderboardCriteriaType.Squid2sAlive,
		CustomLeaderboardCriteriaType.Squid3sAlive => AppApi.CustomLeaderboardCriteriaType.Squid3sAlive,
		CustomLeaderboardCriteriaType.CentipedesAlive => AppApi.CustomLeaderboardCriteriaType.CentipedesAlive,
		CustomLeaderboardCriteriaType.GigapedesAlive => AppApi.CustomLeaderboardCriteriaType.GigapedesAlive,
		CustomLeaderboardCriteriaType.GhostpedesAlive => AppApi.CustomLeaderboardCriteriaType.GhostpedesAlive,
		CustomLeaderboardCriteriaType.Spider1sAlive => AppApi.CustomLeaderboardCriteriaType.Spider1sAlive,
		CustomLeaderboardCriteriaType.Spider2sAlive => AppApi.CustomLeaderboardCriteriaType.Spider2sAlive,
		CustomLeaderboardCriteriaType.LeviathansAlive => AppApi.CustomLeaderboardCriteriaType.LeviathansAlive,
		CustomLeaderboardCriteriaType.OrbsAlive => AppApi.CustomLeaderboardCriteriaType.OrbsAlive,
		CustomLeaderboardCriteriaType.ThornsAlive => AppApi.CustomLeaderboardCriteriaType.ThornsAlive,
		CustomLeaderboardCriteriaType.DeathType => AppApi.CustomLeaderboardCriteriaType.DeathType,
		CustomLeaderboardCriteriaType.Time => AppApi.CustomLeaderboardCriteriaType.Time,
		CustomLeaderboardCriteriaType.LevelUpTime2 => AppApi.CustomLeaderboardCriteriaType.LevelUpTime2,
		CustomLeaderboardCriteriaType.LevelUpTime3 => AppApi.CustomLeaderboardCriteriaType.LevelUpTime3,
		CustomLeaderboardCriteriaType.LevelUpTime4 => AppApi.CustomLeaderboardCriteriaType.LevelUpTime4,
		CustomLeaderboardCriteriaType.EnemiesAlive => AppApi.CustomLeaderboardCriteriaType.EnemiesAlive,
		_ => throw new UnreachableException(),
	};

	private static AppApi.CustomLeaderboardCriteriaOperator ToAppApi(this CustomLeaderboardCriteriaOperator @operator) => @operator switch
	{
		CustomLeaderboardCriteriaOperator.Any => AppApi.CustomLeaderboardCriteriaOperator.Any,
		CustomLeaderboardCriteriaOperator.Equal => AppApi.CustomLeaderboardCriteriaOperator.Equal,
		CustomLeaderboardCriteriaOperator.LessThan => AppApi.CustomLeaderboardCriteriaOperator.LessThan,
		CustomLeaderboardCriteriaOperator.GreaterThan => AppApi.CustomLeaderboardCriteriaOperator.GreaterThan,
		CustomLeaderboardCriteriaOperator.LessThanOrEqual => AppApi.CustomLeaderboardCriteriaOperator.LessThanOrEqual,
		CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => AppApi.CustomLeaderboardCriteriaOperator.GreaterThanOrEqual,
		CustomLeaderboardCriteriaOperator.Modulo => AppApi.CustomLeaderboardCriteriaOperator.Modulo,
		CustomLeaderboardCriteriaOperator.NotEqual => AppApi.CustomLeaderboardCriteriaOperator.NotEqual,
		_ => throw new UnreachableException(),
	};

	/// <summary>
	/// Workaround to keep the API backwards compatible with the old categories.
	/// </summary>
	private static AppApi.CustomLeaderboardCategory GetCategory(CustomLeaderboardRankSorting rankSorting, SpawnsetGameMode gameMode)
	{
		if (rankSorting == CustomLeaderboardRankSorting.TimeAsc)
		{
			return gameMode switch
			{
				SpawnsetGameMode.Survival => AppApi.CustomLeaderboardCategory.Speedrun,
				SpawnsetGameMode.TimeAttack => AppApi.CustomLeaderboardCategory.TimeAttack,
				SpawnsetGameMode.Race => AppApi.CustomLeaderboardCategory.Race,
				_ => throw new UnreachableException(),
			};
		}

		return AppApi.CustomLeaderboardCategory.Survival;
	}
}
