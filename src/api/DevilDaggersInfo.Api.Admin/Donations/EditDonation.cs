using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Donations;

public record EditDonation
{
	public required int PlayerId { get; init; }

	public required int Amount { get; init; }

	public required Currency Currency { get; init; }

	public required int ConvertedEuroCentsReceived { get; init; }

	[StringLength(64)]
	public required string? Note { get; init; }

	public required bool IsRefunded { get; init; }
}
