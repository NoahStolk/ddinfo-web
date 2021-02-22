using DevilDaggersCore.Game;
using DevilDaggersCore.Spawnsets;
using DevilDaggersCore.Spawnsets.Events;
using DiscordBotDdInfo.Attributes;
using DiscordBotDdInfo.Extensions;
using DiscordBotDdInfo.Utils;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscordBotDdInfo.Commands
{
	public class SpawnTimesCommand : AbstractCommand
	{
		public override string Name => "spawntimes";

		[SearchCommand]
		public CommandResult Execute(string search)
		{
			if (!EntityUtils.TryGetInfo(search.Split(' '), out string searchResult, out Enemy? enemy) || enemy == null)
				return new($"No info found using case-insensitive search for the following entities: {searchResult}");

			if (!Spawnset.TryParse(File.ReadAllBytes(Path.Combine("wwwroot", "spawnsets", enemy.GameVersion.ToString())), out Spawnset spawnset))
				return CommandResult.GetCustomResponse("Could not parse spawnset.");

			DiscordEmbedBuilder builder = new()
			{
				Color = new DiscordColor(enemy.ColorCode),
				Title = $"{enemy.Name} spawns",
			};

			List<SpawnEvent> data = spawnset.GenerateSpawnsetEventList(0, 0, 28).OfType<SpawnEvent>().Where(kvp => kvp.Name.Contains(enemy.Name, StringComparison.InvariantCulture)).ToList();
			int total = data.Count;
			const int part = 16;
			int i = 0;
			while (i < total)
			{
				StringBuilder sb = new();
				for (int j = 0; j < Math.Min(part, total - i); j++)
				{
					SpawnEvent spawnEvent = data[i + j];
					sb.AppendLine(spawnEvent.Seconds.ToString("0.0000", CultureInfo.InvariantCulture));
				}

				i += part;

				// Cannot add more than 25 fields...
				if (i > part * 25)
					break;

				builder.AddFieldObject($"{i / part} / {total / part + 1}", $"`{string.Join($"`{Environment.NewLine}`", sb.ToString().TrimEnd().Split(Environment.NewLine))}`", true);
			}

			return new(null, builder.Build());
		}
	}
}
