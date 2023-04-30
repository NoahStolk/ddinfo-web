namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomEntrySummary : ISortableCustomEntry, IDaggerStatCustomEntry
{
	public required int CustomLeaderboardId { get; init; }

	public required int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required int Time { get; init; }

	public required DateTime SubmitDate { get; init; }
}
