namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public interface IDaggerStatCustomEntry
{
	int Time { get; }

	int GemsCollected { get; }

	int GemsDespawned { get; }

	int EnemiesKilled { get; }

	int HomingStored { get; }
}
