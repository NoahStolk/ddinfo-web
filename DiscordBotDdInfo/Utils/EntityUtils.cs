using DevilDaggersCore.Game;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DiscordBotDdInfo.Utils
{
	public static class EntityUtils
	{
		public static string ToEntityName(this string s, bool usesRoman)
		{
			string[] parts = s.Split(' ');
			StringBuilder sb = new();
			foreach (string part in parts)
			{
				if (usesRoman)
				{
					if (part.IsRoman())
					{
						sb.Append(part.ToUpper(CultureInfo.InvariantCulture)); // Capitalize Roman numeral entirely.
						continue;
					}
					else if (int.TryParse(part, out int number) && number < 1000)
					{
						sb.Append(RomanUtils.IntegerToRoman(number)); // Turn number into Roman numeral.
						continue;
					}
				}
				else
				{
					if (part.IsRoman())
					{
						sb.Append(RomanUtils.RomanToInteger(part.ToUpper(CultureInfo.InvariantCulture))); // Turn Roman numeral into number.
						continue;
					}
				}

				sb.Append(part.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture)).Append(part[1..]).Append(' '); // Capitalize first letter of word.
			}

			return sb.ToString().TrimEnd(' ');
		}

		public static bool TryGetInfo<TEntity>(string[] inputs, out string searchResult, out TEntity? entity)
			where TEntity : DevilDaggersEntity
		{
			string? gameVersionKey = Array.Find(inputs, s => Enum.GetNames(typeof(GameVersion)).Any(n => n == s.ToUpper(CultureInfo.InvariantCulture)));
			GameVersion? gameVersion = gameVersionKey == null ? null : (GameVersion?)Enum.Parse(typeof(GameVersion), gameVersionKey.ToUpper(CultureInfo.InvariantCulture));

			List<string> inputsExcludingGameVersion = new();
			foreach (string input in inputs)
			{
				if (input != gameVersionKey)
					inputsExcludingGameVersion.Add(input);
			}

			bool usesRoman = false;
			entity = null;
			StringBuilder? search = null;
			List<string> searches = new();

			for (int i = 0; i < 2; i++)
			{
				int nameIndex = 0;
				search = new(inputsExcludingGameVersion[nameIndex]);

				// TODO: Rewrite this so all permutations are calculated.
				do
				{
					usesRoman = i % 2 == 0;
					string convertedName = search.ToString().ToEntityName(usesRoman).ToLower(CultureInfo.InvariantCulture);

					if (!searches.Contains(convertedName))
						searches.Add(convertedName);

					IEnumerable<TEntity> entities = GameInfo.GetEntities<TEntity>(gameVersion).Where(e => e.Name.ToLower(CultureInfo.InvariantCulture) == convertedName);

					if (!entities.Any())
					{
						if (nameIndex < inputsExcludingGameVersion.Count - 1)
							search.Append(' ').Append(inputsExcludingGameVersion[++nameIndex]);
						else break;
					}
					else
					{
						entity = entities.OrderByDescending(e => e.GameVersion).FirstOrDefault(); // Take the one from the highest game version.
					}
				}
				while (entity == null);

				if (entity != null)
					break;
			}

			searchResult = $"{string.Join(", ", searches.Select(s => $"`{s}`"))}";
			return entity != null;
		}
	}
}
