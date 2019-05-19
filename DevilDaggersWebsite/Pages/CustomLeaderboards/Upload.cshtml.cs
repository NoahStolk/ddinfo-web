using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using DevilDaggersWebsite.Code.Spawnsets;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace DevilDaggersWebsite.Pages.CustomLeaderboards
{
	public class UploadModel : PageModel
	{
		public class UploadResult
		{
			public bool success;
			public string message;

			public UploadResult(bool success, string message)
			{
				this.success = success;
				this.message = message;
			}
		}

		private static readonly Version DDCLMinimalVersion = new Version("0.2.1.0"); // Update this whenever an old version allows cheating or similar

		public string JsonResult { get; set; }

		private readonly ApplicationDbContext _context;

		public UploadModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public void OnGet(string spawnsetHash, int playerID, string username, float time, int gems, int kills, int deathType, int shotsHit, int shotsFired, int enemiesAlive, int homing, float levelUpTime2, float levelUpTime3, float levelUpTime4, string ddclClientVersion)
		{
			try
			{
				UploadResult result = TryUpload(spawnsetHash, playerID, username, time, gems, kills, deathType, shotsHit, shotsFired, enemiesAlive, homing, levelUpTime2, levelUpTime3, levelUpTime4, ddclClientVersion);
				JsonResult = JsonConvert.SerializeObject(result);
			}
			catch (Exception ex)
			{
				JsonResult = JsonConvert.SerializeObject(new UploadResult(false, $"The server returned an error trying to upload score.\n\nDetails:\n\n{ex}"));
			}
		}

		public UploadResult TryUpload(string spawnsetHash, int playerID, string username, float time, int gems, int kills, int deathType, int shotsHit, int shotsFired, int enemiesAlive, int homing, float levelUpTime2, float levelUpTime3, float levelUpTime4, string ddclClientVersion)
		{
			if (Version.Parse(ddclClientVersion) < DDCLMinimalVersion)
				return new UploadResult(false, "You are using an unsupported and outdated version of DDCL. Please update the program.");

			CustomLeaderboard leaderboard = _context.CustomLeaderboards.Where(l => l.SpawnsetHash == spawnsetHash).FirstOrDefault();

			if (leaderboard == null)
				return new UploadResult(false, "This spawnset doesn't have a leaderboard.");

			CustomEntry entry = _context.CustomEntries.Where(e => e.PlayerID == playerID && e.CustomLeaderboardID == leaderboard.ID).FirstOrDefault();
			if (entry == null)
			{
				// New user on this leaderboard
				_context.CustomEntries.Add(new CustomEntry(playerID, username, time, gems, kills, deathType, shotsHit, shotsFired, enemiesAlive, homing, levelUpTime2, levelUpTime3, levelUpTime4, DateTime.Now) { CustomLeaderboard = leaderboard });

				_context.SaveChanges();
				return new UploadResult(true, $"Welcome to the leaderboard for the {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)} spawnset. Your score is {time.ToString("0.0000")}.");
			}
			else
			{
				// Update the username
				entry.Username = username;

				// Users already on the leaderboard, check for higher score
				if (entry.Time >= time)
				{
					_context.SaveChanges();
					return new UploadResult(true, "No new highscore.");
				}

				float timeImproved = time - entry.Time;

				entry.Time = time;
				entry.Gems = gems;
				entry.Kills = kills;
				entry.DeathType = deathType;
				entry.ShotsHit = shotsHit;
				entry.ShotsFired = shotsFired;
				entry.EnemiesAlive = enemiesAlive;
				entry.Homing = homing;
				entry.LevelUpTime2 = levelUpTime2;
				entry.LevelUpTime3 = levelUpTime3;
				entry.LevelUpTime4 = levelUpTime4;
				entry.SubmitDate = DateTime.Now;

				_context.SaveChanges();
				return new UploadResult(true, $"New highscore: {time.ToString("0.0000")} (+{timeImproved.ToString("0.0000")})");
			}
		}
	}
}