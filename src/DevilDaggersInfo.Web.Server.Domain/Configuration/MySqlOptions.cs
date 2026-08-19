namespace DevilDaggersInfo.Web.Server.Domain.Configuration;

public sealed record MySqlOptions
{
	public required string ConnectionString { get; init; }
}
