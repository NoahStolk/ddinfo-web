using DevilDaggersWebsite.Code.Database;
using System;

namespace DevilDaggersWebsite.Code.Extensions
{
	public static class EnumExtensions
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