using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Entities.Enums;
using AdminApi = DevilDaggersInfo.Api.Admin.Donations;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;

public static class DonationConverters
{
	public static AdminApi.GetDonationForOverview ToGetDonationForOverview(this DonationEntity donation) => new()
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

	public static AdminApi.GetDonation ToGetDonation(this DonationEntity donation) => new()
	{
		Id = donation.Id,
		Amount = donation.Amount,
		ConvertedEuroCentsReceived = donation.ConvertedEuroCentsReceived,
		Currency = donation.Currency.ToAdminApi(),
		IsRefunded = donation.IsRefunded,
		Note = donation.Note,
		PlayerId = donation.PlayerId,
	};

	private static AdminApi.Currency ToAdminApi(this Currency currency) => currency switch
	{
		Currency.Eur => AdminApi.Currency.Eur,
		Currency.Usd => AdminApi.Currency.Usd,
		Currency.Aud => AdminApi.Currency.Aud,
		Currency.Gbp => AdminApi.Currency.Gbp,
		Currency.Sgd => AdminApi.Currency.Sgd,
		Currency.Rub => AdminApi.Currency.Rub,
		_ => throw new InvalidEnumConversionException(currency),
	};
}
