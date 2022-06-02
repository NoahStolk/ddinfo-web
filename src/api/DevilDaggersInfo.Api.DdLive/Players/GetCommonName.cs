namespace DevilDaggersInfo.Api.DdLive.Players;

public record GetCommonName
{
	public int Id { get; set; }

	public string CommonName { get; set; } = null!;
}
