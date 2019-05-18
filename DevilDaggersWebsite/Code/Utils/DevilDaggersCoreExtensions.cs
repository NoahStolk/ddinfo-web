using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboard;
using DevilDaggersWebsite.Code.Users;
using Microsoft.AspNetCore.Html;
using System.Web;

namespace DevilDaggersWebsite.Code.Utils
{
	public static class DevilDaggersCoreExtensions
	{
		public static HtmlString ToHTMLData(this Entry entry, string flagCode)
		{
			return new HtmlString($@"
				rank='{entry.Rank}'
				flag='{flagCode}'
				username='{HttpUtility.HtmlEncode(entry.Username)}'
				time='{entry.Time}'
				kills='{entry.Kills}'
				gems='{entry.Gems}'
				accuracy='{(entry.Accuracy * 10000).ToString("0")}'
				death-type='{Game.GetDeathFromDeathType(entry.DeathType).Name}'
				total-time='{entry.TimeTotal}'
				total-kills='{entry.KillsTotal}'
				total-gems='{entry.GemsTotal}'
				total-accuracy='{(entry.AccuracyTotal * 10000).ToString("0")}'
				total-deaths='{entry.DeathsTotal}'
			");
		}

		public static HtmlString ToHTMLData(this Entry entry, string flagCode, PlayerSetting playerSetting)
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
	}
}