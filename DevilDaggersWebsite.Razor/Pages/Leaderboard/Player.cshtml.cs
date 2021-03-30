using DevilDaggersCore.Game;
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

		private int _playerId;

		public PlayerModel(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			_env = env;
			_dbContext = dbContext;
		}

		[BindProperty]
		public Entry? Entry { get; set; }

		public int PlayerId
		{
			get => _playerId;
			set
			{
				if (value <= 0)
					return;
				_playerId = value;
			}
		}

		public Death? Death { get; private set; }
		public bool HasValidTop100Graph { get; private set; }
		public string? UsernameAliases { get; private set; }

		public async Task OnGetAsync(int id)
		{
			PlayerId = Math.Max(1, id);

			Entry = await LeaderboardClient.Instance.GetUserById(PlayerId);

			if (Entry != null)
			{
				Death = GameInfo.GetDeathByType(GameVersion.V31, Entry.DeathType);
				HasValidTop100Graph = Entry.ExistsInHistory(_env);
				IEnumerable<string> aliases = Entry.GetAllUsernameAliases(_env).Where(s => s != Entry.Username);
				UsernameAliases = aliases.Any() ? $" (also known as: {string.Join(", ", aliases)})" : string.Empty;
			}
		}
	}
}
