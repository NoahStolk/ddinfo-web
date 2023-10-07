using DevilDaggersInfo.Web.ApiSpec.Admin.Donations;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;

public static class DonationConverters
{
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
