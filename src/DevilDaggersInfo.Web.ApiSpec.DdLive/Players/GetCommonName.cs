namespace DevilDaggersInfo.Web.ApiSpec.DdLive.Players;

public sealed record GetCommonName
{
	public required int Id { get; init; }

	public required string CommonName { get; init; }
}
