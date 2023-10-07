namespace DevilDaggersInfo.Web.ApiSpec.Admin.Donations;

public record GetDonation
{
	public required int Id { get; init; }

	public required int PlayerId { get; init; }

	public required int Amount { get; init; }

	public required Currency Currency { get; init; }

	public required int ConvertedEuroCentsReceived { get; init; }

	public required string? Note { get; init; }

	public required bool IsRefunded { get; init; }
}
