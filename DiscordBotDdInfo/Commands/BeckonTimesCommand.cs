using DevilDaggersCore.Game;
using DevilDaggersCore.Spawnsets;
using DevilDaggersCore.Spawnsets.Events;
using DiscordBotDdInfo.Extensions;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscordBotDdInfo.Commands
{
	public class BeckonTimesCommand : AbstractCommand
	{
		public override string Name => "beckontimes";

		public CommandResult Execute()
		{
			if (!Spawnset.TryParse(File.ReadAllBytes(Path.Combine("wwwroot", "spawnsets", "V3")), out Spawnset spawnset))
				return CommandResult.GetCustomResponse("Could not parse spawnset.");

			DiscordEmbedBuilder builder = new()
			{
				Color = new DiscordColor(GameInfo.V3Leviathan.ColorCode),
				Title = "Beckon times",
			};

			List<SubEvent> data = spawnset.GenerateSpawnsetEventList(0, 40, 0).OfType<SubEvent>().Where(kvp => kvp.Name.Contains("beckon", StringComparison.InvariantCulture)).ToList();
			int total = data.Count;
			const int part = 16;
			int i = 0;
			while (i < total)
			{
				StringBuilder sb = new();
				for (int j = 0; j < Math.Min(part, total - i); j++)
				{
					SubEvent subEvent = data[i + j];
					sb.AppendLine(subEvent.Seconds.ToString("0.0000", CultureInfo.InvariantCulture));
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
