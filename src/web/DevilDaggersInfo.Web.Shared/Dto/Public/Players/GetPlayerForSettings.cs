namespace DevilDaggersInfo.Web.Shared.Dto.Public.Players;

public record GetPlayerForSettings
{
	public int Id { get; init; }

	public string? CountryCode { get; init; }

	public GetPlayerSettings Settings { get; init; } = null!;
}
