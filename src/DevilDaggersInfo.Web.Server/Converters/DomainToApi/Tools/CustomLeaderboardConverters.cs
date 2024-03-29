using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using System.Diagnostics;
using ToolsApi = DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Tools;

public static class CustomLeaderboardConverters
{
	public static ToolsApi.GetUploadResponse ToAppApi(this UploadResponse uploadResponse)
	{
		if (uploadResponse.Success != null)
			return uploadResponse.Success.ToAppApi(uploadResponse);

		if (uploadResponse.Rejection != null)
			return uploadResponse.Rejection.ToAppApi(uploadResponse);

		throw new InvalidOperationException("Invalid upload response. Both Success and Rejection are null.");
	}

	private static ToolsApi.GetUploadResponse ToAppApi(this UploadCriteriaRejection uploadCriteriaRejection, UploadResponse uploadResponse)
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
			IsAscending = uploadResponse.Leaderboard.RankSorting.IsAscending(),
		};
	}

	private static ToolsApi.GetUploadResponse ToAppApi(this SuccessfulUploadResponse successfulUploadResponse, UploadResponse uploadResponse)
	{
		List<ToolsApi.GetCustomEntry> sortedEntries = successfulUploadResponse.SortedEntries.ConvertAll(ce => ce.ToAppApi());

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
				IsAscending = uploadResponse.Leaderboard.RankSorting.IsAscending(),
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
				IsAscending = uploadResponse.Leaderboard.RankSorting.IsAscending(),
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
				IsAscending = uploadResponse.Leaderboard.RankSorting.IsAscending(),
			},
		};
	}

	public static ToolsApi.GetCustomLeaderboard ToAppApi(this SortedCustomLeaderboard sortedCustomLeaderboard)
	{
		return new()
		{
			Criteria = sortedCustomLeaderboard.Criteria.ConvertAll(c => c.ToAppApi()),
			Daggers = sortedCustomLeaderboard.Daggers?.ToAppApi(),
			SortedEntries = sortedCustomLeaderboard.CustomEntries.ConvertAll(ce => ce.ToAppApi()),
			SpawnsetName = sortedCustomLeaderboard.SpawnsetName,
			RankSorting = sortedCustomLeaderboard.RankSorting.ToAppApi(),
			SpawnsetGameMode = sortedCustomLeaderboard.GameMode.ToAppApi(),
		};
	}

	public static ToolsApi.GetCustomLeaderboardForOverview ToAppApi(this CustomLeaderboardOverview customLeaderboard)
	{
		return new()
		{
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
			RankSorting = customLeaderboard.RankSorting.ToAppApi(),
			SpawnsetGameMode = customLeaderboard.GameMode.ToAppApi(),
		};
	}

	private static ToolsApi.GetCustomLeaderboardCriteria ToAppApi(this CustomLeaderboardCriteria customLeaderboardCriteria)
	{
		return new()
		{
			Type = customLeaderboardCriteria.Type.ToAppApi(),
			Expression = customLeaderboardCriteria.Expression,
			Operator = customLeaderboardCriteria.Operator.ToAppApi(),
		};
	}

	private static ToolsApi.GetCustomLeaderboardDaggers ToAppApi(this CustomLeaderboardDaggers customLeaderboardDaggers)
	{
		return new()
		{
			Bronze = GameTime.FromGameUnits(customLeaderboardDaggers.Bronze).Seconds,
			Silver = GameTime.FromGameUnits(customLeaderboardDaggers.Silver).Seconds,
			Golden = GameTime.FromGameUnits(customLeaderboardDaggers.Golden).Seconds,
			Devil = GameTime.FromGameUnits(customLeaderboardDaggers.Devil).Seconds,
			Leviathan = GameTime.FromGameUnits(customLeaderboardDaggers.Leviathan).Seconds,
		};
	}

	private static ToolsApi.GetCustomLeaderboardSelectedPlayerStats ToAppApi(this CustomLeaderboardOverviewSelectedPlayerStats customLeaderboardOverviewSelectedPlayerStats)
	{
		return new()
		{
			Dagger = customLeaderboardOverviewSelectedPlayerStats.Dagger?.ToAppApi(),
			Rank = customLeaderboardOverviewSelectedPlayerStats.Rank,
			HighscoreValue = customLeaderboardOverviewSelectedPlayerStats.HighscoreValue,
			NextDagger = customLeaderboardOverviewSelectedPlayerStats.NextDagger == null ? null : new()
			{
				Dagger = customLeaderboardOverviewSelectedPlayerStats.NextDagger.Dagger.ToAppApi(),
				DaggerValue = customLeaderboardOverviewSelectedPlayerStats.NextDagger.DaggerValue,
			},
		};
	}

	private static ToolsApi.GetCustomLeaderboardWorldRecord ToAppApi(this CustomLeaderboardOverviewWorldRecord customLeaderboardOverviewWorldRecord)
	{
		return new()
		{
			Dagger = customLeaderboardOverviewWorldRecord.Dagger?.ToAppApi(),
			WorldRecordValue = customLeaderboardOverviewWorldRecord.WorldRecordValue,
		};
	}

	private static ToolsApi.GetScoreState<T> ToAppApi<T>(this UploadResponseScoreState<T> scoreState)
		where T : struct
	{
		return new(scoreState.Value, scoreState.ValueDifference);
	}

	private static ToolsApi.GetCustomEntry ToAppApi(this CustomEntry customEntry)
	{
		return new()
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
			LevelUpTime2InSeconds = GameTime.FromGameUnits(customEntry.LevelUpTime2).Seconds,
			LevelUpTime3InSeconds = GameTime.FromGameUnits(customEntry.LevelUpTime3).Seconds,
			LevelUpTime4InSeconds = GameTime.FromGameUnits(customEntry.LevelUpTime4).Seconds,
			PlayerId = customEntry.PlayerId,
			PlayerName = customEntry.PlayerName,
			Rank = customEntry.Rank,
			SubmitDate = customEntry.SubmitDate,
			TimeInSeconds = GameTime.FromGameUnits(customEntry.Time).Seconds,
			CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger?.ToAppApi(),
		};
	}

	private static ToolsApi.CustomLeaderboardDagger ToAppApi(this CustomLeaderboardDagger dagger)
	{
		return dagger switch
		{
			CustomLeaderboardDagger.Default => ToolsApi.CustomLeaderboardDagger.Default,
			CustomLeaderboardDagger.Bronze => ToolsApi.CustomLeaderboardDagger.Bronze,
			CustomLeaderboardDagger.Silver => ToolsApi.CustomLeaderboardDagger.Silver,
			CustomLeaderboardDagger.Golden => ToolsApi.CustomLeaderboardDagger.Golden,
			CustomLeaderboardDagger.Devil => ToolsApi.CustomLeaderboardDagger.Devil,
			CustomLeaderboardDagger.Leviathan => ToolsApi.CustomLeaderboardDagger.Leviathan,
			_ => throw new UnreachableException(),
		};
	}

	private static ToolsApi.CustomLeaderboardCriteriaType ToAppApi(this CustomLeaderboardCriteriaType criteriaType)
	{
		return criteriaType switch
		{
			CustomLeaderboardCriteriaType.GemsCollected => ToolsApi.CustomLeaderboardCriteriaType.GemsCollected,
			CustomLeaderboardCriteriaType.GemsDespawned => ToolsApi.CustomLeaderboardCriteriaType.GemsDespawned,
			CustomLeaderboardCriteriaType.GemsEaten => ToolsApi.CustomLeaderboardCriteriaType.GemsEaten,
			CustomLeaderboardCriteriaType.EnemiesKilled => ToolsApi.CustomLeaderboardCriteriaType.EnemiesKilled,
			CustomLeaderboardCriteriaType.DaggersFired => ToolsApi.CustomLeaderboardCriteriaType.DaggersFired,
			CustomLeaderboardCriteriaType.DaggersHit => ToolsApi.CustomLeaderboardCriteriaType.DaggersHit,
			CustomLeaderboardCriteriaType.HomingStored => ToolsApi.CustomLeaderboardCriteriaType.HomingStored,
			CustomLeaderboardCriteriaType.HomingEaten => ToolsApi.CustomLeaderboardCriteriaType.HomingEaten,
			CustomLeaderboardCriteriaType.Skull1Kills => ToolsApi.CustomLeaderboardCriteriaType.Skull1Kills,
			CustomLeaderboardCriteriaType.Skull2Kills => ToolsApi.CustomLeaderboardCriteriaType.Skull2Kills,
			CustomLeaderboardCriteriaType.Skull3Kills => ToolsApi.CustomLeaderboardCriteriaType.Skull3Kills,
			CustomLeaderboardCriteriaType.Skull4Kills => ToolsApi.CustomLeaderboardCriteriaType.Skull4Kills,
			CustomLeaderboardCriteriaType.SpiderlingKills => ToolsApi.CustomLeaderboardCriteriaType.SpiderlingKills,
			CustomLeaderboardCriteriaType.SpiderEggKills => ToolsApi.CustomLeaderboardCriteriaType.SpiderEggKills,
			CustomLeaderboardCriteriaType.Squid1Kills => ToolsApi.CustomLeaderboardCriteriaType.Squid1Kills,
			CustomLeaderboardCriteriaType.Squid2Kills => ToolsApi.CustomLeaderboardCriteriaType.Squid2Kills,
			CustomLeaderboardCriteriaType.Squid3Kills => ToolsApi.CustomLeaderboardCriteriaType.Squid3Kills,
			CustomLeaderboardCriteriaType.CentipedeKills => ToolsApi.CustomLeaderboardCriteriaType.CentipedeKills,
			CustomLeaderboardCriteriaType.GigapedeKills => ToolsApi.CustomLeaderboardCriteriaType.GigapedeKills,
			CustomLeaderboardCriteriaType.GhostpedeKills => ToolsApi.CustomLeaderboardCriteriaType.GhostpedeKills,
			CustomLeaderboardCriteriaType.Spider1Kills => ToolsApi.CustomLeaderboardCriteriaType.Spider1Kills,
			CustomLeaderboardCriteriaType.Spider2Kills => ToolsApi.CustomLeaderboardCriteriaType.Spider2Kills,
			CustomLeaderboardCriteriaType.LeviathanKills => ToolsApi.CustomLeaderboardCriteriaType.LeviathanKills,
			CustomLeaderboardCriteriaType.OrbKills => ToolsApi.CustomLeaderboardCriteriaType.OrbKills,
			CustomLeaderboardCriteriaType.ThornKills => ToolsApi.CustomLeaderboardCriteriaType.ThornKills,
			CustomLeaderboardCriteriaType.Skull1sAlive => ToolsApi.CustomLeaderboardCriteriaType.Skull1sAlive,
			CustomLeaderboardCriteriaType.Skull2sAlive => ToolsApi.CustomLeaderboardCriteriaType.Skull2sAlive,
			CustomLeaderboardCriteriaType.Skull3sAlive => ToolsApi.CustomLeaderboardCriteriaType.Skull3sAlive,
			CustomLeaderboardCriteriaType.Skull4sAlive => ToolsApi.CustomLeaderboardCriteriaType.Skull4sAlive,
			CustomLeaderboardCriteriaType.SpiderlingsAlive => ToolsApi.CustomLeaderboardCriteriaType.SpiderlingsAlive,
			CustomLeaderboardCriteriaType.SpiderEggsAlive => ToolsApi.CustomLeaderboardCriteriaType.SpiderEggsAlive,
			CustomLeaderboardCriteriaType.Squid1sAlive => ToolsApi.CustomLeaderboardCriteriaType.Squid1sAlive,
			CustomLeaderboardCriteriaType.Squid2sAlive => ToolsApi.CustomLeaderboardCriteriaType.Squid2sAlive,
			CustomLeaderboardCriteriaType.Squid3sAlive => ToolsApi.CustomLeaderboardCriteriaType.Squid3sAlive,
			CustomLeaderboardCriteriaType.CentipedesAlive => ToolsApi.CustomLeaderboardCriteriaType.CentipedesAlive,
			CustomLeaderboardCriteriaType.GigapedesAlive => ToolsApi.CustomLeaderboardCriteriaType.GigapedesAlive,
			CustomLeaderboardCriteriaType.GhostpedesAlive => ToolsApi.CustomLeaderboardCriteriaType.GhostpedesAlive,
			CustomLeaderboardCriteriaType.Spider1sAlive => ToolsApi.CustomLeaderboardCriteriaType.Spider1sAlive,
			CustomLeaderboardCriteriaType.Spider2sAlive => ToolsApi.CustomLeaderboardCriteriaType.Spider2sAlive,
			CustomLeaderboardCriteriaType.LeviathansAlive => ToolsApi.CustomLeaderboardCriteriaType.LeviathansAlive,
			CustomLeaderboardCriteriaType.OrbsAlive => ToolsApi.CustomLeaderboardCriteriaType.OrbsAlive,
			CustomLeaderboardCriteriaType.ThornsAlive => ToolsApi.CustomLeaderboardCriteriaType.ThornsAlive,
			CustomLeaderboardCriteriaType.DeathType => ToolsApi.CustomLeaderboardCriteriaType.DeathType,
			CustomLeaderboardCriteriaType.Time => ToolsApi.CustomLeaderboardCriteriaType.Time,
			CustomLeaderboardCriteriaType.LevelUpTime2 => ToolsApi.CustomLeaderboardCriteriaType.LevelUpTime2,
			CustomLeaderboardCriteriaType.LevelUpTime3 => ToolsApi.CustomLeaderboardCriteriaType.LevelUpTime3,
			CustomLeaderboardCriteriaType.LevelUpTime4 => ToolsApi.CustomLeaderboardCriteriaType.LevelUpTime4,
			CustomLeaderboardCriteriaType.EnemiesAlive => ToolsApi.CustomLeaderboardCriteriaType.EnemiesAlive,
			_ => throw new UnreachableException(),
		};
	}

	private static ToolsApi.CustomLeaderboardCriteriaOperator ToAppApi(this CustomLeaderboardCriteriaOperator @operator)
	{
		return @operator switch
		{
			CustomLeaderboardCriteriaOperator.Any => ToolsApi.CustomLeaderboardCriteriaOperator.Any,
			CustomLeaderboardCriteriaOperator.Equal => ToolsApi.CustomLeaderboardCriteriaOperator.Equal,
			CustomLeaderboardCriteriaOperator.LessThan => ToolsApi.CustomLeaderboardCriteriaOperator.LessThan,
			CustomLeaderboardCriteriaOperator.GreaterThan => ToolsApi.CustomLeaderboardCriteriaOperator.GreaterThan,
			CustomLeaderboardCriteriaOperator.LessThanOrEqual => ToolsApi.CustomLeaderboardCriteriaOperator.LessThanOrEqual,
			CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => ToolsApi.CustomLeaderboardCriteriaOperator.GreaterThanOrEqual,
			CustomLeaderboardCriteriaOperator.Modulo => ToolsApi.CustomLeaderboardCriteriaOperator.Modulo,
			CustomLeaderboardCriteriaOperator.NotEqual => ToolsApi.CustomLeaderboardCriteriaOperator.NotEqual,
			_ => throw new UnreachableException(),
		};
	}

	public static ToolsApi.SpawnsetGameMode ToAppApi(this SpawnsetGameMode dagger)
	{
		return dagger switch
		{
			SpawnsetGameMode.Survival => ToolsApi.SpawnsetGameMode.Survival,
			SpawnsetGameMode.TimeAttack => ToolsApi.SpawnsetGameMode.TimeAttack,
			SpawnsetGameMode.Race => ToolsApi.SpawnsetGameMode.Race,
			_ => throw new UnreachableException(),
		};
	}

	public static ToolsApi.CustomLeaderboardRankSorting ToAppApi(this CustomLeaderboardRankSorting dagger)
	{
		return dagger switch
		{
			CustomLeaderboardRankSorting.TimeAsc => ToolsApi.CustomLeaderboardRankSorting.TimeAsc,
			CustomLeaderboardRankSorting.GemsCollectedAsc => ToolsApi.CustomLeaderboardRankSorting.GemsCollectedAsc,
			CustomLeaderboardRankSorting.GemsDespawnedAsc => ToolsApi.CustomLeaderboardRankSorting.GemsDespawnedAsc,
			CustomLeaderboardRankSorting.GemsEatenAsc => ToolsApi.CustomLeaderboardRankSorting.GemsEatenAsc,
			CustomLeaderboardRankSorting.EnemiesKilledAsc => ToolsApi.CustomLeaderboardRankSorting.EnemiesKilledAsc,
			CustomLeaderboardRankSorting.EnemiesAliveAsc => ToolsApi.CustomLeaderboardRankSorting.EnemiesAliveAsc,
			CustomLeaderboardRankSorting.HomingStoredAsc => ToolsApi.CustomLeaderboardRankSorting.HomingStoredAsc,
			CustomLeaderboardRankSorting.HomingEatenAsc => ToolsApi.CustomLeaderboardRankSorting.HomingEatenAsc,

			CustomLeaderboardRankSorting.TimeDesc => ToolsApi.CustomLeaderboardRankSorting.TimeDesc,
			CustomLeaderboardRankSorting.GemsCollectedDesc => ToolsApi.CustomLeaderboardRankSorting.GemsCollectedDesc,
			CustomLeaderboardRankSorting.GemsDespawnedDesc => ToolsApi.CustomLeaderboardRankSorting.GemsDespawnedDesc,
			CustomLeaderboardRankSorting.GemsEatenDesc => ToolsApi.CustomLeaderboardRankSorting.GemsEatenDesc,
			CustomLeaderboardRankSorting.EnemiesKilledDesc => ToolsApi.CustomLeaderboardRankSorting.EnemiesKilledDesc,
			CustomLeaderboardRankSorting.EnemiesAliveDesc => ToolsApi.CustomLeaderboardRankSorting.EnemiesAliveDesc,
			CustomLeaderboardRankSorting.HomingStoredDesc => ToolsApi.CustomLeaderboardRankSorting.HomingStoredDesc,
			CustomLeaderboardRankSorting.HomingEatenDesc => ToolsApi.CustomLeaderboardRankSorting.HomingEatenDesc,

			_ => throw new UnreachableException(),
		};
	}
}
