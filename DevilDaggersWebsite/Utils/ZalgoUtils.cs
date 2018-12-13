using System;
using System.Drawing;
using System.Text;

namespace DevilDaggersWebsite.Utils
{
	public static class ZalgoUtils
	{
		private readonly static Random random = new Random();

		private readonly static string[] zalgoAll = new string[]
		{
			"̍", "̎", "̄", "̅", "̿", "̑", "̆", "̐", "͒", "͗", "͑", "̇", "̈", "̊", "͂", "̓", "̈́", "͊", "͋", "͌", "̃", "̂", "̌", "͐", "̀", "́", "̋", "̏", "̒", "̓", "̔", "̽", "̉", "ͣ", "ͤ", "ͥ", "ͦ", "ͧ", "ͨ", "ͩ", "ͪ", "ͫ", "ͬ", "ͭ", "ͮ", "ͯ", "̾", "͛", "͆", "̚", "̕", "̛", "̀", "́", "͘", "̡", "̢", "̧", "̨", "̴", "̵", "̶", "͏", "͜", "͝", "͞", "͟", "͠", "͢", "̸", "̷", "͡", "҉", "̖", "̗", "̘", "̙", "̜", "̝", "̞", "̟", "̠", "̤", "̥", "̦", "̩", "̪", "̫", "̬", "̭", "̮", "̯", "̰", "̱", "̲", "̳", "̹", "̺", "̻", "̼", "ͅ", "͇", "͈", "͉", "͍", "͎", "͓", "͔", "͕", "͖", "͙"
		};

		public static string InterpolateHexColor(string c1, string c2, float percentage)
		{
			ColorConverter converter = new ColorConverter();

			Color color1 = (Color)converter.ConvertFromString(c1);
			Color color2 = (Color)converter.ConvertFromString(c2);

			byte r = (byte)Lerp(color1.R, color2.R, percentage);
			byte g = (byte)Lerp(color1.G, color2.G, percentage);
			byte b = (byte)Lerp(color1.B, color2.B, percentage);

			return string.Format("{0:x2}{1:x2}{2:x2}", r, g, b);
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
						if (random.NextDouble() < chance)
							sb.Append(zalgoAll[random.Next(zalgoAll.Length)]);
					}
					else
					{
						sb.Append(zalgoAll[random.Next(zalgoAll.Length)]);
					}
				}
			}

			return sb.ToString();
		}

		private static float Lerp(int x, int y, float percentage)
		{
			return (y - x) * percentage + x;
		}
	}
}