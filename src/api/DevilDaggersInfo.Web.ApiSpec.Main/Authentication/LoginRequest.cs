namespace DevilDaggersInfo.Web.ApiSpec.Main.Authentication;

public record LoginRequest
{
	public required string Name { get; init; }

	public required string Password { get; init; }
}
