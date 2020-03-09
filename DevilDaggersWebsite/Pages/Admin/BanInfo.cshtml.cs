using CoreBase.Services;
using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;
using DevilDaggersWebsite.Code.Utils;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class BanInfoModel : AdminPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)> BanInfo { get; private set; } = new List<(Ban, string, string)>();

		public BanInfoModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public async Task<ActionResult> OnGetAsync(string password)
		{
			if (!Authenticate(password))
				return RedirectToPage("/Error/404");

			BanInfo = await GetBanInfo();

			return null;
		}

		public async Task<List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)>> GetBanInfo()
		{
			List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)> list = new List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)>();

			IEnumerable<Ban> bans = UserUtils.GetBans(_commonObjects);
			foreach (Ban ban in bans)
			{
				Entry entry = await Hasmodai.GetUserById(ban.ID);

				if (ban.IDResponsible.HasValue)
				{
					Entry entryResponsible = await Hasmodai.GetUserById(ban.IDResponsible.Value);
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