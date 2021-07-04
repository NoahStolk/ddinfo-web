using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Api
{
	[Route("api/leaderboards")]
	[ApiController]
	public class LeaderboardsController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult<Leaderboard?>> GetLeaderboard(int rankStart = 1)
		{
			if (rankStart <= 0)
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Incorrect parameter {nameof(rankStart)} '{rankStart}' specified. Value should be at least 1." });
			return await LeaderboardClient.Instance.GetScores(rankStart);
		}

		[HttpGet("user/by-id")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult<Entry>> GetUserById([Required] int userId)
		{
			Entry? entry = await LeaderboardClient.Instance.GetUserById(userId);
			return entry == null
				? new NotFoundObjectResult(new ProblemDetails { Title = $"Entry with {nameof(userId)} '{userId}' was not found." })
				: (ActionResult<Entry>)entry;
		}

		[HttpGet("user/by-username")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult<List<Entry>?>> GetUserByUsername([Required] string username)
		{
			if (string.IsNullOrEmpty(username) || username.Length < 3)
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Incorrect parameter {nameof(username)} '{username}' specified. Value should be at least 3 characters in length." });

			return await LeaderboardClient.Instance.GetUserSearch(username);
		}

		[HttpGet("user/by-rank")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult<Entry>> GetUserByRank([Required] int rank)
		{
			List<Entry> entries = (await LeaderboardClient.Instance.GetScores(rank))?.Entries ?? new();
			if (entries.Count == 0)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Entry with {nameof(rank)} '{rank}' was not found." });
			return entries[0];
		}
	}
}
