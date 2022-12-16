namespace DevilDaggersInfo.Api.Main.Authentication;

public record AuthenticationRequest
{
	public required string Jwt { get; init; }
}
