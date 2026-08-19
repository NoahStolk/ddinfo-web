namespace DevilDaggersInfo.Web.ApiSpec.Clubber.Players;

public sealed record GetPlayerCountryCode
{
	public required string? CountryCode { get; init; }
}
