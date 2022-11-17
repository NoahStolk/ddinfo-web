namespace DevilDaggersInfo.Api.DdLive.Players;

public record GetCommonName
{
	public int Id { get; set; }

	public required string CommonName { get; set; }
}
