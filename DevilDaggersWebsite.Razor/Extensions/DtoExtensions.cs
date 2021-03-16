using DevilDaggersCore.Game;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Html;
using System.Web;

namespace DevilDaggersWebsite.Razor.Extensions
{
	public static class DtoExtensions
	{
		public static HtmlString ToHtmlData(this Entry entry, string flagCode, Player player)
		{
			return new($@"
rank='{entry.Rank}'
flag='{flagCode}'
username='{HttpUtility.HtmlEncode(entry.Username)}'
time='{entry.Time}'
e-dpi='{player.Edpi * 1000 ?? 0}'
dpi='{player.Dpi ?? 0}'
in-game-sens='{player.InGameSens * 1000 ?? 0}'
fov='{player.Fov ?? 0}'
hand='{(!player.RightHanded.HasValue ? -1 : player.RightHanded.Value ? 1 : 0)}'
flash='{(!player.FlashEnabled.HasValue ? -1 : player.FlashEnabled.Value ? 1 : 0)}'
gamma='{player.Gamma * 1000 ?? 0}'");
		}

		public static HtmlString ToHtmlData(this Entry entry, string flagCode, GameVersion gameVersion)
		{
			ulong deathsTotal = entry.DeathsTotal == 0 ? 1 : entry.DeathsTotal;
			return new($@"
rank='{entry.Rank}'
flag='{flagCode}'
username='{HttpUtility.HtmlEncode(entry.Username)}'
time='{entry.Time}'
kills='{entry.Kills}'
gems='{entry.Gems}'
accuracy='{entry.Accuracy * 10000:0}'
death-type='{GameInfo.GetDeathByType(gameVersion, entry.DeathType)?.Name ?? "Unknown"}'
total-time='{entry.TimeTotal}'
total-kills='{entry.KillsTotal}'
total-gems='{entry.GemsTotal}'
total-accuracy='{entry.AccuracyTotal * 10000:0}'
total-deaths='{entry.DeathsTotal}'
daggers-hit='{entry.DaggersHit}'
daggers-fired='{entry.DaggersFired}'
total-daggers-hit='{entry.DaggersHitTotal}'
total-daggers-fired='{entry.DaggersFiredTotal}'
average-time='{entry.TimeTotal * 10000f / deathsTotal:0}'
average-kills='{entry.KillsTotal * 100f / deathsTotal:0}'
average-gems='{entry.GemsTotal * 100f / deathsTotal:0}'
average-daggers-hit='{entry.DaggersHitTotal * 100f / deathsTotal:0}'
average-daggers-fired='{entry.DaggersFiredTotal * 100f / deathsTotal:0}'
time-by-death='{entry.Time * 10000f / deathsTotal:0}'");
		}
	}
}
