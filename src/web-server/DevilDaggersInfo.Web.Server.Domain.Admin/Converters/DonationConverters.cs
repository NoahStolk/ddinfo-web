using DevilDaggersInfo.Api.Admin.Donations;
using DevilDaggersInfo.Web.Server.Domain.Entities;

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

	private static Currency ToAdminApi(this Types.Web.Currency currency) => currency switch
	{
		Types.Web.Currency.Eur => Currency.Eur,
		Types.Web.Currency.Usd => Currency.Usd,
		Types.Web.Currency.Aud => Currency.Aud,
		Types.Web.Currency.Gbp => Currency.Gbp,
		Types.Web.Currency.Sgd => Currency.Sgd,
		Types.Web.Currency.Rub => Currency.Rub,
		_ => throw new ArgumentOutOfRangeException(nameof(currency), currency, null),
	};

	public static Types.Web.Currency ToDomain(this Currency currency) => currency switch
	{
		Currency.Eur => Types.Web.Currency.Eur,
		Currency.Usd => Types.Web.Currency.Usd,
		Currency.Aud => Types.Web.Currency.Aud,
		Currency.Gbp => Types.Web.Currency.Gbp,
		Currency.Sgd => Types.Web.Currency.Sgd,
		Currency.Rub => Types.Web.Currency.Rub,
		_ => throw new ArgumentOutOfRangeException(nameof(currency), currency, null),
	};
}
