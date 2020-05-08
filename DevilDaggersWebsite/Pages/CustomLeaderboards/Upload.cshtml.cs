using CoreBase3.Services;
using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersCore.Spawnsets;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetBase.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace DevilDaggersWebsite.Pages.CustomLeaderboards
{
	public class UploadModel : PageModel
	{
		public string JsonResult { get; set; }

		private readonly ApplicationDbContext context;
		private readonly ICommonObjects commonObjects;

		public UploadModel(ApplicationDbContext context, ICommonObjects commonObjects)
		{
			this.context = context;
			this.commonObjects = commonObjects;
		}

		public void OnGet(string spawnsetHash,
			int playerId,
			string username,
			int time,
			int gems,
			int kills,
			int deathType,
			int shotsHit,
			int shotsFired,
			int enemiesAlive,
			int homing,
			int levelUpTime2,
			int levelUpTime3,
			int levelUpTime4,
			string ddclClientVersion,
			string v)
		{
			try
			{
				UploadResult result = TryUpload(spawnsetHash, playerId, username, time, gems, kills, deathType, shotsHit, shotsFired, enemiesAlive, homing, levelUpTime2, levelUpTime3, levelUpTime4, ddclClientVersion, v);
				JsonResult = JsonConvert.SerializeObject(result);
			}
			catch (Exception ex)
			{
				JsonResult = JsonConvert.SerializeObject(new UploadResult(false, $"The server returned an error trying to upload score.\n\nDetails:\n\n{ex}", 10));
			}
		}

		private UploadResult TryUpload(
			string spawnsetHash,
			int playerId,
			string username,
			int time,
			int gems,
			int kills,
			int deathType,
			int shotsHit,
			int shotsFired,
			int enemiesAlive,
			int homing,
			int levelUpTime2,
			int levelUpTime3,
			int levelUpTime4,
			string clientVersion,
			string validation)
		{
			Version clientVersionParsed = Version.Parse(clientVersion);
			if (clientVersionParsed < ToolList.DevilDaggersCustomLeaderboards.VersionNumberRequired)
				return new UploadResult(false, "You are using an unsupported and outdated version of DDCL. Please update the program.");

			string spawnsetName = string.Empty;
			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(commonObjects.Env.WebRootPath, "spawnsets")))
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
				playerId,
				username,
				time,
				gems,
				kills,
				deathType,
				shotsHit,
				shotsFired,
				enemiesAlive,
				homing,
				string.Join(",", new int[3] { levelUpTime2, levelUpTime3, levelUpTime4 }));
			if (DecryptValidation(validation) != check)
				return new UploadResult(false, "Invalid submission.");

			CustomLeaderboard leaderboard = context.CustomLeaderboards.Include(l => l.Category).FirstOrDefault(l => l.SpawnsetFileName == spawnsetName); // TODO: Support multiple leaderboards? Would require significant changes as you would need to pick one, or let DDCL upload to all leaderboards of this spawnset.
			if (leaderboard == null)
				return new UploadResult(false, "This spawnset doesn't have a leaderboard.");

			// Submission is accepted.

			// Fix any broken values.
			homing = Math.Max(0, homing);

			// Update the date this leaderboard was submitted to.
			leaderboard.DateLastPlayed = DateTime.Now;

			// Calculate the new rank.
			CustomEntry[] entries = context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();
			int rank = leaderboard.Category.Ascending ? entries.Where(e => e.Time < time).Count() + 1 : entries.Where(e => e.Time > time).Count() + 1; // TODO: Use reflection to use Category.SortingPropertyName.
			int totalPlayers = entries.Length;

			CustomEntry entry = context.CustomEntries.FirstOrDefault(e => e.PlayerId == playerId && e.CustomLeaderboardId == leaderboard.Id);
			if (entry == null)
			{
				// Add new user to this leaderboard.
				context.CustomEntries.Add(new CustomEntry(playerId, username, time, gems, kills, deathType, shotsHit, shotsFired, enemiesAlive, homing, levelUpTime2, levelUpTime3, levelUpTime4, DateTime.Now, clientVersion) { CustomLeaderboard = leaderboard });
				context.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();
				totalPlayers = entries.Length;

				return new UploadResult(true, $"Welcome to the leaderboard for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}.", 0, new SubmissionInfo
				{
					TotalPlayers = totalPlayers,
					Leaderboard = leaderboard,
					Category = leaderboard.Category,
					Entries = entries,
					IsNewUserOnThisLeaderboard = true,
					Rank = rank,
					Time = time,
					Kills = kills,
					Gems = gems,
					ShotsHit = shotsHit,
					ShotsFired = shotsFired,
					EnemiesAlive = enemiesAlive,
					Homing = homing,
					LevelUpTime2 = levelUpTime2,
					LevelUpTime3 = levelUpTime3,
					LevelUpTime4 = levelUpTime4
				});
			}

			// Update the username.
			foreach (CustomEntry en in context.CustomEntries.Where(e => e.PlayerId == entry.PlayerId))
				en.Username = username;

			// User is already on the leaderboard, but did not get a better score.
			if (leaderboard.Category.Ascending && entry.Time <= time // TODO: Use reflection to use Category.SortingPropertyName.
			 || !leaderboard.Category.Ascending && entry.Time >= time)
			{
				context.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();

				return new UploadResult(true, $"No new highscore for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}.", 0, new SubmissionInfo
				{
					TotalPlayers = totalPlayers,
					Leaderboard = leaderboard,
					Category = leaderboard.Category,
					Entries = entries,
					IsNewUserOnThisLeaderboard = false
				});
			}

			// User got a better score.

			// Calculate the old rank.
			int oldRank = leaderboard.Category.Ascending ? entries.Where(e => e.Time < entry.Time).Count() + 1 : entries.Where(e => e.Time > entry.Time).Count() + 1;

			int rankDiff = oldRank - rank;
			int timeDiff = time - entry.Time;
			int killsDiff = kills - entry.Kills;
			int gemsDiff = gems - entry.Gems;
			int shotsHitDiff = shotsHit - entry.ShotsHit;
			int shotsFiredDiff = shotsFired - entry.ShotsFired;
			int enemiesAliveDiff = enemiesAlive - entry.EnemiesAlive;
			int homingDiff = homing - entry.Homing;
			int levelUpTime2Diff = levelUpTime2 - entry.LevelUpTime2;
			int levelUpTime3Diff = levelUpTime3 - entry.LevelUpTime3;
			int levelUpTime4Diff = levelUpTime4 - entry.LevelUpTime4;

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

			context.SaveChanges();

			// Fetch the entries again after having modified the leaderboard.
			entries = context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();

			return new UploadResult(true, $"NEW HIGHSCORE for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}!", 0, new SubmissionInfo
			{
				TotalPlayers = totalPlayers,
				Leaderboard = leaderboard,
				Category = leaderboard.Category,
				Entries = entries,
				IsNewUserOnThisLeaderboard = false,
				Rank = rank,
				RankDiff = rankDiff,
				Time = time,
				TimeDiff = timeDiff,
				Kills = kills,
				KillsDiff = killsDiff,
				Gems = gems,
				GemsDiff = gemsDiff,
				ShotsHit = shotsHit,
				ShotsHitDiff = shotsHitDiff,
				ShotsFired = shotsFired,
				ShotsFiredDiff = shotsFiredDiff,
				EnemiesAlive = enemiesAlive,
				EnemiesAliveDiff = enemiesAliveDiff,
				Homing = homing,
				HomingDiff = homingDiff,
				LevelUpTime2 = levelUpTime2,
				LevelUpTime2Diff = levelUpTime2Diff,
				LevelUpTime3 = levelUpTime3,
				LevelUpTime3Diff = levelUpTime3Diff,
				LevelUpTime4 = levelUpTime4,
				LevelUpTime4Diff = levelUpTime4Diff
			});
		}

		private string DecryptValidation(string validation)
		{
			try
			{
				return Secrets.EncryptionWrapper.DecodeAndDecrypt(HttpUtility.HtmlDecode(validation));
			}
			catch (Exception ex)
			{
				throw new Exception($"Could not decrypt '{validation}'.", ex);
			}
		}
	}
}