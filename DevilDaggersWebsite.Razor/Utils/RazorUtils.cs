using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DevilDaggersWebsite.Razor.Utils
{
	public static class RazorUtils
	{
		public const string DiscordUrl = "https://discord.gg/NF32j8S";
		public const string ContactEmail = "contact@devildaggers.info";

		static RazorUtils()
		{
			try
			{
				BuildTime = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToUniversalTime().ToString("yyyy-MM-dd HH:mm UTC", CultureInfo.InvariantCulture);
			}
			catch
			{
			}
		}

		public static string BuildTime { get; } = string.Empty;

		public static HtmlString NAString { get; set; } = new("<span style='color: #444;'>N/A</span>");

		public static HtmlString GetCssList(IWebHostEnvironment env, string subdirectory)
			=> GetList(env, subdirectory, (sb, href) => sb.Append("<link rel='stylesheet' href='/").Append(href).Append("' />\n"));

		public static HtmlString GetJsList(IWebHostEnvironment env, string subdirectory)
			=> GetList(env, subdirectory, (sb, href) => sb.Append("<script defer src='/").Append(href).Append("' asp-append-version='true'></script>\n"));

		private static HtmlString GetList(IWebHostEnvironment env, string subdirectory, Action<StringBuilder, string> appendAction)
		{
			string directory = Path.Combine(env.WebRootPath, subdirectory);

			StringBuilder sb = new();
			foreach (string path in Directory.GetFiles(directory))
				appendAction(sb, Path.Combine(subdirectory, Path.GetFileName(path)));

			return new HtmlString(sb.ToString());
		}

		public static HtmlString GetCopyrightString(string name, int startYear)
			=> GetCopyrightString(name, startYear, DateTime.Now.Year);

		public static HtmlString GetCopyrightString(string name, int startYear, int endYear)
		{
			string year = startYear == endYear ? startYear.ToString(CultureInfo.InvariantCulture) : $"{startYear}-{endYear}";

			return new HtmlString($"Copyright &copy; {year} {name}");
		}

		public static HtmlString GetLayoutAnchor(this Enemy enemy, bool plural = false, GameVersion? gameVersionOverride = null)
		{
			string colorCode = enemy.ColorCode;
			if (gameVersionOverride.HasValue)
				colorCode = GameInfo.GetEntities<Enemy>(gameVersionOverride).First(e => e.Name == enemy.Name).ColorCode;

			return new HtmlString($"<a style='color: #{colorCode};' href='/Wiki/Enemies{(gameVersionOverride == null ? string.Empty : $"?GameVersion={gameVersionOverride}")}#{enemy.Name.Replace(" ", string.Empty, StringComparison.InvariantCulture)}'>{enemy.Name}{(plural ? "s" : string.Empty)}</a>");
		}

		public static HtmlString GetLayoutAnchor(this Upgrade upgrade)
			=> new HtmlString($"<a style='color: #{upgrade.ColorCode};' href='/Wiki/Upgrades?GameVersion={upgrade.GameVersion}#{upgrade.Name}'>{upgrade.Name}</a>");

		// TODO: Rewrite whole method. It's messy and not very performant.
		public static HtmlString GetLayout(string str, GameVersion? gameVersion = null)
		{
			char[] beginSeparators = new char[] { '>', ' ', ',', '.', '(', '-', '/' };
			char[] endSeparators = new char[] { ' ', ',', '.', 's', ')', '\'', ';', '/' };

			List<Enemy> enemies = GameInfo.GetEntities<Enemy>(gameVersion);

			// Use reverse iteration because transmuted skulls come after normal skulls in the list.
			for (int i = enemies.Count - 1; i >= 0; i--)
			{
				Enemy enemy = enemies[i];
				foreach (char begin in beginSeparators)
				{
					foreach (char end in endSeparators)
					{
						string enemyString = $"{begin}{enemy.Name}{end}";
						if (str.Contains(enemyString, StringComparison.InvariantCulture))
						{
							// Enemy string should not be inside an <a> element.
							if (str.Length < str.IndexOf(enemyString, StringComparison.InvariantCulture) + enemyString.Length + "</a>".Length || str.Substring(str.IndexOf(enemyString, StringComparison.InvariantCulture) + enemyString.Length, "</a>".Length) != "</a>")
								str = str.Replace(enemyString, $"{begin}{enemy.GetLayoutAnchor(end == 's')}{(end == 's' ? string.Empty : end.ToString())}", StringComparison.InvariantCulture);
						}
					}
				}
			}

			foreach (Upgrade upgrade in GameInfo.GetEntities<Upgrade>(gameVersion))
			{
				foreach (char begin in beginSeparators)
				{
					foreach (char end in endSeparators)
					{
						string upgradeString = $"{begin}{upgrade.Name}{end}";
						if (str.Contains(upgradeString, StringComparison.InvariantCulture))
							str = str.Replace(upgradeString, $"{begin}{upgrade.GetLayoutAnchor()}{end}", StringComparison.InvariantCulture);
					}
				}
			}

			return new HtmlString(str);
		}

		public static string TransmuteString(this string str)
		{
			return str
				.Replace(" transmute ", " <a style='color: var(--col-red);' href='/Wiki/Enemies#transmuted-skulls'>transmute</a> ", StringComparison.InvariantCulture)
				.Replace(" transmutes ", " <a style='color: var(--col-red);' href='/Wiki/Enemies#transmuted-skulls'>transmutes</a> ", StringComparison.InvariantCulture);
		}

		public static string ToIdString(this string str)
			=> $"{str.ToLower(CultureInfo.InvariantCulture).Replace(" ", "-", StringComparison.InvariantCulture)}";

		public static string S(this int value)
			=> value == 1 ? string.Empty : "s";

		public static string GetCssWidth(int width)
			=> $"width: {width}px;";

		public static List<SelectListItem> EnumToSelectList<TEnum>()
			where TEnum : Enum
			=> ((IEnumerable<TEnum>)Enum.GetValues(typeof(TEnum))).Select(c => new SelectListItem { Text = c.ToString(), Value = ((int)(object)c).ToString(CultureInfo.InvariantCulture) }).ToList();
	}
}