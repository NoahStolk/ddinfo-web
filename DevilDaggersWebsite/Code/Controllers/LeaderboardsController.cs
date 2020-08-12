﻿using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.External;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/leaderboards")]
	[ApiController]
	public class LeaderboardsController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<Leaderboard>> GetLeaderboard([Required] int rankStart)
		{
			if (rankStart <= 0)
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Incorrect parameter {nameof(rankStart)} '{rankStart}' specified. Value should be at least 1." });
			return await HasmodaiUtils.GetScores(rankStart);
		}

		[HttpGet("user/by-id")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<Entry>> GetUserById([Required] int userId)
		{
			try
			{
				Dictionary<string, string> postValues = new Dictionary<string, string>
				{
					{ "uid", userId.ToString() }
				};

				FormUrlEncodedContent content = new FormUrlEncodedContent(postValues);
				HttpClient client = new HttpClient();
				HttpResponseMessage response = await client.PostAsync(HasmodaiUtils.GetUserByIdUrl, content);
				byte[] data = await response.Content.ReadAsByteArrayAsync();

				int bytePosition = 19;

				Entry entry = new Entry
				{
					Username = HasmodaiUtils.GetUsername(data, ref bytePosition),
					Rank = BitConverter.ToInt32(data, bytePosition),
					Id = BitConverter.ToInt32(data, bytePosition + 4),
					Time = BitConverter.ToInt32(data, bytePosition + 12),
					Kills = BitConverter.ToInt32(data, bytePosition + 16),
					Gems = BitConverter.ToInt32(data, bytePosition + 28),
					ShotsHit = BitConverter.ToInt32(data, bytePosition + 24),
					ShotsFired = BitConverter.ToInt32(data, bytePosition + 20),
					DeathType = BitConverter.ToInt16(data, bytePosition + 32),
					TimeTotal = BitConverter.ToUInt64(data, bytePosition + 60),
					KillsTotal = BitConverter.ToUInt64(data, bytePosition + 44),
					GemsTotal = BitConverter.ToUInt64(data, bytePosition + 68),
					DeathsTotal = BitConverter.ToUInt64(data, bytePosition + 36),
					ShotsHitTotal = BitConverter.ToUInt64(data, bytePosition + 76),
					ShotsFiredTotal = BitConverter.ToUInt64(data, bytePosition + 52)
				};

				return entry;
			}
			catch
			{
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Entry with {nameof(userId)} '{userId}' was not found." });
			}
		}

		[HttpGet("user/by-username")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<List<Entry>>> GetUserByUsername([Required] string username)
		{
			if (string.IsNullOrEmpty(username) || username.Length < 3)
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Incorrect parameter {nameof(username)} '{username}' specified. Value should be at least 3 characters in length." });

			return (await HasmodaiUtils.GetUserSearch(username)).Entries;
		}

		[HttpGet("user/by-rank")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<Entry>> GetUserByRank([Required] int rank)
		{
			List<Entry> entries = (await HasmodaiUtils.GetScores(rank)).Entries;
			if (entries.Count == 0)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Entry with {nameof(rank)} '{rank}' was not found." });
			return entries[0];
		}
	}
}