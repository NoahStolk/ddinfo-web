namespace DevilDaggersInfo.Web.Server.Domain.Configuration;

public record AuthenticationOptions
{
	public required string JwtKey { get; init; }
}
