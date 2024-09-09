using DevilDaggersInfo.Web.ApiSpec.Admin.Donations;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Donations;

public class EditDonationState : IStateObject<EditDonation>
{
	public int PlayerId { get; set; }

	public int Amount { get; set; }

	public Currency Currency { get; set; }

	public int ConvertedEuroCentsReceived { get; set; }

	[StringLength(64)]
	public string? Note { get; set; }

	public bool IsRefunded { get; set; }

	public EditDonation ToModel()
	{
		return new EditDonation
		{
			PlayerId = PlayerId,
			Amount = Amount,
			Currency = Currency,
			ConvertedEuroCentsReceived = ConvertedEuroCentsReceived,
			Note = Note,
			IsRefunded = IsRefunded,
		};
	}
}
