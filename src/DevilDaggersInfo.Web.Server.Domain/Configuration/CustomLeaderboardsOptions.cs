namespace DevilDaggersInfo.Web.Server.Domain.Configuration;

public sealed record CustomLeaderboardsOptions
{
	public required string InitializationVector { get; init; }

	public required string Password { get; init; }

	public required string Salt { get; init; }
}
