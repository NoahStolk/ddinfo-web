using CoreBase.Services;
using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersCore.Spawnsets;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using DevilDaggersWebsite.Code.Utils;
using EncryptionUtils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
		private const int textWidth = 25;

		public string JsonResult { get; set; }

		private readonly ApplicationDbContext context;
		private readonly ICommonObjects commonObjects;

		public UploadModel(ApplicationDbContext context, ICommonObjects commonObjects)
		{
			this.context = context;
			this.commonObjects = commonObjects;
		}

		public void OnGet(string spawnsetHash, int playerId, string username, float time, int gems, int kills, int deathType, int shotsHit, int shotsFired, int enemiesAlive, int homing, float levelUpTime2, float levelUpTime3, float levelUpTime4, string ddclClientVersion, string v)
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

		private UploadResult TryUpload(string spawnsetHash, int playerId, string username, float time, int gems, int kills, int deathType, int shotsHit, int shotsFired, int enemiesAlive, int homing, float levelUpTime2, float levelUpTime3, float levelUpTime4, string clientVersion, string validation)
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
				string.Join(",", new float[3] { levelUpTime2, levelUpTime3, levelUpTime4 }));
			if (DecryptValidation(validation) != check)
				return new UploadResult(false, "Invalid submission.");

			CustomLeaderboard leaderboard = context.CustomLeaderboards.Include(l => l.Category).FirstOrDefault(l => l.SpawnsetFileName == spawnsetName); // TODO: Multiple leaderboards
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
			List<CustomEntry> entries = context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToList();
			int rank = leaderboard.Category.Ascending ? entries.Where(e => e.Time < time).Count() + 1 : entries.Where(e => e.Time > time).Count() + 1; // TODO: Use reflection to use Category.SortingPropertyName.
			int totalPlayers = entries.Count();

			CustomEntry entry = context.CustomEntries.FirstOrDefault(e => e.PlayerId == playerId && e.CustomLeaderboardId == leaderboard.Id);
			if (entry == null)
			{
				// Add new user to this leaderboard.
				context.CustomEntries.Add(new CustomEntry(playerId, username, time, gems, kills, deathType, shotsHit, shotsFired, enemiesAlive, homing, levelUpTime2, levelUpTime3, levelUpTime4, DateTime.Now, clientVersion) { CustomLeaderboard = leaderboard });

				context.SaveChanges();

				if (clientVersionParsed <= new Version(0, 4, 4, 0))
				{
					return new UploadResult(true, $@"Welcome to the leaderboard for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}.

{$"Rank",-textWidth}{rank} / {++totalPlayers}
{$"Time",-textWidth}{time:0.0000}");
				}

				return new UploadResult(true, $"Welcome to the leaderboard for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}.", 0, new SubmissionInfo
				{
					TotalPlayers = totalPlayers,
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
			else
			{
				// Update the username.
				foreach (CustomEntry en in context.CustomEntries.Where(e => e.PlayerId == entry.PlayerId))
					en.Username = username;

				// User is already on the leaderboard, check for better score.
				if (leaderboard.Category.Ascending && entry.Time <= time // TODO: Use reflection to use Category.SortingPropertyName.
				 || !leaderboard.Category.Ascending && entry.Time >= time)
				{
					context.SaveChanges();
					return new UploadResult(true, $"No new highscore for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}.");
				}

				// Calculate the old rank.
				int oldRank = leaderboard.Category.Ascending ? entries.Where(e => e.Time < time).Count() + 1 : entries.Where(e => e.Time > time).Count() + 1;

				int rankDiff = oldRank - rank;
				float timeDiff = time - entry.Time;
				int killsDiff = kills - entry.Kills;
				int gemsDiff = gems - entry.Gems;
				int shotsHitDiff = shotsHit - entry.ShotsHit;
				int shotsFiredDiff = shotsFired - entry.ShotsFired;
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

				context.SaveChanges();

				if (clientVersionParsed <= new Version(0, 4, 4, 0))
				{
					double accuracy = shotsFired == 0 ? 0 : shotsHit / (double)shotsFired;
					double accuracyDiff = accuracy - (entry.ShotsFired == 0 ? 0 : entry.ShotsHit / (double)entry.ShotsFired);

					return new UploadResult(true, $@"NEW HIGHSCORE for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}!
                
{$"Rank",-textWidth}{$"{rank} / {totalPlayers}",textWidth} ({rankDiff:+0;-#})
{$"Time",-textWidth}{time,textWidth:0.0000} ({(timeDiff < 0 ? "" : "+")}{timeDiff:0.0000})
{$"Kills",-textWidth}{kills,textWidth} ({killsDiff:+0;-#})
{$"Gems",-textWidth}{gems,textWidth} ({gemsDiff:+0;-#})
{$"Accuracy",-textWidth}{accuracy,textWidth:0.00%} ({(accuracyDiff < 0 ? "" : "+")}{accuracyDiff:0.00%})
{$"Enemies Alive",-textWidth}{enemiesAlive,textWidth} ({enemiesAliveDiff:+0;-#})
{$"Homing",-textWidth}{homing,textWidth} ({homingDiff:+0;-#})
{$"Level 2",-textWidth}{levelUpTime2,textWidth:0.0000} ({(levelUpTime2Diff < 0 ? "" : "+")}{levelUpTime2Diff:0.0000})
{$"Level 3",-textWidth}{levelUpTime3,textWidth:0.0000} ({(levelUpTime3Diff < 0 ? "" : "+")}{levelUpTime3Diff:0.0000})
{$"Level 4",-textWidth}{levelUpTime4,textWidth:0.0000} ({(levelUpTime4Diff < 0 ? "" : "+")}{levelUpTime4Diff:0.0000})");
				}

				return new UploadResult(true, $"NEW HIGHSCORE for {SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}!", 0, new SubmissionInfo
				{
					TotalPlayers = totalPlayers,
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
				throw new Exception($"Could not decrypt '{validation}'.", ex);
			}
		}
	}
}