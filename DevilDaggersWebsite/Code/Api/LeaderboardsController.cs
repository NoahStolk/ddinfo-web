using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.External;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class LeaderboardsController : ControllerBase
	{
		[HttpGet("leaderboard")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<Leaderboard>> GetLeaderboard(int rankStart)
		{
			if (rankStart <= 0)
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Incorrect parameter {nameof(rankStart)} '{rankStart}' specified." });
			return await HasmodaiUtils.GetScores(rankStart);
		}

		[HttpGet("user/by-id")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<Entry>> GetUserById(int userId)
		{
			try
			{
				Dictionary<string, string> postValues = new Dictionary<string, string>
				{
					{ "uid", userId.ToString() }
				};

				FormUrlEncodedContent content = new FormUrlEncodedContent(postValues);
				HttpClient client = new HttpClient();
				HttpResponseMessage resp = await client.PostAsync(HasmodaiUtils.GetUserByIdUrl, content);
				byte[] data = await resp.Content.ReadAsByteArrayAsync();

				int bytePos = 19;

				Entry entry = new Entry
				{
					Username = HasmodaiUtils.GetUsername(data, ref bytePos),
					Rank = BitConverter.ToInt32(data, bytePos),
					Id = BitConverter.ToInt32(data, bytePos + 4),
					Time = BitConverter.ToInt32(data, bytePos + 12),
					Kills = BitConverter.ToInt32(data, bytePos + 16),
					Gems = BitConverter.ToInt32(data, bytePos + 28),
					ShotsHit = BitConverter.ToInt32(data, bytePos + 24),
					ShotsFired = BitConverter.ToInt32(data, bytePos + 20),
					DeathType = BitConverter.ToInt16(data, bytePos + 32),
					TimeTotal = BitConverter.ToUInt64(data, bytePos + 60),
					KillsTotal = BitConverter.ToUInt64(data, bytePos + 44),
					GemsTotal = BitConverter.ToUInt64(data, bytePos + 68),
					DeathsTotal = BitConverter.ToUInt64(data, bytePos + 36),
					ShotsHitTotal = BitConverter.ToUInt64(data, bytePos + 76),
					ShotsFiredTotal = BitConverter.ToUInt64(data, bytePos + 52)
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
		public async Task<ActionResult<List<Entry>>> GetUserByUsername(string username)
			=> (await HasmodaiUtils.GetUserSearch(username)).Entries;

		[HttpGet("user/by-rank")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<Entry>> GetUserByRank(int rank)
		{
			List<Entry> entries = (await HasmodaiUtils.GetScores(rank)).Entries;
			if (entries.Count == 0)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Entry with {nameof(rank)} '{rank}' was not found." });
			return entries[0];
		}
	}
}