namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities.Views;

public record CustomEntryDdclResult(
	int Time,
	DateTime SubmitDate,
	int Id,
	int CustomLeaderboardId,
	int PlayerId,
	string PlayerName,
	int GemsCollected,
	int GemsDespawned,
	int GemsEaten,
	int GemsTotal,
	int EnemiesAlive,
	int EnemiesKilled,
	int HomingStored,
	int HomingEaten,
	byte DeathType,
	int DaggersFired,
	int DaggersHit,
	int LevelUpTime2,
	int LevelUpTime3,
	int LevelUpTime4,
	string ClientVersion) : ISortableCustomEntry;
