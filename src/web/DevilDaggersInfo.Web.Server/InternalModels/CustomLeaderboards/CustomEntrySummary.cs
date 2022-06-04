using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;

public class CustomEntrySummary : ISortableCustomEntry
{
	public int CustomLeaderboardId { get; init; }

	public int PlayerId { get; init; }

	public string PlayerName { get; init; } = string.Empty;

	public int Time { get; init; }

	public DateTime SubmitDate { get; init; }
}
