using DevilDaggersCore.Game;
using DevilDaggersWebsite.Models.Leaderboard;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Utils
{
	public static class RazorUtils
	{
		public static HtmlString NAString = new HtmlString($"<span style='color: #444;'>N/A</span>");

		public static string DiscordLink = "https://discord.gg/NF32j8S";

		public static string Email = "contact@devildaggers.info";

		public static string BanColorCode = "666";

		public const int DEFAULT_MAX_WAVES = 28;
		public const int MAX_SPAWNS = 10000;

		public static HtmlString GetEnemyLayoutAnchor(Enemy enemy, bool plural = false, float zalgo = 0)
		{
			string color = (zalgo == 0 ? enemy.ColorCode : ZalgoUtils.InterpolateHexColor($"#FF{enemy.ColorCode}", "#FFFF0000", zalgo / 100f));
			return new HtmlString($"<a style='color: #{color};' href='/Wiki/Enemies#{enemy.Name.Replace(" ", "")}'>{enemy.Name.ToZalgo(zalgo / 20f)}{(plural ? "s" : "")}</a>");
		}

		public static HtmlString GetUpgradeLayoutAnchor(Upgrade upgrade)
		{
			return new HtmlString($"<a style='color: #{upgrade.ColorCode};' href='/Wiki/Upgrades#{upgrade.Name}'>{upgrade.Name}</a>");
		}

		// TODO: Rewrite
		public static HtmlString GetLayout(string str)
		{
			char[] beginSeparators = new char[] { '>', ' ', ',', '.', '(' };
			char[] endSeparators = new char[] { ' ', ',', '.', 's', ')', '\'' };

			List<Enemy> enemies = Game.GetEntities<Enemy>();
			for (int i = enemies.Count - 1; i >= 0; i--) // Reverse iteration because transmuted skulls come after normal skulls in the list
			{
				Enemy enemy = enemies[i];
				foreach (char begin in beginSeparators)
				{
					foreach (char end in endSeparators)
					{
						string enemyString = $"{begin}{enemy.Name}{end}";
						if (str.Contains(enemyString))
						{
							// Enemy string should not be inside an <a> element
							if (str.Length < str.IndexOf(enemyString) + enemyString.Length + "</a>".Length || str.Substring(str.IndexOf(enemyString) + enemyString.Length, "</a>".Length) != "</a>")
								str = str.Replace(enemyString, $"{begin}{GetEnemyLayoutAnchor(enemy, end == 's')}{((end == 's') ? "" : end.ToString())}");
						}
					}
				}
			}

			foreach (Upgrade upgrade in Game.GetEntities<Upgrade>())
			{
				foreach (char begin in beginSeparators)
				{
					foreach (char end in endSeparators)
					{
						string upgradeString = $"{begin}{upgrade.Name}{end}";
						if (str.Contains(upgradeString))
							str = str.Replace(upgradeString, $"{begin}{GetUpgradeLayoutAnchor(upgrade)}{end}");
					}
				}
			}

			return new HtmlString(str);
		}

		public static HtmlString GetLeaderboardInformationHTMLString(string info)
		{
			if (string.IsNullOrEmpty(info) || string.IsNullOrWhiteSpace(info))
				return NAString;

			return new HtmlString(info
				.TrimEnd('\n')
				.Replace("\n", "<br />")
				.Replace(CompletionEntryCombined.PartiallyMissing.ToString(), "<span style='color: #f80'>(Partially missing)</span>")
				.Replace(CompletionEntryCombined.Missing.ToString(), "<span style='color: #f00'>(Missing)</span>"));
		}
	}
}