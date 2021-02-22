using DevilDaggersCore.Game;
using DiscordBotDdInfo.Attributes;
using DiscordBotDdInfo.Utils;
using DSharpPlus.Entities;
using System;
using System.Globalization;
using System.Linq;

namespace DiscordBotDdInfo.Commands
{
	public class DefineCommand : AbstractCommand
	{
		public override string Name => "define";

		[SearchCommand]
		public CommandResult Execute(string search)
		{
			if (!EntityUtils.TryGetInfo(search.Split(' '), out string searchResult, out DevilDaggersEntity? entity) || entity == null)
				return new($"No info found using case-insensitive search for the following entities: {searchResult}");

			string htmlName = entity.Name.Replace(" ", "%20", StringComparison.InvariantCulture);

			DiscordEmbedBuilder builder = new() { Color = new DiscordColor(entity.ColorCode), };

			if (entity is Enemy enemy)
			{
				builder.AddField("HP", $"{enemy.Hp} {(enemy.Gems > 1 ? enemy.GetGemHpString() : string.Empty)}");
				builder.AddField("Gems", enemy.Gems.ToString(CultureInfo.InvariantCulture));
				builder.AddField("Death type", enemy.Death?.Name ?? "None");
				if (enemy.SpawnedBy.Count != 0)
					builder.AddField("Spawned by", string.Join(", ", enemy.SpawnedBy.Select(e => e.Name)));
				builder.AddField("Behavior", string.Join("\n", GameInfo.GetEnemyInfo(enemy)));

				builder.Url = "https://devildaggers.info/Wiki/Enemies";

				if (enemy.GameVersion == GameVersion.V3 && enemy == GameInfo.V3Gigapede)
					builder.ImageUrl = $"https://devildaggers.info/Images/Enemies/{htmlName}V3.png";
				else
					builder.ImageUrl = $"https://devildaggers.info/Images/Enemies/{htmlName}.png";

				builder.Thumbnail = new() { Url = "https://devildaggers.info/Images/Icons/skull.png", };
			}
			else if (entity is Upgrade upgrade)
			{
				builder.AddField("Unlocks at", upgrade.UnlocksAt);
				builder.AddField("Spray", $"{upgrade.DefaultSprayPerSecond:0.##} daggers per second");
				builder.AddField("Shot", $"{upgrade.DefaultShot} daggers");
				if (upgrade.HomingSprayPerSecond.HasValue)
					builder.AddField("Homing spray", $"{upgrade.HomingSprayPerSecond.Value:0.##} daggers per second");
				if (upgrade.HomingShot.HasValue)
					builder.AddField("Homing shot", $"{upgrade.HomingShot.Value} daggers");

				builder.Url = $"{NetworkUtils.BaseUrl}/Wiki/Upgrades";
				builder.ImageUrl = $"{NetworkUtils.BaseUrl}/Images/Upgrades/{htmlName}.png";
				builder.Thumbnail = new() { Url = $"{NetworkUtils.BaseUrl}/Images/Icons/gem.png", };
			}
			else if (entity is Dagger dagger)
			{
				builder.Title = $"{dagger.Name} Dagger";
				builder.AddField("Unlocks at", dagger.UnlockSecond.HasValue ? $"{dagger.UnlockSecond.Value} seconds" : "N/A");

				builder.Url = $"{NetworkUtils.BaseUrl}/Wiki/Daggers";
				builder.ImageUrl = $"{NetworkUtils.BaseUrl}/Images/Daggers/{htmlName}.png";
				builder.Thumbnail = new() { Url = $"{NetworkUtils.BaseUrl}/Images/Icons/dagger.png", };
			}
			else if (entity is Death death)
			{
				builder.AddField("Caused by", $"{string.Join(", ", GameInfo.GetEntities<Enemy>(entity.GameVersion).Where(e => e.Death?.DeathType == death.DeathType).Select(e => e.Name))}");
				builder.Thumbnail = new() { Url = $"{NetworkUtils.BaseUrl}/Images/Icons/skull.png", };
			}
			else
			{
				return new($"Entity {entity.Name} is of an undefined type.");
			}

			if (entity is not Dagger)
			{
				builder.Title = $"{entity.Name} ({entity.GameVersion})";
				builder.AddField("Appearances", string.Join(", ", GameInfo.GetAppearances(entity.Name)));
			}

			return new(null, builder.Build());
		}
	}
}
