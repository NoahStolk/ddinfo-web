namespace DevilDaggersInfo.Api.Main.Donations;

public record GetDonation
{
	public int Amount { get; init; }

	public int ConvertedEuroCentsReceived { get; init; }

	public Currency Currency { get; init; }

	public bool IsRefunded { get; init; }
}
