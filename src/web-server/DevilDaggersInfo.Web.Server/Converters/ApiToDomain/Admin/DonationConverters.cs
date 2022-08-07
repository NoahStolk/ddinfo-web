using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using AdminApi = DevilDaggersInfo.Api.Admin.Donations;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;

public static class DonationConverters
{
	public static Currency ToDomain(this AdminApi.Currency currency) => currency switch
	{
		AdminApi.Currency.Eur => Currency.Eur,
		AdminApi.Currency.Usd => Currency.Usd,
		AdminApi.Currency.Aud => Currency.Aud,
		AdminApi.Currency.Gbp => Currency.Gbp,
		AdminApi.Currency.Sgd => Currency.Sgd,
		AdminApi.Currency.Rub => Currency.Rub,
		_ => throw new InvalidEnumConversionException(currency),
	};
}
