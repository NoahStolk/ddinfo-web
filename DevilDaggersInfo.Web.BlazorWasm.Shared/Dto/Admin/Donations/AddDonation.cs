namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Donations;

public class AddDonation
{
	public int PlayerId { get; init; }

	public int Amount { get; init; }

	public Currency Currency { get; init; }

	public int ConvertedEuroCentsReceived { get; init; }

	[StringLength(64)]
	public string? Note { get; init; }

	public bool IsRefunded { get; init; }
}
