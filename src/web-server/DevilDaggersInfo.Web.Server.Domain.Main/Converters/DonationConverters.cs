using DevilDaggersInfo.Api.Main.Donations;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters;

public static class DonationConverters
{
	public static Currency ToMainApi(this Types.Web.Currency currency) => currency switch
	{
		Types.Web.Currency.Eur => Currency.Eur,
		Types.Web.Currency.Usd => Currency.Usd,
		Types.Web.Currency.Aud => Currency.Aud,
		Types.Web.Currency.Gbp => Currency.Gbp,
		Types.Web.Currency.Sgd => Currency.Sgd,
		Types.Web.Currency.Rub => Currency.Rub,
		_ => throw new UnreachableException(),
	};
}
