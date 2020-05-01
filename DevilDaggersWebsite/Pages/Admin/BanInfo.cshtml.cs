using CoreBase3.Services;
using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;
using DevilDaggersWebsite.Code.Utils;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class BanInfoModel : AdminPageModel
	{
		private readonly ICommonObjects commonObjects;

		public List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)> BanInfo { get; private set; } = new List<(Ban, string, string)>();

		public BanInfoModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;
		}

		public async Task<ActionResult> OnGetAsync(string password)
		{
			if (!Authenticate(password))
				return RedirectToPage("/Error/404");

			IEnumerable<Ban> bans = UserUtils.GetBans(commonObjects);
			IEnumerable<int> userIds = bans.SelectMany(b => b.IdResponsible.HasValue ? new[] { b.Id, b.IdResponsible.Value } : new[] { b.Id });
			Entry[] entries = await Task.WhenAll(userIds.Select(async id => await Hasmodai.GetUserById(id)));

			BanInfo = GetBanInfo(bans, entries);

			return null;
		}

		private List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)> GetBanInfo(IEnumerable<Ban> bans, Entry[] entries)
		{
			List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)> list = new List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)>();

			foreach (Ban ban in bans)
			{
				Entry entry = entries.FirstOrDefault(e => e.Id == ban.Id);

				if (ban.IdResponsible.HasValue)
				{
					Entry entryResponsible = entries.FirstOrDefault(e => e.Id == ban.IdResponsible.Value);
					list.Add((ban, entry.Username, entryResponsible.Username));
				}
				else
				{
					list.Add((ban, entry.Username, string.Empty));
				}
			}

			return list;
		}
	}
}