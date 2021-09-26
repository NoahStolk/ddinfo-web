namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public class GetPlayerForLeaderboard : IGetDto<int>
{
	public int Id { get; init; }

	public bool IsBanned { get; init; }

	public string? BanDescription { get; init; }

	public string? CountryCode { get; init; }
}
