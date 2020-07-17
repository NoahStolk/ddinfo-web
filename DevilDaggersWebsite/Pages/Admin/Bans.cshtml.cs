using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.External;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin
{
	public class BansModel : AdminFilePageModel<Ban>
	{
		public List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)> BanInfo { get; private set; } = new List<(Ban ban, string bannedAccountUsername, string responsibleAccountUsername)>();

		public BansModel(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
			: base(httpContextAccessor, env)
		{
		}

		public async Task<ActionResult> OnGetAsync()
		{
			List<Ban> bans = UserUtils.GetUserObjects<Ban>(env);
			IEnumerable<int> userIds = bans.SelectMany(b => b.IdResponsible.HasValue ? new[] { b.Id, b.IdResponsible.Value } : new[] { b.Id });
			Entry[] entries = await Task.WhenAll(userIds.Select(async id => await Hasmodai.GetUserById(id)));

			foreach (Ban ban in bans)
			{
				Entry entry = entries.FirstOrDefault(e => e.Id == ban.Id);

				if (ban.IdResponsible.HasValue)
				{
					Entry entryResponsible = entries.FirstOrDefault(e => e.Id == ban.IdResponsible.Value);
					BanInfo.Add((ban, entry.Username, entryResponsible.Username));
				}
				else
				{
					BanInfo.Add((ban, entry.Username, string.Empty));
				}
			}

			return null;
		}
	}
}