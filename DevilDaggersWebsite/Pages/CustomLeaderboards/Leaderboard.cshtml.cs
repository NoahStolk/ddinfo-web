using DevilDaggersCore.Extensions;
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages.CustomLeaderboards
{
	public class LeaderboardModel : PageModel
	{
		private readonly ApplicationDbContext dbContext;

		public LeaderboardModel(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public SpawnsetFile? SpawnsetFile { get; private set; }

		[BindProperty]
		public CustomLeaderboard Leaderboard { get; set; }

		[BindProperty]
		public List<CustomEntry> Entries { get; private set; }

		public ActionResult? OnGet(string spawnsetName)
		{
			if (string.IsNullOrEmpty(spawnsetName))
				return RedirectToPage("Index");

			SpawnsetFile = dbContext.SpawnsetFiles.Include(sf => sf.Player).FirstOrDefault(sf => sf.Name == spawnsetName);
			if (SpawnsetFile == null)
				return RedirectToPage("Index");

			Leaderboard = dbContext.CustomLeaderboards
				.Include(l => l.Category)
				.FirstOrDefault(l => l.SpawnsetFileId == SpawnsetFile.Id);
			if (Leaderboard == null)
				return RedirectToPage("Index");

			Entries = dbContext.CustomEntries
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