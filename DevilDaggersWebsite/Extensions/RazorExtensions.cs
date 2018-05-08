using DevilDaggersWebsite.Helpers;
using DevilDaggersWebsite.Models.Game;

namespace DevilDaggersWebsite.Extensions
{
	public static class RazorExtensions
	{
		public static string Email = "contact@devildaggers.info";

		public static string BanColorCode = "666666";


		public static string GetEnemyLayoutAnchor(Enemy enemy, bool plural = false)
		{
			return string.Format("<a style='color: #{0};' href='/Home/Enemies/#{1}'>{2}{3}</a>", enemy.ColorCode, enemy.Name.Replace(" ", string.Empty), enemy.Name, (plural ? "s" : ""));
		}

		public static string GetHandLayoutAnchor(Upgrade hand, bool hideMobile = false)
		{
			if (hideMobile)
				return string.Format("<a style='color: #{0};' href='/Home/Hands/#{1}'>{2}</a>", hand.ColorCode, string.Format("Level{0}", hand.Level), string.Format("<span class='hide-mobile'>Level </span>{0}", hand.Level));

			return string.Format("<a style='color: #{0};' href='/Home/Hands/#{1}'>{2}</a>", hand.ColorCode, string.Format("Level{0}", hand.Level), string.Format("Level {0}", hand.Level));
		}

		public static string GetLayout(string str)
		{
			char[] beginSeparators = new char[] { '>', ' ', ',', '.' };
			char[] endSeparators = new char[] { ' ', ',', '.', 's' };

			foreach (Enemy enemy in GameHelper.Enemies)
			{
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
							str = str.Replace(handString, string.Format("{0}{1}{2}", begin, GetHandLayoutAnchor(upgrade), end));
					}
				}
			}

			return str;
		}
	}
}