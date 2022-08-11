using DevilDaggersInfo.Web.Server.Domain.Entities;
using AdminApi = DevilDaggersInfo.Api.Admin.Donations;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class DonationConverters
{
	public static AdminApi.GetDonationForOverview ToGetDonationForOverview(this DonationEntity donation) => new()
	{
		Id = donation.Id,
		Amount = donation.Amount,
		ConvertedEuroCentsReceived = donation.ConvertedEuroCentsReceived,
		Currency = donation.Currency,
		DateReceived = donation.DateReceived,
		IsRefunded = donation.IsRefunded,
		Note = donation.Note,
		PlayerName = donation.Player.PlayerName,
	};

	public static AdminApi.GetDonation ToGetDonation(this DonationEntity donation) => new()
	{
		Id = donation.Id,
		Amount = donation.Amount,
		ConvertedEuroCentsReceived = donation.ConvertedEuroCentsReceived,
		Currency = donation.Currency,
		IsRefunded = donation.IsRefunded,
		Note = donation.Note,
		PlayerId = donation.PlayerId,
	};
}
