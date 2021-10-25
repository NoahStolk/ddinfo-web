namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public class GetNumberOfCustomLeaderboards
{
	public Dictionary<CustomLeaderboardCategory, int> CountPerCategory { get; init; } = new();
}
