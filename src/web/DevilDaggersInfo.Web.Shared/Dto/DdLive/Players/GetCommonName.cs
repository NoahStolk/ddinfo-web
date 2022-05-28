namespace DevilDaggersInfo.Web.Shared.Dto.DdLive.Players;

public record GetCommonName
{
	public int Id { get; set; }

	public string CommonName { get; set; } = null!;
}
