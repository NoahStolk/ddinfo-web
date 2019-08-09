using CoreBase.Services;
using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersCore.Spawnsets;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetBase.Encryption;
using NetBase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DevilDaggersWebsite.Pages.CustomLeaderboards
{
	public class UploadModel : PageModel
	{
		public string JsonResult { get; set; }

		private readonly ApplicationDbContext _context;
		private readonly ICommonObjects _commonObjects;

		public UploadModel(ApplicationDbContext context, ICommonObjects commonObjects)
		{
			_context = context;
			_commonObjects = commonObjects;
		}

		public void OnGet(string spawnsetHash, int playerID, string username, float time, int gems, int kills, int deathType, int shotsHit, int shotsFired, int enemiesAlive, int homing, float levelUpTime2, float levelUpTime3, float levelUpTime4, string ddclClientVersion, string v)
		{
			try
			{
				UploadResult result = TryUpload(spawnsetHash, playerID, username, time, gems, kills, deathType, shotsHit, shotsFired, enemiesAlive, homing, levelUpTime2, levelUpTime3, levelUpTime4, ddclClientVersion, v);
				JsonResult = JsonConvert.SerializeObject(result);
			}
			catch (Exception ex)
			{
				JsonResult = JsonConvert.SerializeObject(new UploadResult(false, $"The server returned an error trying to upload score.\n\nDetails:\n\n{ex}", 10));
			}
		}

		private UploadResult TryUpload(string spawnsetHash, int playerID, string username, float time, int gems, int kills, int deathType, int shotsHit, int shotsFired, int enemiesAlive, int homing, float levelUpTime2, float levelUpTime3, float levelUpTime4, string clientVersion, string validation)
		{
			Version clientVersionParsed = Version.Parse(clientVersion);
			if (clientVersionParsed < Version.Parse(ToolUtils.Tools.Where(t => t.Name == "DDCL").FirstOrDefault().VersionNumberRequired))
				return new UploadResult(false, "You are using an unsupported and outdated version of DDCL. Please update the program.");

			string spawnsetName = string.Empty;
			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets")))
			{
				string hash = string.Empty;
				
				using (FileStream fs = new FileStream(spawnsetPath, FileMode.Open, FileAccess.Read))
					if (Spawnset.TryParse(fs, out Spawnset spawnsetObject))
						hash = spawnsetObject.GetHashString();

				if (hash == spawnsetHash)
				{
					spawnsetName = Path.GetFileName(spawnsetPath);
					break;
				}
			}

			if (string.IsNullOrEmpty(spawnsetName))
				return new UploadResult(false, "This spawnset does not exist on DevilDaggers.info.");

			string check = string.Join(";",
				playerID,
				username,
				time,
				gems,
				kills,
				deathType,
				shotsHit,
				shotsFired,
				enemiesAlive,
				homing,
				string.Join(",", new float[3] { levelUpTime2, levelUpTime3, levelUpTime4 }));
			if (DecryptValidation(validation) != check)
				return new UploadResult(false, "Invalid submission.");

			CustomLeaderboard leaderboard = _context.CustomLeaderboards.Include(l => l.Category).Where(l => l.SpawnsetFileName == spawnsetName).FirstOrDefault(); // TODO: Multiple leaderboards
			if (leaderboard == null)
				return new UploadResult(false, "This spawnset doesn't have a leaderboard.");

			if ((leaderboard.Category.Ascending || leaderboard.Category.SortingPropertyName != "Time") && clientVersionParsed <= new Version(0, 4, 0, 1))
				return new UploadResult(false, $"This version of DDCL does not support custom leaderboards of the category '{leaderboard.Category.Name}'.");

			if (leaderboard.Category.Name == "Challenge" && clientVersionParsed <= new Version(0, 4, 3, 0))
				return new UploadResult(false, $"This version of DDCL does not support custom leaderboards of the category '{leaderboard.Category.Name}'.");

			// Submission is accepted.

			// Fix any broken values.
			homing = Math.Max(0, homing);

			// Update the date this leaderboard was submitted to.
			leaderboard.DateLastPlayed = DateTime.Now;

			// Calculate the new rank.
			List<CustomEntry> entries = _context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToList();
			int rank = leaderboard.Category.Ascending ? entries.Where(e => e.Time < time).Count() + 1 : entries.Where(e => e.Time > time).Count() + 1; // TODO: Use reflection to use Category.SortingPropertyName.
			int totalPlayers = entries.Count();

			CustomEntry entry = _context.CustomEntries.Where(e => e.PlayerID == playerID && e.CustomLeaderboardID == leaderboard.ID).FirstOrDefault();
			if (entry == null)
			{
				// Add new user to this leaderboard.
				_context.CustomEntries.Add(new CustomEntry(playerID, username, time, gems, kills, deathType, shotsHit, shotsFired, enemiesAlive, homing, levelUpTime2, levelUpTime3, levelUpTime4, DateTime.Now, clientVersion) { CustomLeaderboard = leaderboard });

				_context.SaveChanges();
				return new UploadResult(true, $@"Welcome to the leaderboard for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}.

{$"Rank".PadRight(20)}{rank} / {++totalPlayers}
{$"Score".PadRight(20)}{time.ToString("0.0000")}");
			}
			else
			{
				// Update the username.
				foreach (CustomEntry en in _context.CustomEntries.Where(e => e.PlayerID == entry.PlayerID))
					en.Username = username;
					
				// User is already on the leaderboard, check for better score.
				if ((leaderboard.Category.Ascending && entry.Time <= time) // TODO: Use reflection to use Category.SortingPropertyName.
				 || (!leaderboard.Category.Ascending && entry.Time >= time))
				{
					_context.SaveChanges();
					return new UploadResult(true, $"No new highscore for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}.");
				}

				// Calculate the old rank.
				int oldRank = leaderboard.Category.Ascending ? entries.Where(e => e.Time < time).Count() + 1 : entries.Where(e => e.Time > time).Count() + 1;

				double accuracy = shotsFired == 0 ? 0 : shotsHit / (double)shotsFired;

				int rankDiff = oldRank - rank;
				float timeDiff = time - entry.Time;
				int killsDiff = kills - entry.Kills;
				int gemsDiff = gems - entry.Gems;
				double accuracyDiff = accuracy - (entry.ShotsFired == 0 ? 0 : entry.ShotsHit / (double)entry.ShotsFired);
				int enemiesAliveDiff = enemiesAlive - entry.EnemiesAlive;
				int homingDiff = homing - entry.Homing;
				float levelUpTime2Diff = levelUpTime2 - entry.LevelUpTime2;
				float levelUpTime3Diff = levelUpTime3 - entry.LevelUpTime3;
				float levelUpTime4Diff = levelUpTime4 - entry.LevelUpTime4;

				entry.Time = time;
				entry.Kills = kills;
				entry.Gems = gems;
				entry.DeathType = deathType;
				entry.ShotsHit = shotsHit;
				entry.ShotsFired = shotsFired;
				entry.EnemiesAlive = enemiesAlive;
				entry.Homing = homing;
				entry.LevelUpTime2 = levelUpTime2;
				entry.LevelUpTime3 = levelUpTime3;
				entry.LevelUpTime4 = levelUpTime4;
				entry.SubmitDate = DateTime.Now;
				entry.ClientVersion = clientVersion;

				_context.SaveChanges();

				int width = 15;
				return new UploadResult(true, $@"NEW HIGHSCORE for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}!
				
{$"Rank".ToString().PadRight(width)}{$"{rank} / {totalPlayers}".PadRight(width)}({rankDiff.ToString("+0;-#")})
{$"Time".ToString().PadRight(width)}{time.ToString("0.0000").PadRight(width)}({(timeDiff < 0 ? "" : "+")}{timeDiff.ToString("0.0000")})
{$"Kills".ToString().PadRight(width)}{kills.ToString().PadRight(width)}({killsDiff.ToString("+0;-#")})
{$"Gems".ToString().PadRight(width)}{gems.ToString().PadRight(width)}({gemsDiff.ToString("+0;-#")})
{$"Accuracy".ToString().PadRight(width)}{accuracy.ToString("0.00%").PadRight(width)}({(accuracyDiff < 0 ? "" : "+")}{accuracyDiff.ToString("0.00%")})
{$"Enemies Alive".ToString().PadRight(width)}{enemiesAlive.ToString().PadRight(width)}({enemiesAliveDiff.ToString("+0;-#")})
{$"Homing".ToString().PadRight(width)}{homing.ToString().PadRight(width)}({homingDiff.ToString("+0;-#")})
{$"Level 2".ToString().PadRight(width)}{levelUpTime2.ToString("0.0000").PadRight(width)}({(levelUpTime2Diff < 0 ? "" : "+")}{levelUpTime2Diff.ToString("0.0000")})
{$"Level 3".ToString().PadRight(width)}{levelUpTime3.ToString("0.0000").PadRight(width)}({(levelUpTime3Diff < 0 ? "" : "+")}{levelUpTime3Diff.ToString("0.0000")})
{$"Level 4".ToString().PadRight(width)}{levelUpTime4.ToString("0.0000").PadRight(width)}({(levelUpTime4Diff < 0 ? "" : "+")}{levelUpTime4Diff.ToString("0.0000")})");
			}
		}

		private string DecryptValidation(string validation)
		{
			try
			{
				AesBase32Wrapper aes = new AesBase32Wrapper("4GDdtUpDelr2wIae", "xx7SXitvxQh4tJzn", "K0sfsKXLZKmKs929");
				return aes.DecodeAndDecrypt(HttpUtility.HtmlDecode(validation));
			}
			catch (Exception ex)
			{
				throw new Exception($"Could not decrypt {validation}", ex);
			}
		}
	}
}