namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Donations;

public class EditDonation
{
	public int PlayerId { get; set; }

	public int Amount { get; set; }

	public Currency Currency { get; set; }

	public int ConvertedEuroCentsReceived { get; set; }

	[StringLength(64)]
	public string? Note { get; set; }

	public bool IsRefunded { get; set; }
}
