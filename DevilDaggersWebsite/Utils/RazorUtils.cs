using DevilDaggersWebsite.Models.Game;

namespace DevilDaggersWebsite.Utils
{
	public static class RazorUtils
	{
		public static string Email = "contact@devildaggers.info";

		public static string BanColorCode = "666666";

		public static string GetEnemyLayoutAnchor(Enemy enemy, bool plural = false)
		{
			return string.Format("<a style='color: #{0};' href='/Enemies#{1}'>{2}{3}</a>", enemy.ColorCode, enemy.Name.Replace(" ", string.Empty), enemy.Name, (plural ? "s" : ""));
		}

		public static string GetUpgradeLayoutAnchor(Upgrade upgrade, bool hideMobile = false)
		{
			if (hideMobile)
				return string.Format("<a style='color: #{0};' href='/Upgrades#{1}'>{2}</a>", upgrade.ColorCode, string.Format("Level{0}", upgrade.Level), string.Format("<span class='hidden-xs'>Level </span>{0}", upgrade.Level));

			return string.Format("<a style='color: #{0};' href='/Upgrades#{1}'>{2}</a>", upgrade.ColorCode, string.Format("Level{0}", upgrade.Level), string.Format("Level {0}", upgrade.Level));
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
						string enemyString = string.Format("{0}{1}{2}", begin, enemy.Name, end);
						if (str.Contains(enemyString))
						{
							// Enemy string should not be inside an <a> element
							if (str.Substring(str.IndexOf(enemyString) + enemyString.Length, "</a>".Length) != "</a>")
								str = str.Replace(enemyString, string.Format("{0}{1}{2}", begin, GetEnemyLayoutAnchor(enemy, (end == 's')), (end == 's') ? "" : end.ToString()));
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
						string handString = string.Format("{0}Level {1}{2}", begin, upgrade.Level, end);
						if (str.Contains(handString))
							str = str.Replace(handString, string.Format("{0}{1}{2}", begin, GetUpgradeLayoutAnchor(upgrade), end));
					}
				}
			}

			return str;
		}
	}
}