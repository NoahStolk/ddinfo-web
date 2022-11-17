namespace DevilDaggersInfo.Api.Main.Players;

public record GetPlayerForSettings
{
	public int Id { get; init; }

	public string? CountryCode { get; init; }

	public required GetPlayerSettings Settings { get; init; }
}
