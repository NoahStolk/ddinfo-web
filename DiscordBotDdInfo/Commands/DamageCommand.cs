using DevilDaggersCore.Game;
using DiscordBotDdInfo.Attributes;
using DiscordBotDdInfo.Utils;
using DSharpPlus.Entities;
using System;
using System.Globalization;

namespace DiscordBotDdInfo.Commands
{
	public class DamageCommand : AbstractCommand
	{
		public override string Name => "damage";

		[SearchCommand]
		public CommandResult Execute(string search)
		{
			string[] searchArray = search.Split(' ');
			if (!EntityUtils.TryGetInfo(searchArray, out string searchResult, out Enemy? enemy) || enemy == null)
				return new($"No info found using case-insensitive search for the following entities: {searchResult}");

			bool gem = searchArray[^1] == "gem" && enemy.Gems > 1;

			DiscordEmbedBuilder builder = new()
			{
				Title = $"Damage stats for {enemy.Name}{(gem ? " Gem" : string.Empty)} ({enemy.GameVersion})",
				Url = $"{NetworkUtils.BaseUrl}/Wiki/Enemies#damage-stats",
				Color = new DiscordColor(enemy.ColorCode),
				Thumbnail = new() { Url = gem ? $"{NetworkUtils.BaseUrl}/Images/Icons/gem.png" : $"{NetworkUtils.BaseUrl}/Images/Icons/skull.png", },
			};

			int hp = gem ? enemy.Hp / enemy.Gems : enemy.Hp;
			string homing3 = enemy.Homing3.HasValue ? gem ? (enemy.Homing3.Value / enemy.Gems).ToString("0.##", CultureInfo.InvariantCulture) : enemy.Homing3.Value.ToString("0.##", CultureInfo.InvariantCulture) : "N/A";
			string homing4 = enemy.Homing4.HasValue ? gem ? (enemy.Homing4.Value / enemy.Gems).ToString("0.##", CultureInfo.InvariantCulture) : enemy.Homing4.Value.ToString("0.##", CultureInfo.InvariantCulture) : "N/A";

			builder.AddField("HP", hp.ToString(CultureInfo.InvariantCulture));

			foreach (Upgrade upgrade in GameInfo.GetEntities<Upgrade>(enemy.GameVersion))
			{
				float defaultSpray = gem ? enemy.Hp / enemy.Gems / upgrade.DefaultSprayPerSecond : enemy.Hp / upgrade.DefaultSprayPerSecond;
				builder.AddField($"{upgrade.Name} default spray", $"{defaultSpray:0.000} seconds");

				float defaultShot = gem ? (float)Math.Ceiling(enemy.Hp / enemy.Gems / (float)upgrade.DefaultShot) : (float)Math.Ceiling(enemy.Hp / (float)upgrade.DefaultShot);
				builder.AddField($"{upgrade.Name} default shot", defaultShot.ToString(CultureInfo.InvariantCulture));

				if (upgrade == GameInfo.V1Level3 || upgrade == GameInfo.V2Level3 || upgrade == GameInfo.V3Level3)
					builder.AddField("Level 3 homing daggers", homing3);
				else if (upgrade == GameInfo.V2Level4 || upgrade == GameInfo.V3Level4)
					builder.AddField("Level 4 homing daggers", homing4);
			}

			return new(null, builder.Build());
		}
	}
}
