namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Donations;

public record GetDonator
{
	public int? PlayerId { get; init; }

	public string PlayerName { get; init; } = null!;

	public List<GetDonation> Donations { get; init; } = new();
}
