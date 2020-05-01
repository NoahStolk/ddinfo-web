using CoreBase3.Services;
using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Tools.Website;
using DevilDaggersWebsite.Code.Users;
using Microsoft.AspNetCore.Html;
using NetBase.Utils;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DevilDaggersWebsite.Code.Utils
{
	public static class DevilDaggersCoreExtensions
	{
		public static HtmlString ToHtmlData(this Entry entry, string flagCode) => new HtmlString($@"
			rank='{entry.Rank}'
			flag='{flagCode}'
			username='{HttpUtility.HtmlEncode(entry.Username)}'
			time='{entry.Time}'
			kills='{entry.Kills}'
			gems='{entry.Gems}'
			accuracy='{entry.Accuracy * 10000:0}'
			death-type='{GameInfo.GetDeathFromDeathType(entry.DeathType).Name}'
			total-time='{entry.TimeTotal}'
			total-kills='{entry.KillsTotal}'
			total-gems='{entry.GemsTotal}'
			total-accuracy='{entry.AccuracyTotal * 10000:0}'
			total-deaths='{entry.DeathsTotal}'
		");

		public static HtmlString ToHtmlData(this Entry entry, string flagCode, PlayerSetting playerSetting) => new HtmlString($@"
			rank='{entry.Rank}'
			flag='{flagCode}'
			username='{HttpUtility.HtmlEncode(entry.Username)}'
			time='{entry.Time}'
			e-dpi='{playerSetting.Edpi * 1000 ?? 0}'
			dpi='{playerSetting.Dpi ?? 0}'
			in-game-sens='{playerSetting.InGameSens * 1000 ?? 0}'
			fov='{playerSetting.Fov ?? 0}'
			hand='{(!playerSetting.RightHanded.HasValue ? -1 : playerSetting.RightHanded.Value ? 1 : 0)}'
			flash='{(!playerSetting.FlashEnabled.HasValue ? -1 : playerSetting.FlashEnabled.Value ? 1 : 0)}'
		");

		public static bool ExistsInHistory(this Entry entry, ICommonObjects commonObjects)
		{
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(commonObjects.Env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath, Encoding.UTF8));
				if (leaderboard.Entries.Any(e => e.Id == entry.Id))
					return true;
			}
			return false;
		}

		public static HtmlString ToChangelogHtmlString(this Tool tool)
		{
			StringBuilder sb = new StringBuilder();
			foreach (ChangelogEntry entry in tool.Changelog)
			{
				sb.Append($"<h3>{entry.VersionNumber} - {entry.Date:MMMM dd, yyyy}</h3><ul>");
				foreach (Change change in entry.Changes)
					sb.Append(change.ToHtmlString());
				sb.Append("</ul>");
			}
			return new HtmlString(sb.ToString());
		}

		public static HtmlString ToHtmlString(this Change change)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"<li>{change.Description}</li>");
			if (change.SubChanges != null && change.SubChanges.Count != 0)
				foreach (Change subChange in change.SubChanges)
					sb.Append($"<ul>{subChange.ToHtmlString()}</ul>");
			return new HtmlString(sb.ToString());
		}
	}
}