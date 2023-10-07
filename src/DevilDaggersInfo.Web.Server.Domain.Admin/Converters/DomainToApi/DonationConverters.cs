using DevilDaggersInfo.Web.ApiSpec.Admin.Donations;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;

public static class DonationConverters
{
	public static GetDonationForOverview ToAdminApiOverview(this DonationEntity donation)
	{
		if (donation.Player == null)
			throw new InvalidOperationException("Player is not included.");

		return new()
		{
			Id = donation.Id,
			Amount = donation.Amount,
			ConvertedEuroCentsReceived = donation.ConvertedEuroCentsReceived,
			Currency = donation.Currency.ToAdminApi(),
			DateReceived = donation.DateReceived,
			IsRefunded = donation.IsRefunded,
			Note = donation.Note,
			PlayerName = donation.Player.PlayerName,
		};
	}

	public static GetDonation ToAdminApi(this DonationEntity donation) => new()
	{
		Id = donation.Id,
		Amount = donation.Amount,
		ConvertedEuroCentsReceived = donation.ConvertedEuroCentsReceived,
		Currency = donation.Currency.ToAdminApi(),
		IsRefunded = donation.IsRefunded,
		Note = donation.Note,
		PlayerId = donation.PlayerId,
	};

	private static Currency ToAdminApi(this Entities.Enums.Currency currency) => currency switch
	{
		Entities.Enums.Currency.Eur => Currency.Eur,
		Entities.Enums.Currency.Usd => Currency.Usd,
		Entities.Enums.Currency.Aud => Currency.Aud,
		Entities.Enums.Currency.Gbp => Currency.Gbp,
		Entities.Enums.Currency.Sgd => Currency.Sgd,
		Entities.Enums.Currency.Rub => Currency.Rub,
		_ => throw new UnreachableException(),
	};
}
