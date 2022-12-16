using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.Donations;

public record GetDonationForOverview : IAdminOverviewGetDto
{
	public required int Id { get; init; }

	public required string PlayerName { get; init; }

	public required int Amount { get; init; }

	public required Currency Currency { get; init; }

	public required int ConvertedEuroCentsReceived { get; init; }

	public required DateTime DateReceived { get; init; }

	public required string? Note { get; init; }

	public required bool IsRefunded { get; init; }
}
