using DevilDaggersInfo.Web.ApiSpec.Main.Donations;

namespace DevilDaggersInfo.Web.Client.Extensions;

internal static class CurrencyExtensions
{
	public static char GetChar(this Currency currency)
	{
		return currency switch
		{
			Currency.Eur => '€',
			Currency.Usd or Currency.Aud or Currency.Sgd => '$',
			Currency.Gbp => '£',
			Currency.Rub => '₽',
			_ => '?',
		};
	}
}
