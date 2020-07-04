using System;

namespace DevilDaggersWebsite.Code.Donations
{
	public enum Currency
	{
		Eur,
		Usd,
		Aud,
		Gbp
	}

	public static class CurrencyExtensions
	{
		public static char GetChar(this Currency currency) => currency switch
		{
			Currency.Eur => '€',
			Currency.Usd => '$',
			Currency.Aud => '$',
			Currency.Gbp => '£',
			_ => throw new NotImplementedException($"{nameof(Currency)} '{currency}' has not been implemented.")
		};
	}
}