using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using AppApi = DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;

public static class CustomLeaderboardConverters
{
	public static AppApi.GetUploadResponse ToAppApi(this UploadResponse uploadResponse)
	{
		if (uploadResponse.Success != null)
			return uploadResponse.Success.ToAppApi();

		if (uploadResponse.Rejection != null)
			return uploadResponse.Rejection.ToAppApi();

		throw new InvalidOperationException("Invalid upload response. Both Success and Rejection are null.");
	}

	private static AppApi.GetUploadResponse ToAppApi(this UploadCriteriaRejection uploadCriteriaRejection)
	{
		return new()
		{
			CriteriaRejection = new()
			{
				ActualValue = uploadCriteriaRejection.ActualValue,
				ExpectedValue = uploadCriteriaRejection.ExpectedValue,
				CriteriaName = uploadCriteriaRejection.CriteriaName,
				CriteriaOperator = uploadCriteriaRejection.CriteriaOperator,
			},
		};
	}

	private static AppApi.GetUploadResponse ToAppApi(this SuccessfulUploadResponse successfulUploadResponse)
	{
		List<AppApi.GetCustomEntry> sortedEntries = successfulUploadResponse.SortedEntries.ConvertAll(ce => ce.ToAppApi());

		return successfulUploadResponse.SubmissionType switch
		{
			SubmissionType.FirstScore => new()
			{
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
				IsAscending = successfulUploadResponse.Leaderboard.Category.IsAscending(),
			},
			SubmissionType.NewHighscore => new()
			{
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
				IsAscending = successfulUploadResponse.Leaderboard.Category.IsAscending(),
			},
			_ => new()
			{
				NoHighscore = new(),
				NewSortedEntries = sortedEntries,
				IsAscending = successfulUploadResponse.Leaderboard.Category.IsAscending(),
			},
		};
	}

	public static AppApi.GetCustomLeaderboard ToAppApi(this SortedCustomLeaderboard sortedCustomLeaderboard) => new()
	{
		Category = sortedCustomLeaderboard.Category,
		Criteria = sortedCustomLeaderboard.Criteria.ConvertAll(c => c.ToAppApi()),
		Daggers = sortedCustomLeaderboard.Daggers?.ToAppApi(),
		IsAscending = sortedCustomLeaderboard.Category.IsAscending(),
		SortedEntries = sortedCustomLeaderboard.CustomEntries.ConvertAll(ce => ce.ToAppApi()),
		SpawnsetName = sortedCustomLeaderboard.SpawnsetName,
	};

	public static AppApi.GetCustomLeaderboardForOverview ToAppApi(this CustomLeaderboardOverview customLeaderboard) => new()
	{
		Category = customLeaderboard.Category,
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
		Type = customLeaderboardCriteria.Type,
		Expression = customLeaderboardCriteria.Expression,
		Operator = customLeaderboardCriteria.Operator,
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
		Dagger = customLeaderboardOverviewSelectedPlayerStats.Dagger,
		Rank = customLeaderboardOverviewSelectedPlayerStats.Rank,
		Time = customLeaderboardOverviewSelectedPlayerStats.Time.ToSecondsTime(),
		NextDagger = customLeaderboardOverviewSelectedPlayerStats.NextDagger == null ? null : new()
		{
			Dagger = customLeaderboardOverviewSelectedPlayerStats.NextDagger.Dagger,
			Time = customLeaderboardOverviewSelectedPlayerStats.NextDagger.Time.ToSecondsTime(),
		},
	};

	private static AppApi.GetCustomLeaderboardWorldRecord ToAppApi(this CustomLeaderboardOverviewWorldRecord customLeaderboardOverviewWorldRecord) => new()
	{
		Dagger = customLeaderboardOverviewWorldRecord.Dagger,
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
		CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger,
	};
}
