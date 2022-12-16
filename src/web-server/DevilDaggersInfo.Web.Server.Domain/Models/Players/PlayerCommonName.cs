namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerCommonName
{
	public int Id { get; init; }

	public required string CommonName { get; init; }
}
