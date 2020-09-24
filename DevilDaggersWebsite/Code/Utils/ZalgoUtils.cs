using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace DevilDaggersWebsite.Code.Utils
{
	public static class ZalgoUtils
	{
		private static readonly List<string> _zalgoAll = new List<string>();

		private static readonly Random _random = new Random();

		static ZalgoUtils()
		{
			for (int i = 300; i < 340; i++)
			{
				int num = i;
				string codePoint = $"0{num}";
				int code = int.Parse(codePoint, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
				string unicodeString = char.ConvertFromUtf32(code);
				_zalgoAll.Add(unicodeString);
			}
		}

		public static string InterpolateHexColor(string c1, string c2, float percentage)
		{
			ColorConverter converter = new ColorConverter();

			Color color1 = (Color)converter.ConvertFromString(c1);
			Color color2 = (Color)converter.ConvertFromString(c2);

			byte r = (byte)Lerp(color1.R, color2.R, percentage);
			byte g = (byte)Lerp(color1.G, color2.G, percentage);
			byte b = (byte)Lerp(color1.B, color2.B, percentage);

			return $"{r:x2}{g:x2}{b:x2}";
		}

		public static string ToZalgo(this string str, float amount)
		{
			if (amount == 0)
				return str;

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < str.Length; i++)
			{
				sb.Append(str[i]);
				for (int j = 0; j < amount; j++)
				{
					if (j == Math.Floor(amount))
					{
						float chance = amount % 1;
						if (_random.NextDouble() < chance)
							sb.Append(_zalgoAll[_random.Next(_zalgoAll.Count)]);
					}
					else
					{
						sb.Append(_zalgoAll[_random.Next(_zalgoAll.Count)]);
					}
				}
			}

			return sb.ToString();
		}

		private static float Lerp(int x, int y, float percentage)
			=> (y - x) * percentage + x;
	}
}