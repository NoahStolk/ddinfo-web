using DevilDaggersCore.Game;
using DevilDaggersCore.Utils;
using DiscordBotDdInfo.Clients;
using DSharpPlus.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotDdInfo.Commands
{
	public class WorldRecordCommand : AbstractCommand
	{
		public override string Name => "wr";

		public async Task<CommandResult> Execute()
			=> await Execute(DateTime.Now);

		public async Task<CommandResult> Execute(DateTime dateTime)
		{
			if (dateTime > DateTime.Now)
			{
				int days = (dateTime - DateTime.Now).Days;
				return CommandResult.GetCustomResponse($"Wait {days} day{(days == 1 ? string.Empty : "s")} and I'll tell you.");
			}

			WorldRecord? worldRecord = (await NetworkUtils.ApiClient.LeaderboardHistory_GetWorldRecordsAsync(dateTime)).FirstOrDefault();
			if (worldRecord == null)
				return CommandResult.GetCustomResponse("The world was not ready for Devil Daggers yet.");

			DiscordEmbedBuilder builder = new()
			{
				Title = $"World record on {dateTime.ToString(FormatUtils.DateFormat, CultureInfo.InvariantCulture)} ({GameInfo.GetGameVersionFromDate(dateTime)?.ToString() ?? "Pre-release"})",
				Url = $"{NetworkUtils.BaseUrl}/Leaderboard/WorldRecordProgression",
				Thumbnail = new() { Url = $"{NetworkUtils.BaseUrl}/Images/Icons/stopwatch.png", },
			};

			// TODO: Add field for lastest leaderboard history update.
			builder.AddField("Date set", worldRecord.DateTime.ToString(FormatUtils.DateFormat, CultureInfo.InvariantCulture), true);
			builder.AddField("Username", worldRecord.Entry.Username, true);
			builder.AddField("Time", (worldRecord.Entry.Time / 10000f).ToString(FormatUtils.LeaderboardTimeFormat, CultureInfo.InvariantCulture), true);
			builder.AddField("Gems", worldRecord.Entry.Gems == 0 ? "?" : worldRecord.Entry.Gems.ToString(CultureInfo.InvariantCulture), true);
			builder.AddField("Kills", worldRecord.Entry.Kills == 0 ? "?" : worldRecord.Entry.Kills.ToString(CultureInfo.InvariantCulture), true);
			builder.AddField("Accuracy", worldRecord.Entry.DaggersFired == 0 ? "?" : $"{worldRecord.Entry.DaggersHit / (float)worldRecord.Entry.DaggersFired * 100:0.00}% ({FormatUtils.FormatDaggersInt32(worldRecord.Entry.DaggersHit, worldRecord.Entry.DaggersFired, true)})", true);
			builder.AddField("Death type", GameInfo.GetEntities<DevilDaggersCore.Game.Death>().Find(d => d.DeathType == worldRecord.Entry.DeathType)?.Name ?? "?", true);
			return new(null, builder.Build());
		}
	}
}
