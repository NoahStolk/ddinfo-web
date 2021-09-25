namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Donations;

public class GetDonation
{
	public int Amount { get; init; }

	public int ConvertedEuroCentsReceived { get; init; }

	public Currency Currency { get; init; }

	public bool IsRefunded { get; init; }
}
