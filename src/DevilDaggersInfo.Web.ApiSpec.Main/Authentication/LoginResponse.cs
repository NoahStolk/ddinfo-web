namespace DevilDaggersInfo.Web.ApiSpec.Main.Authentication;

public sealed record LoginResponse
{
	public required int Id { get; init; }

	public required string Name { get; init; }

	public required string Token { get; init; }
}
