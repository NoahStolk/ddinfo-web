using System;

namespace DevilDaggersWebsite.Code.Extensions
{
	public static class EnumExtensions
	{
		public static char GetChar(this Users.Currency currency) => currency switch
		{
			Users.Currency.Eur => '€',
			Users.Currency.Usd => '$',
			Users.Currency.Aud => '$',
			Users.Currency.Gbp => '£',
			_ => throw new NotImplementedException($"{nameof(Users.Currency)} '{currency}' has not been implemented.")
		};

		public static char GetChar(this Database.Currency currency) => currency switch
		{
			Database.Currency.Eur => '€',
			Database.Currency.Usd => '$',
			Database.Currency.Aud => '$',
			Database.Currency.Gbp => '£',
			_ => throw new NotImplementedException($"{nameof(Database.Currency)} '{currency}' has not been implemented.")
		};
	}
}