namespace DevilDaggersInfo.Web.Server.Domain.Configuration;

public record MySqlOptions
{
	public required string ConnectionString { get; init; }
}
