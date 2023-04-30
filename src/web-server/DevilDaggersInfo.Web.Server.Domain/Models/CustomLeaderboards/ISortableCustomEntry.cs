namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public interface ISortableCustomEntry
{
	int Time { get; }

	int GemsCollected { get; }

	int EnemiesKilled { get; }

	int HomingStored { get; }

	DateTime SubmitDate { get; }
}
