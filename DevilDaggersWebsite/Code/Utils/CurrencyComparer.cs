using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Utils
{
	public class CurrencyComparer : Comparer<char>
	{
		private readonly List<char> currencies = new List<char>
		{
			'£',
			'€',
			'$'
		};

		public override int Compare(char x, char y)
		{
			if (currencies.IndexOf(x) < currencies.IndexOf(y))
				return -1;
			else if (currencies.IndexOf(x) > currencies.IndexOf(y))
				return 1;
			return 0;
		}
	}
}