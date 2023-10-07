namespace DevilDaggersInfo.Api.DdLive.Players;

public record GetCommonName
{
	public required int Id { get; init; }

	public required string CommonName { get; init; }
}
