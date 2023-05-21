namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomEntrySummary : ISortableCustomEntry, IDaggerStatCustomEntry
{
	public required int CustomLeaderboardId { get; init; }

	public required int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required int Time { get; init; }

	public required int GemsCollected { get; init; }

	public required int GemsDespawned { get; init; }

	public required int EnemiesKilled { get; init; }

	public required int HomingStored { get; init; }

	public required DateTime SubmitDate { get; init; }
}
