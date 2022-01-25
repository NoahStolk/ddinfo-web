namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Donations;

public class GetDonation : IGetDto
{
	public int Id { get; init; }

	public int PlayerId { get; init; }

	public int Amount { get; init; }

	public Currency Currency { get; init; }

	public int ConvertedEuroCentsReceived { get; init; }

	public string? Note { get; init; }

	public bool IsRefunded { get; init; }
}
