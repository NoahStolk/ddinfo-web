namespace DevilDaggersInfo.Web.ApiSpec.Main.Authentication;

public sealed record AuthenticationRequest
{
	public required string Jwt { get; init; }
}
