using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using MainApi = DevilDaggersInfo.Api.Main.Donations;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class DonationConverters
{
	public static MainApi.Currency ToMainApi(this Currency currency) => currency switch
	{
		Currency.Eur => MainApi.Currency.Eur,
		Currency.Usd => MainApi.Currency.Usd,
		Currency.Aud => MainApi.Currency.Aud,
		Currency.Gbp => MainApi.Currency.Gbp,
		Currency.Sgd => MainApi.Currency.Sgd,
		Currency.Rub => MainApi.Currency.Rub,
		_ => throw new InvalidEnumConversionException(currency),
	};
}
