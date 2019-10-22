using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Tools;
using DevilDaggersWebsite.Code.Users;
using Microsoft.AspNetCore.Html;
using System.Text;
using System.Web;

namespace DevilDaggersWebsite.Code.Utils
{
	public static class DevilDaggersCoreExtensions
	{
		public static HtmlString ToHtmlData(this Entry entry, string flagCode)
		{
			return new HtmlString($@"
				rank='{entry.Rank}'
				flag='{flagCode}'
				username='{HttpUtility.HtmlEncode(entry.Username)}'
				time='{entry.Time}'
				kills='{entry.Kills}'
				gems='{entry.Gems}'
				accuracy='{(entry.Accuracy * 10000).ToString("0")}'
				death-type='{GameInfo.GetDeathFromDeathType(entry.DeathType).Name}'
				total-time='{entry.TimeTotal}'
				total-kills='{entry.KillsTotal}'
				total-gems='{entry.GemsTotal}'
				total-accuracy='{(entry.AccuracyTotal * 10000).ToString("0")}'
				total-deaths='{entry.DeathsTotal}'
			");
		}

		public static HtmlString ToHtmlData(this Entry entry, string flagCode, PlayerSetting playerSetting)
		{
			return new HtmlString($@"
				rank='{entry.Rank}'
				flag='{flagCode}'
				username='{HttpUtility.HtmlEncode(entry.Username)}'
				time='{entry.Time}'
				e-dpi='{playerSetting.EDPI * 1000 ?? 0}'
				dpi='{playerSetting.DPI ?? 0}'
				in-game-sens='{playerSetting.InGameSens * 1000 ?? 0}'
				fov='{playerSetting.FOV ?? 0}'
				hand='{(!playerSetting.RightHanded.HasValue ? -1 : playerSetting.RightHanded.Value ? 1 : 0)}'
				flash='{(!playerSetting.FlashEnabled.HasValue ? -1 : playerSetting.FlashEnabled.Value ? 1 : 0)}'
			");
		}

		public static HtmlString ToChangeLogHtmlString(this Tool tool)
		{
			StringBuilder sb = new StringBuilder();
			foreach (ChangeLogEntry entry in tool.ChangeLog)
			{
				sb.Append($"<h3>{entry.VersionNumber} - {entry.Date.ToString("MMMM dd, yyyy")}</h3><ul>");
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