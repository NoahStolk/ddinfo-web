namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public interface ISortableCustomEntry
{
	int Time { get; }

	int GemsCollected { get; }

	int GemsDespawned { get; }

	int GemsEaten { get; }

	int EnemiesKilled { get; }

	int EnemiesAlive { get; }

	int HomingStored { get; }

	int HomingEaten { get; }

	DateTime SubmitDate { get; }
}
