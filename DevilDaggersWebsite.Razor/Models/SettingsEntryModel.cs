using DevilDaggersCore.Game;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Web;

namespace DevilDaggersWebsite.Razor.Models
{
	public class SettingsEntryModel : IEntryModel
	{
		public SettingsEntryModel(Entry entry, Player player, IEnumerable<Donation> donations)
		{
			PlayerId = entry.Id;
			Rank = entry.Rank;

			IsBanned = player.IsBanned;
			BanDescription = player.BanDescription;

			Username = entry.Username;

			List<string> titles = player.PlayerTitles.ConvertAll(pt => pt.Title.Name) ?? new();
			if (player.IsPublicDonator(donations))
				titles.Add("Donator");
			Titles = titles.ToArray();

			Dagger dagger = GameInfo.GetDaggerFromTime(GameVersion.V31, entry.Time);
			Death? death = GameInfo.GetDeathByType(GameVersion.V31, entry.DeathType);

			DaggerColor = dagger.Name.ToLower();
			DeathStyle = $"color: #{death?.ColorCode ?? "444"};";

			FlagCode = player.CountryCode ?? string.Empty;
			CountryName = UserUtils.CountryNames.ContainsKey(FlagCode) ? UserUtils.CountryNames[FlagCode] : "Invalid country code";

			HtmlData = new($@"
rank='{entry.Rank}'
flag='{FlagCode}'
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

		public int PlayerId { get; }
		public int Rank { get; }

		public bool IsBanned { get; }
		public string? BanDescription { get; }

		public string Username { get; }
		public string[] Titles { get; }
		public string DaggerColor { get; }
		public string DeathStyle { get; }

		public string FlagCode { get; }
		public string CountryName { get; }

		public HtmlString HtmlData { get; }

		public string BanString => string.Empty;
	}
}
