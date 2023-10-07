namespace DevilDaggersInfo.Web.ApiSpec.Main.Authentication;

public record AuthenticationRequest
{
	public required string Jwt { get; init; }
}
