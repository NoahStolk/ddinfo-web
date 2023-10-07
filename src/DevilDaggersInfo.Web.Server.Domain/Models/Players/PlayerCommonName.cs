namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerCommonName
{
	public required int Id { get; init; }

	public required string CommonName { get; init; }
}
