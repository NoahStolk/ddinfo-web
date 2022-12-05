using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using AppApi = DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;

public static class CustomLeaderboardConverters
{
	public static AppApi.GetUploadSuccess ToAppApi(this UploadResponse uploadResponse) => new()
	{
		DaggersFiredState = uploadResponse.DaggersFiredState.ToAppApi(),
		DaggersHitState = uploadResponse.DaggersHitState.ToAppApi(),
		EnemiesAliveState = uploadResponse.EnemiesAliveState.ToAppApi(),
		EnemiesKilledState = uploadResponse.EnemiesKilledState.ToAppApi(),
		GemsCollectedState = uploadResponse.GemsCollectedState.ToAppApi(),
		GemsDespawnedState = uploadResponse.GemsDespawnedState.ToAppApi(),
		GemsEatenState = uploadResponse.GemsEatenState.ToAppApi(),
		GemsTotalState = uploadResponse.GemsTotalState.ToAppApi(),
		HomingEatenState = uploadResponse.HomingEatenState.ToAppApi(),
		HomingStoredState = uploadResponse.HomingStoredState.ToAppApi(),
		LevelUpTime2State = uploadResponse.LevelUpTime2State.ToAppApi(),
		LevelUpTime3State = uploadResponse.LevelUpTime3State.ToAppApi(),
		LevelUpTime4State = uploadResponse.LevelUpTime4State.ToAppApi(),
		Message = uploadResponse.Message,
		RankState = uploadResponse.RankState.ToAppApi(),
		SubmissionType = uploadResponse.SubmissionType.ToAppApi(),
		TimeState = uploadResponse.TimeState.ToAppApi(),
	};

	public static AppApi.GetCustomLeaderboard ToAppApi(this SortedCustomLeaderboard sortedCustomLeaderboard) => new()
	{
		Category = sortedCustomLeaderboard.Category,
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
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		WorldRecord = customLeaderboard.WorldRecord?.ToAppApi(),
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

	private static AppApi.SubmissionType ToAppApi(this SubmissionType submissionType) => submissionType switch
	{
		SubmissionType.NoHighscore => AppApi.SubmissionType.NoHighscore,
		SubmissionType.NewHighscore => AppApi.SubmissionType.NewHighscore,
		SubmissionType.FirstScore => AppApi.SubmissionType.FirstScore,
		_ => throw new InvalidEnumConversionException(submissionType),
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
