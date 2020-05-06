using CoreBase3.Services;
using DevilDaggersCore;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetBase.Extensions;
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

		public ICommonObjects CommonObjects { get; }

		public LeaderboardModel(ApplicationDbContext context, ICommonObjects commonObjects)
		{
			this.context = context;
			CommonObjects = commonObjects;
		}

		public ActionResult OnGet(string spawnset)
		{
			if (spawnset == null)
				return RedirectToPage("Index");

			SpawnsetFile = SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(CommonObjects, Path.Combine(CommonObjects.Env.WebRootPath, "spawnsets", spawnset));

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
				0 => (Leaderboard.Bronze / 10000f).ToString(FormatUtils.LeaderboardTimeFormat),
				1 => (Leaderboard.Silver / 10000f).ToString(FormatUtils.LeaderboardTimeFormat),
				2 => (Leaderboard.Golden / 10000f).ToString(FormatUtils.LeaderboardTimeFormat),
				3 => (Leaderboard.Devil / 10000f).ToString(FormatUtils.LeaderboardTimeFormat),
				_ => Leaderboard.Category.Ascending ?
Entries.Any(e => e.Time <= Leaderboard.Homing) ?
(Leaderboard.Homing / 10000f).ToString(FormatUtils.LeaderboardTimeFormat)
:
"???"
:
Entries.Any(e => e.Time >= Leaderboard.Homing) ?
(Leaderboard.Homing / 10000f).ToString(FormatUtils.LeaderboardTimeFormat)
:
"???"
			};

			return (daggerName, seconds);
		}
	}
}