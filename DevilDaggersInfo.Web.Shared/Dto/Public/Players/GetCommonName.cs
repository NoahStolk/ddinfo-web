namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public record GetCommonName
{
	public int Id { get; set; }

	public string CommonName { get; set; } = null!;
}
