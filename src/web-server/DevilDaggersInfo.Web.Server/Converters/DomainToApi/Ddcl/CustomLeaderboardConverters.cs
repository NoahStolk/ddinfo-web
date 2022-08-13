using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DdclApi = DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddcl;

public static class CustomLeaderboardConverters
{
	public static DdclApi.GetUploadSuccess ToDdclApi(this UploadResponse uploadResponse) => new()
	{
		Category = uploadResponse.Leaderboard.Category,
		DaggersFiredState = uploadResponse.DaggersFiredState.ToDdclApi(),
		DaggersHitState = uploadResponse.DaggersHitState.ToDdclApi(),
		EnemiesAliveState = uploadResponse.EnemiesAliveState.ToDdclApi(),
		EnemiesKilledState = uploadResponse.EnemiesKilledState.ToDdclApi(),
		Entries = uploadResponse.SortedEntries.ConvertAll(e => e.ToDdclApiObsolete()),
		GemsCollectedState = uploadResponse.GemsCollectedState.ToDdclApi(),
		GemsDespawnedState = uploadResponse.GemsDespawnedState.ToDdclApi(),
		GemsEatenState = uploadResponse.GemsEatenState.ToDdclApi(),
		GemsTotalState = uploadResponse.GemsTotalState.ToDdclApi(),
		HomingEatenState = uploadResponse.HomingEatenState.ToDdclApi(),
		HomingStoredState = uploadResponse.HomingStoredState.ToDdclApi(),
		IsHighscore = uploadResponse.SubmissionType == SubmissionType.NewHighscore,
		IsNewPlayerOnThisLeaderboard = uploadResponse.SubmissionType == SubmissionType.FirstScore,
		Leaderboard = uploadResponse.Leaderboard.ToDdclApi(),
		LevelUpTime2State = uploadResponse.LevelUpTime2State.ToDdclApi(),
		LevelUpTime3State = uploadResponse.LevelUpTime3State.ToDdclApi(),
		LevelUpTime4State = uploadResponse.LevelUpTime4State.ToDdclApi(),
		Message = uploadResponse.Message,
		RankState = uploadResponse.RankState.ToDdclApi(),
		SpawnsetName = uploadResponse.Leaderboard.SpawnsetName,
		SubmissionType = uploadResponse.SubmissionType.ToDdclApi(),
		TimeState = uploadResponse.TimeState.ToDdclApi(),
		TotalPlayers = uploadResponse.SortedEntries.Count,
	};

	public static DdclApi.GetCustomLeaderboard ToDdclApi(this SortedCustomLeaderboard sortedCustomLeaderboard) => new()
	{
		Category = sortedCustomLeaderboard.Category,
		Daggers = sortedCustomLeaderboard.Daggers?.ToDdclApi(),
		IsAscending = sortedCustomLeaderboard.Category.IsAscending(),
		SortedEntries = sortedCustomLeaderboard.CustomEntries.ConvertAll(ce => ce.ToDdclApi()),
		SpawnsetName = sortedCustomLeaderboard.SpawnsetName,
	};

	public static DdclApi.GetCustomLeaderboardForOverview ToDdclApi(this CustomLeaderboardOverview customLeaderboard) => new()
	{
		Category = customLeaderboard.Category,
		Daggers = customLeaderboard.Daggers?.ToDdclApi(),
		PlayerCount = customLeaderboard.PlayerCount,
		SelectedPlayerStats = customLeaderboard.SelectedPlayerStats?.ToDdclApi(),
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetName = customLeaderboard.SpawnsetName,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		WorldRecord = customLeaderboard.WorldRecord?.ToDdclApi(),
	};

	private static DdclApi.GetCustomLeaderboardDdcl ToDdclApi(this CustomLeaderboardSummary customLeaderboard) => new()
	{
		Category = customLeaderboard.Category,
		Daggers = customLeaderboard.Daggers?.ToDdclApi(),
		IsAscending = customLeaderboard.Category.IsAscending(),
		SpawnsetName = customLeaderboard.SpawnsetName,
	};

	public static DdclApi.GetCustomLeaderboardDaggersDdcl ToDdclApi(this CustomLeaderboardDaggers customLeaderboardDaggers) => new()
	{
		Bronze = customLeaderboardDaggers.Bronze.ToSecondsTime(),
		Silver = customLeaderboardDaggers.Silver.ToSecondsTime(),
		Golden = customLeaderboardDaggers.Golden.ToSecondsTime(),
		Devil = customLeaderboardDaggers.Devil.ToSecondsTime(),
		Leviathan = customLeaderboardDaggers.Leviathan.ToSecondsTime(),
	};

	public static DdclApi.GetCustomLeaderboardSelectedPlayerStats ToDdclApi(this CustomLeaderboardOverviewSelectedPlayerStats customLeaderboardOverviewSelectedPlayerStats) => new()
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

	public static DdclApi.GetCustomLeaderboardWorldRecord ToDdclApi(this CustomLeaderboardOverviewWorldRecord customLeaderboardOverviewWorldRecord) => new()
	{
		Dagger = customLeaderboardOverviewWorldRecord.Dagger,
		Time = customLeaderboardOverviewWorldRecord.Time.ToSecondsTime(),
	};

	private static DdclApi.SubmissionType ToDdclApi(this SubmissionType submissionType) => submissionType switch
	{
		SubmissionType.NoHighscore => DdclApi.SubmissionType.NoHighscore,
		SubmissionType.NewHighscore => DdclApi.SubmissionType.NewHighscore,
		SubmissionType.FirstScore => DdclApi.SubmissionType.FirstScore,
		_ => throw new InvalidEnumConversionException(submissionType),
	};

	private static DdclApi.GetScoreState<T> ToDdclApi<T>(this UploadResponseScoreState<T> scoreState)
		where T : struct
		=> new(scoreState.Value, scoreState.ValueDifference);

	private static DdclApi.GetCustomEntry ToDdclApi(this CustomEntry customEntry) => new()
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

	private static DdclApi.GetCustomEntryDdcl ToDdclApiObsolete(this CustomEntry customEntry) => new()
	{
		ClientVersion = customEntry.ClientVersion,
		DaggersFired = customEntry.DaggersFired,
		DaggersHit = customEntry.DaggersHit,
		DeathType = customEntry.DeathType,
		EnemiesAlive = customEntry.EnemiesAlive,
		EnemiesKilled = customEntry.EnemiesKilled,
		GemsCollected = customEntry.GemsCollected,
		GemsDespawned = customEntry.GemsDespawned ?? 0,
		GemsEaten = customEntry.GemsEaten ?? 0,
		GemsTotal = customEntry.GemsTotal ?? 0,
		HasReplay = customEntry.HasReplay,
		HomingEaten = customEntry.HomingEaten ?? 0,
		HomingStored = customEntry.HomingStored,
		Id = customEntry.Id,
		LevelUpTime2InSeconds = customEntry.LevelUpTime2.ToSecondsTime(),
		LevelUpTime3InSeconds = customEntry.LevelUpTime3.ToSecondsTime(),
		LevelUpTime4InSeconds = customEntry.LevelUpTime4.ToSecondsTime(),
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.PlayerName,
		SubmitDate = customEntry.SubmitDate,
		TimeInSeconds = customEntry.Time.ToSecondsTime(),
	};
}
