﻿using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/leaderboards")]
[ApiController]
public class LeaderboardsController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<GetLeaderboard?>> GetLeaderboard([Range(1, int.MaxValue)] int rankStart = 1)
	{
		LeaderboardResponse l = await LeaderboardClient.Instance.GetScores(rankStart);
		return l.ToGetLeaderboardPublic();
	}

	[HttpGet("player/by-id")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetEntry>> GetPlayerById([Required, Range(1, int.MaxValue)] int userId)
	{
		EntryResponse e = await LeaderboardClient.Instance.GetUserById(userId);
		return e.ToGetEntryPublic();
	}

	[HttpGet("player/by-ids")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetPlayersByIds(string commaSeparatedUserIds)
	{
		IEnumerable<int> userIds = commaSeparatedUserIds.Split(',').Where(s => int.TryParse(s, out _)).Select(int.Parse);

		List<EntryResponse> el = await LeaderboardClient.Instance.GetUsersByIds(userIds);
		return el.ConvertAll(e => e.ToGetEntryPublic());
	}

	[HttpGet("player/by-username")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetPlayersByName([Required, MinLength(3)] string username)
	{
		List<EntryResponse> el = await LeaderboardClient.Instance.GetUserSearch(username);
		return el.ConvertAll(e => e.ToGetEntryPublic());
	}

	[HttpGet("player/by-rank")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetEntry>> GetPlayerByRank([Required, Range(1, int.MaxValue)] int rank)
	{
		LeaderboardResponse l = await LeaderboardClient.Instance.GetScores(rank);
		if (l.Entries.Count == 0)
			return NotFound();

		return l.Entries[0].ToGetEntryPublic();
	}
}
