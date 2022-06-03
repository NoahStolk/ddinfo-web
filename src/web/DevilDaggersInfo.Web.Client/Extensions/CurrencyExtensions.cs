using DevilDaggersInfo.Api.Main.Donations;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CurrencyExtensions
{
	public static char GetChar(this Currency currency) => currency switch
	{
		Currency.Eur => '€',
		Currency.Usd or Currency.Aud or Currency.Sgd => '$',
		Currency.Gbp => '£',
		Currency.Rub => '₽',
		_ => '?',
	};
}
