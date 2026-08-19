namespace DevilDaggersInfo.Web.ApiSpec.Main.Players;

public sealed record GetPlayerForSettings
{
	public required int Id { get; init; }

	public required string? CountryCode { get; init; }

	public required GetPlayerSettings Settings { get; init; }
}
