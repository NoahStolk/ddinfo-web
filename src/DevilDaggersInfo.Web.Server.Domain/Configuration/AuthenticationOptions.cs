namespace DevilDaggersInfo.Web.Server.Domain.Configuration;

public sealed record AuthenticationOptions
{
	public required string JwtKey { get; init; }
}
