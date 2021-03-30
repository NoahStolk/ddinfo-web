using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class PlayerModel : PageModel
	{
		private readonly IWebHostEnvironment _env;
		private readonly ApplicationDbContext _dbContext;

		private int _userId;

		public PlayerModel(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			_env = env;
			_dbContext = dbContext;
		}

		[BindProperty]
		public Entry? Entry { get; set; }

		public int UserId
		{
			get => _userId;
			set
			{
				if (value <= 0)
					return;
				_userId = value;
			}
		}

		public bool IsValidTop100Graph { get; private set; }
		public string? UsernameAliases { get; private set; }

		public async Task OnGetAsync(int userId)
		{
			UserId = Math.Max(1, userId);

			Entry = await LeaderboardClient.Instance.GetUserById(UserId);

			if (Entry != null)
			{
				IsValidTop100Graph = UserId > 0 && Entry.ExistsInHistory(_env);
				IEnumerable<string> aliases = Entry.GetAllUsernameAliases(_env).Where(s => s != Entry.Username);
				UsernameAliases = aliases.Any() ? $" (also known as: {string.Join(", ", aliases)})" : string.Empty;
			}
		}
	}
}
