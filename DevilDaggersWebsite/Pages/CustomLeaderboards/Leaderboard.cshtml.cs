using DevilDaggersCore.Extensions;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Pages.CustomLeaderboards
{
	public class LeaderboardModel : PageModel
	{
		public SpawnsetFile SpawnsetFile { get; private set; }

		[BindProperty]
		public CustomLeaderboard Leaderboard { get; set; }

		[BindProperty]
		public List<CustomEntry> Entries { get; set; }

		private readonly ApplicationDbContext context;

		public IWebHostEnvironment Env { get; }

		public LeaderboardModel(ApplicationDbContext context, IWebHostEnvironment env)
		{
			this.context = context;
			Env = env;
		}

		public ActionResult OnGet(string spawnset)
		{
			if (spawnset == null)
				return RedirectToPage("Index");

			SpawnsetFile = SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(Env, Path.Combine(Env.WebRootPath, "spawnsets", spawnset));

			if (SpawnsetFile == null)
				return RedirectToPage("Index");

			Leaderboard = context.CustomLeaderboards
				.Include(l => l.Category)
				.FirstOrDefault(l => l.SpawnsetFileName == spawnset);

			if (Leaderboard == null)
				return RedirectToPage("Index");

			Entries = context.CustomEntries
				.Where(e => e.CustomLeaderboard == Leaderboard)
				.OrderByMember(Leaderboard.Category.SortingPropertyName, Leaderboard.Category.Ascending)
				.ThenByMember(nameof(CustomEntry.SubmitDate), true)
				.ToList();

			return null;
		}

		public (string daggerName, string seconds) GetDaggerInfo(int daggerIndex)
		{
			if (daggerIndex < 0 || daggerIndex > 4)
				throw new ArgumentOutOfRangeException(nameof(daggerIndex), $"'{nameof(daggerIndex)}' must be between 0 (bronze) and 4 (homing). '{nameof(daggerIndex)}' was {daggerIndex}.");

			string daggerName = daggerIndex switch
			{
				0 => "Bronze",
				1 => "Silver",
				2 => "Golden",
				3 => "Devil",
				_ => "Homing"
			};
			string seconds = daggerIndex switch
			{
				0 => Leaderboard.Bronze.FormatTimeInteger(),
				1 => Leaderboard.Silver.FormatTimeInteger(),
				2 => Leaderboard.Golden.FormatTimeInteger(),
				3 => Leaderboard.Devil.FormatTimeInteger(),
				_ => Leaderboard.Homing.FormatTimeInteger()
			};

			return (daggerName, seconds);
		}
	}
}