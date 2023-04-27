using DevilDaggersInfo.Api.Main.Donations;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters;

public static class DonationConverters
{
	public static Currency ToMainApi(this Entities.Enums.Currency currency) => currency switch
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
