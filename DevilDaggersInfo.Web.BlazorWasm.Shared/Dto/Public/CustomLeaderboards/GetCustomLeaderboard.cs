using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public class GetCustomLeaderboard
{
	public int SpawnsetId { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public string SpawnsetAuthorName { get; init; } = null!;

	public string? SpawnsetHtmlDescription { get; init; }

	public GetCustomLeaderboardDaggers? Daggers { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime DateCreated { get; init; }

	public int SubmitCount { get; init; }

	public bool IsFeatured { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public List<GetCustomEntry> CustomEntries { get; set; } = new();
}
