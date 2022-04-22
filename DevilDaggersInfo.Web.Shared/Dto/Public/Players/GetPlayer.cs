namespace DevilDaggersInfo.Web.Shared.Dto.Public.Players;

public record GetPlayer
{
	public int Id { get; init; }

	public bool IsBanned { get; init; }

	public string? BanDescription { get; init; }

	public bool IsPublicDonator { get; init; }

	public string? CountryCode { get; init; }

	public GetPlayerSettings? Settings { get; init; }
}
