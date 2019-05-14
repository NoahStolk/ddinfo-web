using CoreBase.Services;
using DevilDaggersCore.Leaderboard;
using DevilDaggersWebsite.Models.PageModels;
using DevilDaggersWebsite.Models.User;
using DevilDaggersWebsite.Utils;
using DevilDaggersWebsite.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class BanInfoModel : AdminPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public List<Tuple<Ban, string, string>> BanInfo { get; private set; } = new List<Tuple<Ban, string, string>>();

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

		public async Task<List<Tuple<Ban, string, string>>> GetBanInfo()
		{
			List<Tuple<Ban, string, string>> list = new List<Tuple<Ban, string, string>>();

			IEnumerable<Ban> bans = UserUtils.GetBans(_commonObjects);
			foreach (Ban ban in bans)
			{
				Entry entry = await Hasmodai.GetUserByID(ban.ID);

				if (ban.IDResponsible.HasValue)
				{
					Entry entryResponsible = await Hasmodai.GetUserByID(ban.IDResponsible.Value);
					list.Add(Tuple.Create(ban, entry.Username, entryResponsible.Username));
				}
				else
				{
					list.Add(Tuple.Create(ban, entry.Username, string.Empty));
				}
			}

			return list;
		}
	}
}