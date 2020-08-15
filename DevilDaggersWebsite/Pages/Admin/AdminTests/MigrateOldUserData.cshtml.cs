using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.External;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Old = DevilDaggersWebsite.Code.Users;

namespace DevilDaggersWebsite.Pages.Admin.AdminTests
{
	public class MigrateOldUserDataModel : PageModel
	{
		public MigrateOldUserDataModel(IWebHostEnvironment env, ApplicationDbContext applicationDbContext)
		{
			Env = env;
			ApplicationDbContext = applicationDbContext;
		}

		public IWebHostEnvironment Env { get; }
		public ApplicationDbContext ApplicationDbContext { get; }

		public async Task OnGetAsync()
		{
			if (!ApplicationDbContext.Donations.Any())
			{
				ApplicationDbContext.Donations.AddRange(MigrateDonations());
				ApplicationDbContext.SaveChanges();
			}

			if (!ApplicationDbContext.Titles.Any())
			{
				ApplicationDbContext.Titles.AddRange(MigrateTitles());
				ApplicationDbContext.SaveChanges();
			}

			if (!ApplicationDbContext.Players.Any())
			{
				ApplicationDbContext.Players.AddRange(await MigratePlayers());
				ApplicationDbContext.SaveChanges();
			}

			if (!ApplicationDbContext.AssetMods.Any())
			{
				ApplicationDbContext.AssetMods.AddRange(MigrateAssetMods());
				ApplicationDbContext.SaveChanges();
			}

			foreach (Old.User user in UserUtils.GetUserObjects<Old.User>(Env))
			{
				Player player = ApplicationDbContext.Players.FirstOrDefault(p => p.Id == user.Id);

				List<Title> titles = ApplicationDbContext.Titles.Where(t => user.Titles != null && user.Titles.Contains(t.Name)).ToList();
				List<PlayerTitle> playerTitles = titles.Select(title => new PlayerTitle { Title = title, TitleId = title.Id, Player = player, PlayerId = player.Id }).ToList();
				player.PlayerTitles = playerTitles;
			}

			ApplicationDbContext.SaveChanges();

			foreach (Old.AssetMod oldMod in UserUtils.GetUserObjects<Old.AssetMod>(Env))
			{
				AssetMod newMod = ApplicationDbContext.AssetMods.FirstOrDefault(am => am.Name == oldMod.Name);
				List<Player> modAuthors = ApplicationDbContext.Players.Where(p => oldMod.AuthorIds.Contains(p.Id)).ToList();

				List<PlayerAssetMod> playerAssetMods = modAuthors.Select(player => new PlayerAssetMod { Player = player, PlayerId = player.Id, AssetMod = newMod, AssetModId = newMod.Id }).ToList();
				newMod.PlayerAssetMods = playerAssetMods;
			}

			ApplicationDbContext.SaveChanges();
		}

		private List<Donation> MigrateDonations()
			=> UserUtils.GetUserObjects<Old.Donation>(Env)
				.Select(d => new Donation
				{
					Amount = d.Amount,
					ConvertedEuroCentsReceived = d.ConvertedEuroCentsReceived,
					Currency = (Currency)d.Currency,
					DateReceived = d.DateReceived,
					IsRefunded = d.IsRefunded,
					Note = d.Note,
					PlayerId = d.DonatorId,
				})
				.ToList();

		private List<Title> MigrateTitles()
			=> UserUtils.GetUserObjects<Old.User>(Env).Where(u => u.Titles != null && u.Titles.Any()).SelectMany(u => u.Titles).Distinct().Select(s => new Title { Name = s }).ToList();

		private async Task<List<Player>> MigratePlayers()
		{
			List<Player> players = new List<Player>();

			foreach (Old.User user in UserUtils.GetUserObjects<Old.User>(Env))
			{
				players.Add(new Player
				{
					Id = user.Id,
					Username = user.Username,
					IsAnonymous = user.IsAnonymous,
				});
			}

			foreach (Old.PlayerSetting setting in UserUtils.GetUserObjects<Old.PlayerSetting>(Env))
			{
				Player? player = players.FirstOrDefault(p => p.Id == setting.Id);
				if (player == null)
				{
					player = new Player
					{
						Username = (await HasmodaiUtils.GetUserById(setting.Id)).Username,
						Id = setting.Id,
						Dpi = setting.Dpi,
						FlashEnabled = setting.FlashEnabled,
						Fov = setting.Fov,
						Gamma = setting.Gamma,
						InGameSens = setting.InGameSens,
						RightHanded = setting.RightHanded,
					};
					players.Add(player);
				}
				else
				{
					players.Remove(players.FirstOrDefault(p => p.Id == setting.Id));

					player.Username ??= (await HasmodaiUtils.GetUserById(player.Id)).Username;
					player.Dpi = setting.Dpi;
					player.FlashEnabled = setting.FlashEnabled;
					player.Fov = setting.Fov;
					player.Gamma = setting.Gamma;
					player.InGameSens = setting.InGameSens;
					player.RightHanded = setting.RightHanded;

					players.Add(player);
				}
			}

			foreach (Old.Flag flag in UserUtils.GetUserObjects<Old.Flag>(Env))
			{
				Player? player = players.FirstOrDefault(p => p.Id == flag.Id);
				if (player == null)
				{
					player = new Player
					{
						Username = (await HasmodaiUtils.GetUserById(flag.Id)).Username,
						Id = flag.Id,
						CountryCode = flag.CountryCode,
					};
					players.Add(player);
				}
				else
				{
					players.Remove(players.FirstOrDefault(p => p.Id == flag.Id));

					player.Username ??= (await HasmodaiUtils.GetUserById(player.Id)).Username;
					player.CountryCode = flag.CountryCode;

					players.Add(player);
				}
			}

			foreach (Old.Ban ban in UserUtils.GetUserObjects<Old.Ban>(Env))
			{
				Player? player = players.FirstOrDefault(p => p.Id == ban.Id);
				if (player == null)
				{
					player = new Player
					{
						Username = (await HasmodaiUtils.GetUserById(ban.Id)).Username,
						Id = ban.Id,
						IsBanned = true,
						BanDescription = ban.Description,
						BanResponsibleId = ban.IdResponsible,
					};
					players.Add(player);
				}
				else
				{
					players.Remove(players.FirstOrDefault(p => p.Id == ban.Id));

					player.Username ??= (await HasmodaiUtils.GetUserById(player.Id)).Username;
					player.IsBanned = true;
					player.BanDescription = ban.Description;
					player.BanResponsibleId = ban.IdResponsible;

					players.Add(player);
				}
			}

			return players;
		}

		private List<AssetMod> MigrateAssetMods()
		{
			List<AssetMod> assetMods = new List<AssetMod>();

			foreach (Old.AssetMod assetMod in UserUtils.GetUserObjects<Old.AssetMod>(Env))
			{
				foreach (int author in assetMod.AuthorIds)
				{
					if (!ApplicationDbContext.Players.Any(p => p.Id == author))
						ApplicationDbContext.Players.Add(new Player { Id = author });
				}

				assetMods.Add(new AssetMod
				{
					AssetModFileContents = (AssetModFileContents)assetMod.AssetModFileContents,
					AssetModTypes = (AssetModTypes)assetMod.AssetModType,
					Name = assetMod.Name,
					Url = new Uri(assetMod.Url),
				});
			}

			return assetMods;
		}
	}
}