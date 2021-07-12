using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Donations;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters
{
	public static class DonationConverters
	{
		public static GetDonation ToGetDonation(this Donation donation) => new()
		{
			Id = donation.Id,
			Amount = donation.Amount,
			ConvertedEuroCentsReceived = donation.ConvertedEuroCentsReceived,
			Currency = donation.Currency,
			DateReceived = donation.DateReceived,
			IsRefunded = donation.IsRefunded,
			Note = donation.Note,
			PlayerId = donation.PlayerId,
			PlayerName = donation.Player.PlayerName,
		};
	}
}
