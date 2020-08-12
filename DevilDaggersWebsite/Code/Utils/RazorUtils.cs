using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DevilDaggersWebsite.Code.Utils
{
	public static class RazorUtils
	{
		public const string DiscordUrl = "https://discord.gg/NF32j8S";
		public const string ContactEmail = "contact@devildaggers.info";

		public static HtmlString NAString { get; set; } = new HtmlString($"<span style='color: #444;'>N/A</span>");

		public static HtmlString GetCssList(IWebHostEnvironment env, string subdirectory)
			=> GetList(env, subdirectory, (sb, href) => sb.Append($"<link rel='stylesheet' href='/{href}' />\n"));

		public static HtmlString GetJsList(IWebHostEnvironment env, string subdirectory)
			=> GetList(env, subdirectory, (sb, href) => sb.Append($"<script defer src='/{href}' asp-append-version='true'></script>\n"));

		private static HtmlString GetList(IWebHostEnvironment env, string subdirectory, Action<StringBuilder, string> appendAction)
		{
			string directory = Path.Combine(env.WebRootPath, subdirectory);

			StringBuilder sb = new StringBuilder();
			foreach (string path in Directory.GetFiles(directory))
				appendAction(sb, Path.Combine(subdirectory, Path.GetFileName(path)));

			return new HtmlString(sb.ToString());
		}

		public static HtmlString GetCopyrightString(string name, int startYear)
			=> GetCopyrightString(name, startYear, DateTime.Now.Year);

		public static HtmlString GetCopyrightString(string name, int startYear, int endYear)
		{
			string year = startYear == endYear ? startYear.ToString() : $"{startYear}-{endYear}";

			return new HtmlString($"Copyright &copy; {year} {name}");
		}

		public static HtmlString GetLayoutAnchor(this Enemy enemy, bool plural = false, float zalgo = 0)
		{
			string color = zalgo == 0 ? enemy.ColorCode : ZalgoUtils.InterpolateHexColor($"#FF{enemy.ColorCode}", "#FFFF0000", zalgo / 100f);
			return new HtmlString($"<a style='color: #{color};' href='/Wiki/Enemies#{enemy.Name.Replace(" ", "")}'>{enemy.Name.ToZalgo(zalgo / 20f)}{(plural ? "s" : "")}</a>");
		}

		public static HtmlString GetLayoutAnchor(this Upgrade upgrade)
		{
			return new HtmlString($"<a style='color: #{upgrade.ColorCode};' href='/Wiki/Upgrades#{upgrade.Name}'>{upgrade.Name}</a>");
		}

		// TODO: Rewrite whole method. It's messy and not very performant.
		public static HtmlString GetLayout(string str, GameVersion? gameVersion = null)
		{
			char[] beginSeparators = new char[] { '>', ' ', ',', '.', '(', '-', '/' };
			char[] endSeparators = new char[] { ' ', ',', '.', 's', ')', '\'', ';', '/' };

			List<Enemy> enemies = GameInfo.GetEntities<Enemy>(gameVersion);
			for (int i = enemies.Count - 1; i >= 0; i--) // Use reverse iteration because transmuted skulls come after normal skulls in the list.
			{
				Enemy enemy = enemies[i];
				foreach (char begin in beginSeparators)
				{
					foreach (char end in endSeparators)
					{
						string enemyString = $"{begin}{enemy.Name}{end}";
						if (str.Contains(enemyString))
						{
							// Enemy string should not be inside an <a> element.
							if (str.Length < str.IndexOf(enemyString) + enemyString.Length + "</a>".Length || str.Substring(str.IndexOf(enemyString) + enemyString.Length, "</a>".Length) != "</a>")
								str = str.Replace(enemyString, $"{begin}{GetLayoutAnchor(enemy, end == 's')}{((end == 's') ? "" : end.ToString())}");
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
						if (str.Contains(upgradeString))
							str = str.Replace(upgradeString, $"{begin}{GetLayoutAnchor(upgrade)}{end}");
					}
				}
			}

			return new HtmlString(str);
		}

		public static string TransmuteString(this string str)
		{
			str = str
				.Replace(" transmute ", " <a style='color: var(--col-red);' href='/Wiki/Enemies#transmuted-skulls'>transmute</a> ")
				.Replace(" transmutes ", " <a style='color: var(--col-red);' href='/Wiki/Enemies#transmuted-skulls'>transmutes</a> ");

			return str;
		}

		public static string ToIdString(this string str)
			=> $"{str.ToLower().Replace(" ", "-")}";

		public static string S(this int value)
			=> value == 1 ? "" : "s";

		public static HtmlString GetFormattedReturnType(Type type)
		{
			string cssClass = type.IsGenericType ? "api-generic-return-type" : "api-return-type";
			StringBuilder sb = new StringBuilder($"<span class='{cssClass}'>{GetTypeString(type.Name)}</span>");
			while (type.IsGenericType)
			{
				Type[] genericArguments = type.GetGenericArguments();
				type = genericArguments[0];
				cssClass = type.IsGenericType ? "api-generic-return-type" : "api-return-type";

				sb.Append($"<span class='api-generic-return-type'>&lt;</span><span class='{cssClass}'>{string.Join(", ", genericArguments.Select(t => GetTypeString(t.Name)))}</span><span class='api-generic-return-type'>&gt;</span>");
			}
			return new HtmlString(sb.ToString());

			static string GetTypeString(string typeName)
			{
				if (typeName.Contains('`'))
					return typeName.Substring(0, typeName.IndexOf('`'));
				return typeName;
			}
		}

		public static HtmlString GetFormattedParameter(ParameterInfo parameter)
		{
			Type underlyingType = Nullable.GetUnderlyingType(parameter.ParameterType);
			bool isNullable = underlyingType != null;
			Type actualType = isNullable ? underlyingType : parameter.ParameterType;

			string typeSpan = $"<span class='api-parameter-type'>{actualType.Name}</span>";
			typeSpan = isNullable ? $"<span class='api-nullable'>Nullable&lt;{typeSpan}&gt;</span>" : typeSpan;

			return new HtmlString(@$"{typeSpan}
<span class='api-parameter{(parameter.IsOptional ? "-optional" : "")}'>{parameter.Name}</span>
{(parameter.IsOptional ? $"= <span class='api-parameter-default-value'>{GetParameterFormattedDefaultValue(parameter)}</span>" : "")}");
		}

		public static HtmlString GetParameterFormattedDefaultValue(ParameterInfo parameter)
		{
			Type underlyingType = Nullable.GetUnderlyingType(parameter.ParameterType);
			bool isNullable = underlyingType != null;
			Type actualType = isNullable ? underlyingType : parameter.ParameterType;

			if (actualType.IsValueType)
			{
				if (parameter.HasDefaultValue)
					return new HtmlString(parameter.DefaultValue?.ToString() ?? "null");
				return new HtmlString(Activator.CreateInstance(actualType).ToString());
			}

			if (parameter.HasDefaultValue && !string.IsNullOrEmpty((string)parameter.DefaultValue))
				return new HtmlString(parameter.DefaultValue.ToString());
			return new HtmlString("<span class='api-null'>null</span>");
		}
	}
}