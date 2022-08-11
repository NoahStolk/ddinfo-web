using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Donations;

public record AddDonation
{
	public int PlayerId { get; set; }

	public int Amount { get; set; }

	public Currency Currency { get; set; }

	public int ConvertedEuroCentsReceived { get; set; }

	[StringLength(64)]
	public string? Note { get; set; }

	public bool IsRefunded { get; set; }
}
