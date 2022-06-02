using DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;
using DdclApi = DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.Ddcl;

public static class DomainToApi
{
	public static DdclApi.GetUploadSuccess ToDdclApi(this UploadResponse uploadResponse) => new()
	{
		Category = uploadResponse.Category.ToDdclApi(),
		DaggersFiredState = uploadResponse.DaggersFiredState.ToDdclApi(),
		DaggersHitState = uploadResponse.DaggersHitState.ToDdclApi(),
		EnemiesAliveState = uploadResponse.EnemiesAliveState.ToDdclApi(),
		EnemiesKilledState = uploadResponse.EnemiesKilledState.ToDdclApi(),
		Entries = uploadResponse.Entries.ConvertAll(e => e.ToDdclApi()),
		GemsCollectedState = uploadResponse.GemsCollectedState.ToDdclApi(),
		GemsDespawnedState = uploadResponse.GemsDespawnedState.ToDdclApi(),
		GemsEatenState = uploadResponse.GemsEatenState.ToDdclApi(),
		GemsTotalState = uploadResponse.GemsTotalState.ToDdclApi(),
		HomingEatenState = uploadResponse.HomingEatenState.ToDdclApi(),
		HomingStoredState = uploadResponse.HomingStoredState.ToDdclApi(),
		IsHighscore = uploadResponse.IsHighscore,
		IsNewPlayerOnThisLeaderboard = uploadResponse.IsNewPlayerOnThisLeaderboard,
		Leaderboard = uploadResponse.Leaderboard.ToDdclApi(),
		LevelUpTime2State = uploadResponse.LevelUpTime2State.ToDdclApi(),
		LevelUpTime3State = uploadResponse.LevelUpTime3State.ToDdclApi(),
		LevelUpTime4State = uploadResponse.LevelUpTime4State.ToDdclApi(),
		Message = uploadResponse.Message,
		RankState = uploadResponse.RankState.ToDdclApi(),
		TimeState = uploadResponse.TimeState.ToDdclApi(),
		TotalPlayers = uploadResponse.TotalPlayers,
	};

	private static DdclApi.GetCustomLeaderboardDdcl ToDdclApi(this CustomLeaderboardSummary customLeaderboard) => new()
	{
		Category = customLeaderboard.Category.ToDdclApi(),
		Daggers = customLeaderboard.Daggers?.ToDdclApi(),
		IsAscending = customLeaderboard.Category.IsAscending(),
		SpawnsetName = customLeaderboard.SpawnsetName,
	};

	private static DdclApi.GetCustomLeaderboardDaggersDdcl ToDdclApi(this CustomLeaderboardDaggers customLeaderboardDaggers) => new()
	{
		Bronze = customLeaderboardDaggers.Bronze.ToSecondsTime(),
		Silver = customLeaderboardDaggers.Silver.ToSecondsTime(),
		Golden = customLeaderboardDaggers.Golden.ToSecondsTime(),
		Devil = customLeaderboardDaggers.Devil.ToSecondsTime(),
		Leviathan = customLeaderboardDaggers.Leviathan.ToSecondsTime(),
	};

	private static DdclApi.GetCustomEntryDdcl ToDdclApi(this CustomEntryWithReplay customEntry) => new()
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

	private static DdclApi.CustomLeaderboardCategory ToDdclApi(this CustomLeaderboardCategory customLeaderboardCategory) => customLeaderboardCategory switch
	{
		CustomLeaderboardCategory.Survival => DdclApi.CustomLeaderboardCategory.Survival,
		CustomLeaderboardCategory.TimeAttack => DdclApi.CustomLeaderboardCategory.TimeAttack,
		CustomLeaderboardCategory.Speedrun => DdclApi.CustomLeaderboardCategory.Speedrun,
		CustomLeaderboardCategory.Race => DdclApi.CustomLeaderboardCategory.Race,
		CustomLeaderboardCategory.Pacifist => DdclApi.CustomLeaderboardCategory.Pacifist,
		_ => throw new InvalidOperationException($"Cannot convert custom leaderboard category '{customLeaderboardCategory}' to a DDCL API model."),
	};

	private static DdclApi.GetScoreState<T> ToDdclApi<T>(this UploadResponseScoreState<T> scoreState)
		where T : struct
		=> new(scoreState.Value, scoreState.ValueDifference);
}
