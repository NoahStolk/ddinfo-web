using DevilDaggersWebsite.Models.Game;
using System.Globalization;

namespace DevilDaggersWebsite.Utils
{
	public static class RazorUtils
	{
		public static CultureInfo DEFAULT_CULTURE = CultureInfo.CreateSpecificCulture("en-US");

		public static string Email = "contact@devildaggers.info";

		public static string BanColorCode = "666666";

		public const int DEFAULT_MAX_WAVES = 25;
		public const int MAX_SPAWNS = 10000;

		public static string GetEnemyLayoutAnchor(Enemy enemy, bool plural = false)
		{
			return $"<a style='color: #{enemy.ColorCode};' href='/Enemies#{enemy.Name.Replace(" ", string.Empty)}'>{enemy.Name}{(plural ? "s" : "")}</a>";
		}

		public static string GetUpgradeLayoutAnchor(Upgrade upgrade)
		{
			return $"<a style='color: #{upgrade.ColorCode};' href='/Upgrades#Level{upgrade.Level}'>Level {upgrade.Level}</a>";
		}

		public static string GetLayout(string str)
		{
			char[] beginSeparators = new char[] { '>', ' ', ',', '.', '(' };
			char[] endSeparators = new char[] { ' ', ',', '.', 's', ')' };

			for (int i = GameHelper.Enemies.Length - 1; i >= 0; i--) // Reverse iteration because transmuted skulls are after normal skulls in the list
			{
				Enemy enemy = GameHelper.Enemies[i];
				foreach (char begin in beginSeparators)
				{
					foreach (char end in endSeparators)
					{
						string enemyString = $"{begin}{enemy.Name}{end}";
						if (str.Contains(enemyString))
						{
							// Enemy string should not be inside an <a> element
							if (str.Length < str.IndexOf(enemyString) + enemyString.Length + "</a>".Length || str.Substring(str.IndexOf(enemyString) + enemyString.Length, "</a>".Length) != "</a>")
								str = str.Replace(enemyString, $"{begin}{GetEnemyLayoutAnchor(enemy, (end == 's'))}{((end == 's') ? "" : end.ToString())}");
						}
					}
				}
			}

			foreach (Upgrade upgrade in GameHelper.Upgrades)
			{
				foreach (char begin in beginSeparators)
				{
					foreach (char end in endSeparators)
					{
						string upgradeString = $"{begin}Level {upgrade.Level}{end}";
						if (str.Contains(upgradeString))
							str = str.Replace(upgradeString, $"{begin}{GetUpgradeLayoutAnchor(upgrade)}{end}");
					}
				}
			}

			return str;
		}
	}
}