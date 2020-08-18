﻿using DevilDaggersCore.Extensions;
using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.External;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dto = DevilDaggersWebsite.Code.DataTransferObjects;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/custom-leaderboards")]
	[ApiController]
	public class CustomLeaderboardsController : ControllerBase
	{
		private readonly ApplicationDbContext dbContext;
		private readonly IWebHostEnvironment env;

		public CustomLeaderboardsController(ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			this.dbContext = dbContext;
			this.env = env;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Dto.CustomLeaderboard>> GetCustomLeaderboards()
			=> dbContext.CustomLeaderboards.Select(cl => new Dto.CustomLeaderboard
			{
				SpawnsetFileName = cl.SpawnsetFileName,
				Bronze = cl.Bronze,
				Silver = cl.Silver,
				Golden = cl.Golden,
				Devil = cl.Devil,
				Homing = cl.Homing,
				DateLastPlayed = cl.DateLastPlayed,
				DateCreated = cl.DateCreated,
			}).ToList();

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Dto.UploadSuccess>> UploadScore([FromBody] Dto.UploadRequest uploadRequest)
		{
			try
			{
				return await TryUpload(uploadRequest);
			}
			catch (Exception ex)
			{
				return new ObjectResult($"The server returned an error trying to upload score.\n\nDetails:\n\n{ex}") { StatusCode = StatusCodes.Status500InternalServerError };
			}
		}

		private async Task<ActionResult<Dto.UploadSuccess>> TryUpload(Dto.UploadRequest uploadRequest)
		{
			Version clientVersionParsed = Version.Parse(uploadRequest.DdclClientVersion);
			if (clientVersionParsed < ToolList.DevilDaggersCustomLeaderboards.VersionNumberRequired)
				return new BadRequestObjectResult("You are using an unsupported and outdated version of DDCL. Please update the program.");

			string spawnsetName = string.Empty;
			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "spawnsets")))
			{
				string hash = string.Empty;

				using (FileStream fs = new FileStream(spawnsetPath, FileMode.Open, FileAccess.Read))
				{
					if (Spawnset.TryParse(fs, out Spawnset spawnsetObject))
						hash = spawnsetObject.GetHashString();
				}

				if (hash == uploadRequest.SpawnsetHash)
				{
					spawnsetName = Path.GetFileName(spawnsetPath);
					break;
				}
			}

			if (string.IsNullOrEmpty(spawnsetName))
				return new BadRequestObjectResult("This spawnset does not exist on DevilDaggers.info.");

			string check = string.Join(
				";",
				uploadRequest.PlayerId,
				uploadRequest.Time,
				uploadRequest.Gems,
				uploadRequest.Kills,
				uploadRequest.DeathType,
				uploadRequest.DaggersHit,
				uploadRequest.DaggersFired,
				uploadRequest.EnemiesAlive,
				uploadRequest.Homing,
				string.Join(",", new int[3] { uploadRequest.LevelUpTime2, uploadRequest.LevelUpTime3, uploadRequest.LevelUpTime4 }));
			if (DecryptValidation(uploadRequest.Validation) != check)
				return new UnauthorizedObjectResult("Invalid submission.");

			CustomLeaderboard leaderboard = dbContext.CustomLeaderboards.Include(l => l.Category).FirstOrDefault(l => l.SpawnsetFileName == spawnsetName);
			if (leaderboard == null)
				return new BadRequestObjectResult("This spawnset doesn't have a leaderboard.");

			// Submission is accepted.

			// Fix any broken values.
			uploadRequest.Homing = Math.Max(0, uploadRequest.Homing);

			// Update the date this leaderboard was submitted to.
			leaderboard.DateLastPlayed = DateTime.Now;

			// Calculate the new rank.
			IEnumerable<CustomEntry> entries = dbContext.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();
			int rank = leaderboard.Category.Ascending ? entries.Where(e => e.Time < uploadRequest.Time).Count() + 1 : entries.Where(e => e.Time > uploadRequest.Time).Count() + 1; // TODO: Use reflection to use Category.SortingPropertyName.
			int totalPlayers = entries.Count();

			CustomEntry entry = dbContext.CustomEntries.FirstOrDefault(e => e.PlayerId == uploadRequest.PlayerId && e.CustomLeaderboardId == leaderboard.Id);
			if (entry == null)
			{
				// Add new user to this leaderboard.
				dbContext.CustomEntries.Add(new CustomEntry
				{
					PlayerId = uploadRequest.PlayerId,
					Time = uploadRequest.Time,
					Gems = uploadRequest.Gems,
					Kills = uploadRequest.Kills,
					DeathType = uploadRequest.DeathType,
					DaggersHit = uploadRequest.DaggersHit,
					DaggersFired = uploadRequest.DaggersFired,
					EnemiesAlive = uploadRequest.EnemiesAlive,
					Homing = uploadRequest.Homing,
					LevelUpTime2 = uploadRequest.LevelUpTime2,
					LevelUpTime3 = uploadRequest.LevelUpTime3,
					LevelUpTime4 = uploadRequest.LevelUpTime4,
					SubmitDate = DateTime.Now,
					ClientVersion = uploadRequest.DdclClientVersion,
					CustomLeaderboard = leaderboard,
				});
				dbContext.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = dbContext.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending);
				totalPlayers = entries.Count();

				return new Dto.UploadSuccess
				{
					Message = $"Welcome to the leaderboard for {Dto.SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}.",
					TotalPlayers = totalPlayers,
					Leaderboard = leaderboard,
					Category = leaderboard.Category,
					Entries = entries
						.Select(e => new Dto.CustomEntry
						{
							PlayerId = e.PlayerId,
							ClientVersion = e.ClientVersion,
							DeathType = e.DeathType,
							EnemiesAlive = e.EnemiesAlive,
							Gems = e.Gems,
							Homing = e.Homing,
							Kills = e.Kills,
							LevelUpTime2 = e.LevelUpTime2,
							LevelUpTime3 = e.LevelUpTime3,
							LevelUpTime4 = e.LevelUpTime4,
							DaggersFired = e.DaggersFired,
							DaggersHit = e.DaggersHit,
							SubmitDate = e.SubmitDate,
							Time = e.Time,
						})
						.ToList(),
					IsNewUserOnThisLeaderboard = true,
					Rank = rank,
					Time = uploadRequest.Time,
					Kills = uploadRequest.Kills,
					Gems = uploadRequest.Gems,
					DaggersHit = uploadRequest.DaggersHit,
					DaggersFired = uploadRequest.DaggersFired,
					EnemiesAlive = uploadRequest.EnemiesAlive,
					Homing = uploadRequest.Homing,
					LevelUpTime2 = uploadRequest.LevelUpTime2,
					LevelUpTime3 = uploadRequest.LevelUpTime3,
					LevelUpTime4 = uploadRequest.LevelUpTime4,
				};
			}

			// Add the player or update the username.
			Player player = dbContext.Players.FirstOrDefault(p => p.Id == entry.PlayerId);
			if (player == null)
			{
				player = new Player
				{
					Id = entry.PlayerId,
					Username = await GetUsername(uploadRequest),
				};
				dbContext.Players.Add(player);
			}
			else
			{
				player.Username = await GetUsername(uploadRequest);
			}

			// User is already on the leaderboard, but did not get a better score.
			// TODO: Use reflection to use Category.SortingPropertyName.
			if (leaderboard.Category.Ascending && entry.Time <= uploadRequest.Time || !leaderboard.Category.Ascending && entry.Time >= uploadRequest.Time)
			{
				dbContext.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = dbContext.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();

				return new Dto.UploadSuccess
				{
					Message = $"No new highscore for {Dto.SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}.",
					TotalPlayers = totalPlayers,
					Leaderboard = leaderboard,
					Category = leaderboard.Category,
					Entries = entries
						.Select(e => new Dto.CustomEntry
						{
							PlayerId = e.PlayerId,
							ClientVersion = e.ClientVersion,
							DeathType = e.DeathType,
							EnemiesAlive = e.EnemiesAlive,
							Gems = e.Gems,
							Homing = e.Homing,
							Kills = e.Kills,
							LevelUpTime2 = e.LevelUpTime2,
							LevelUpTime3 = e.LevelUpTime3,
							LevelUpTime4 = e.LevelUpTime4,
							DaggersFired = e.DaggersFired,
							DaggersHit = e.DaggersHit,
							SubmitDate = e.SubmitDate,
							Time = e.Time,
						})
						.ToList(),
					IsNewUserOnThisLeaderboard = false,
				};
			}

			// User got a better score.

			// Calculate the old rank.
			int oldRank = leaderboard.Category.Ascending ? entries.Where(e => e.Time < entry.Time).Count() + 1 : entries.Where(e => e.Time > entry.Time).Count() + 1;

			int rankDiff = oldRank - rank;
			int timeDiff = uploadRequest.Time - entry.Time;
			int killsDiff = uploadRequest.Kills - entry.Kills;
			int gemsDiff = uploadRequest.Gems - entry.Gems;
			int shotsHitDiff = uploadRequest.DaggersHit - entry.DaggersHit;
			int shotsFiredDiff = uploadRequest.DaggersFired - entry.DaggersFired;
			int enemiesAliveDiff = uploadRequest.EnemiesAlive - entry.EnemiesAlive;
			int homingDiff = uploadRequest.Homing - entry.Homing;
			int levelUpTime2Diff = uploadRequest.LevelUpTime2 - entry.LevelUpTime2;
			int levelUpTime3Diff = uploadRequest.LevelUpTime3 - entry.LevelUpTime3;
			int levelUpTime4Diff = uploadRequest.LevelUpTime4 - entry.LevelUpTime4;

			entry.Time = uploadRequest.Time;
			entry.Kills = uploadRequest.Kills;
			entry.Gems = uploadRequest.Gems;
			entry.DeathType = uploadRequest.DeathType;
			entry.DaggersHit = uploadRequest.DaggersHit;
			entry.DaggersFired = uploadRequest.DaggersFired;
			entry.EnemiesAlive = uploadRequest.EnemiesAlive;
			entry.Homing = uploadRequest.Homing;
			entry.LevelUpTime2 = uploadRequest.LevelUpTime2;
			entry.LevelUpTime3 = uploadRequest.LevelUpTime3;
			entry.LevelUpTime4 = uploadRequest.LevelUpTime4;
			entry.SubmitDate = DateTime.Now;
			entry.ClientVersion = uploadRequest.DdclClientVersion;

			dbContext.SaveChanges();

			// Fetch the entries again after having modified the leaderboard.
			entries = dbContext.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();

			return new Dto.UploadSuccess
			{
				Message = $"NEW HIGHSCORE for {Dto.SpawnsetFile.GetName(leaderboard.SpawnsetFileName)}!",
				TotalPlayers = totalPlayers,
				Leaderboard = leaderboard,
				Category = leaderboard.Category,
				Entries = entries
					.Select(e => new Dto.CustomEntry
					{
						PlayerId = e.PlayerId,
						ClientVersion = e.ClientVersion,
						DeathType = e.DeathType,
						EnemiesAlive = e.EnemiesAlive,
						Gems = e.Gems,
						Homing = e.Homing,
						Kills = e.Kills,
						LevelUpTime2 = e.LevelUpTime2,
						LevelUpTime3 = e.LevelUpTime3,
						LevelUpTime4 = e.LevelUpTime4,
						DaggersFired = e.DaggersFired,
						DaggersHit = e.DaggersHit,
						SubmitDate = e.SubmitDate,
						Time = e.Time,
					})
					.ToList(),
				IsNewUserOnThisLeaderboard = false,
				Rank = rank,
				RankDiff = rankDiff,
				Time = uploadRequest.Time,
				TimeDiff = timeDiff,
				Kills = uploadRequest.Kills,
				KillsDiff = killsDiff,
				Gems = uploadRequest.Gems,
				GemsDiff = gemsDiff,
				DaggersHit = uploadRequest.DaggersHit,
				DaggersHitDiff = shotsHitDiff,
				DaggersFired = uploadRequest.DaggersFired,
				DaggersFiredDiff = shotsFiredDiff,
				EnemiesAlive = uploadRequest.EnemiesAlive,
				EnemiesAliveDiff = enemiesAliveDiff,
				Homing = uploadRequest.Homing,
				HomingDiff = homingDiff,
				LevelUpTime2 = uploadRequest.LevelUpTime2,
				LevelUpTime2Diff = levelUpTime2Diff,
				LevelUpTime3 = uploadRequest.LevelUpTime3,
				LevelUpTime3Diff = levelUpTime3Diff,
				LevelUpTime4 = uploadRequest.LevelUpTime4,
				LevelUpTime4Diff = levelUpTime4Diff,
			};
		}

		private static async Task<string> GetUsername(Dto.UploadRequest uploadRequest)
		{
			if (uploadRequest.Username.EndsWith("med fragger", StringComparison.InvariantCulture))
				return (await HasmodaiUtils.GetUserById(uploadRequest.PlayerId)).Username;
			return uploadRequest.Username;
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