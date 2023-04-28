using DevilDaggersInfo.Api.Main.Leaderboards;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/leaderboards")]
[ApiController]
public class LeaderboardsController : ControllerBase
{
	private readonly IDdLeaderboardService _leaderboardClient;

	public LeaderboardsController(IDdLeaderboardService leaderboardClient)
	{
		_leaderboardClient = leaderboardClient;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetLeaderboard?>> GetLeaderboard([Range(1, int.MaxValue)] int rankStart = 1)
	{
		return (await _leaderboardClient.GetLeaderboard(rankStart)).ToMainApi();
	}

	// FORBIDDEN: Used by DDLIVE.
	[HttpGet("entry/by-id")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetEntry>> GetEntryById([Required, Range(1, int.MaxValue)] int id)
	{
		return (await _leaderboardClient.GetEntryById(id)).ToMainApi();
	}

	[HttpGet("entry/by-ids")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetEntriesByIds(string commaSeparatedIds)
	{
		IEnumerable<int> ids = commaSeparatedIds.Split(',').Where(s => int.TryParse(s, out _)).Select(int.Parse);

		return (await _leaderboardClient.GetEntriesByIds(ids)).ConvertAll(e => e.ToMainApi());
	}

	[HttpGet("entry/by-username")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetEntriesByName([Required, MinLength(3), MaxLength(16)] string name)
	{
		return (await _leaderboardClient.GetEntriesByName(name)).ConvertAll(e => e.ToMainApi());
	}

	[HttpGet("entry/by-rank")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetEntry>> GetEntryByRank([Required, Range(1, int.MaxValue)] int rank)
	{
		IDdLeaderboardService.LeaderboardResponse leaderboard = await _leaderboardClient.GetLeaderboard(rank);

		if (leaderboard.Entries.Count == 0)
			return NotFound();

		return leaderboard.Entries[0].ToMainApi();
	}
}
