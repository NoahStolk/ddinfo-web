using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DiscordBotDdInfo.Utils
{
	public static class RomanUtils
	{
		private static readonly Dictionary<char, int> _romanMap = new()
		{
			{ 'I', 1 },
			{ 'V', 5 },
			{ 'X', 10 },
			{ 'L', 50 },
			{ 'C', 100 },
			{ 'D', 500 },
			{ 'M', 1000 },
		};

		private static readonly Dictionary<string, int> _romanMapExtended = new()
		{
			{ "I", 1 },
			{ "IV", 4 },
			{ "V", 5 },
			{ "IX", 9 },
			{ "X", 10 },
			{ "XL", 40 },
			{ "L", 50 },
			{ "XC", 90 },
			{ "C", 100 },
			{ "CD", 400 },
			{ "D", 500 },
			{ "CM", 900 },
			{ "M", 1000 },
		};

		public static int RomanToInteger(string input)
		{
			int number = 0;

			for (int i = 0; i < input.Length; i++)
			{
				if (i + 1 < input.Length && _romanMap[input[i]] < _romanMap[input[i + 1]])
					number -= _romanMap[input[i]];
				else
					number += _romanMap[input[i]];
			}

			return number;
		}

		public static string IntegerToRoman(int input)
		{
			int l = 0;
			for (int i = input; i > 0; i--)
			{
				if (_romanMapExtended.ContainsValue(i))
				{
					l = i;
					break;
				}
			}

			if (input == l)
				return _romanMapExtended.FirstOrDefault(x => x.Value == input).Key;

			return _romanMapExtended.FirstOrDefault(x => x.Value == l).Key + IntegerToRoman(input - l);
		}

		public static bool IsRoman(this string s, bool caseSensitive = false)
		{
			if (!caseSensitive)
				s = s.ToUpper(CultureInfo.InvariantCulture);
			foreach (char c in s)
			{
				if (!_romanMap.ContainsKey(c))
					return false;
			}

			return true;
		}
	}
}
