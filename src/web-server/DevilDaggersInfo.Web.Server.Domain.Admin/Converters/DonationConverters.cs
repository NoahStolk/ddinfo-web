using DevilDaggersInfo.Api.Admin.Donations;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class DonationConverters
{
	// ! Navigation property.
	public static GetDonationForOverview ToGetDonationForOverview(this DonationEntity donation) => new()
	{
		Id = donation.Id,
		Amount = donation.Amount,
		ConvertedEuroCentsReceived = donation.ConvertedEuroCentsReceived,
		Currency = donation.Currency.ToAdminApi(),
		DateReceived = donation.DateReceived,
		IsRefunded = donation.IsRefunded,
		Note = donation.Note,
		PlayerName = donation.Player!.PlayerName,
	};

	public static GetDonation ToGetDonation(this DonationEntity donation) => new()
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

	public static Entities.Enums.Currency ToDomain(this Currency currency) => currency switch
	{
		Currency.Eur => Entities.Enums.Currency.Eur,
		Currency.Usd => Entities.Enums.Currency.Usd,
		Currency.Aud => Entities.Enums.Currency.Aud,
		Currency.Gbp => Entities.Enums.Currency.Gbp,
		Currency.Sgd => Entities.Enums.Currency.Sgd,
		Currency.Rub => Entities.Enums.Currency.Rub,
		_ => throw new UnreachableException(),
	};
}
