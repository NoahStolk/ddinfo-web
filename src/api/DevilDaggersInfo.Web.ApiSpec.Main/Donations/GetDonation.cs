namespace DevilDaggersInfo.Api.Main.Donations;

public record GetDonation
{
	public required int Amount { get; init; }

	public required int ConvertedEuroCentsReceived { get; init; }

	public required Currency Currency { get; init; }

	public required bool IsRefunded { get; init; }
}
