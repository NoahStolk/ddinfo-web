namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Donations;

public class GetDonator
{
	public bool HideDonations { get; init; }

	public int PlayerId { get; init; }

	public string PlayerName { get; init; } = null!;

	public List<GetDonation> Donations { get; init; } = new();
}
