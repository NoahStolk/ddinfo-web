using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;
using DevilDaggersInfo.Web.Server.Converters.Public;

namespace DevilDaggersInfo.Web.Server.Controllers.Public;

[Route("api/leaderboards")]
[ApiController]
public class LeaderboardsController : ControllerBase
{
	private readonly LeaderboardClient _leaderboardClient;

	public LeaderboardsController(LeaderboardClient leaderboardClient)
	{
		_leaderboardClient = leaderboardClient;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetLeaderboard?>> GetLeaderboard([Range(1, int.MaxValue)] int rankStart = 1)
	{
		LeaderboardResponse? leaderboardResponse = await _leaderboardClient.GetLeaderboard(rankStart);
		if (leaderboardResponse == null)
			return BadRequest("The requested data could not be retrieved from the leaderboard servers.");

		return leaderboardResponse.ToGetLeaderboardPublic();
	}

	[HttpGet("entry/by-id")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetEntry>> GetEntryById([Required, Range(1, int.MaxValue)] int id)
	{
		EntryResponse? entryResponse = await _leaderboardClient.GetEntryById(id);
		if (entryResponse == null)
			return BadRequest();

		return entryResponse.ToGetEntryPublic();
	}

	[HttpGet("entry/by-ids")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetEntriesByIds(string commaSeparatedIds)
	{
		IEnumerable<int> ids = commaSeparatedIds.Split(',').Where(s => int.TryParse(s, out _)).Select(int.Parse);

		List<EntryResponse>? entriesResponse = await _leaderboardClient.GetEntriesByIds(ids);
		if (entriesResponse == null)
			return BadRequest("The requested data could not be retrieved from the leaderboard servers.");

		return entriesResponse.ConvertAll(e => e.ToGetEntryPublic());
	}

	[HttpGet("entry/by-username")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetEntriesByName([Required, MinLength(3), MaxLength(16)] string name)
	{
		List<EntryResponse>? entriesResponse = await _leaderboardClient.GetEntriesByName(name);
		if (entriesResponse == null)
			return BadRequest("The requested data could not be retrieved from the leaderboard servers.");

		return entriesResponse.ConvertAll(e => e.ToGetEntryPublic());
	}

	[HttpGet("entry/by-rank")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetEntry>> GetEntryByRank([Required, Range(1, int.MaxValue)] int rank)
	{
		// TODO: Implement separate method that only parses the first entry.
		LeaderboardResponse? leaderboardResponse = await _leaderboardClient.GetLeaderboard(rank);
		if (leaderboardResponse == null)
			return BadRequest("The requested data could not be retrieved from the leaderboard servers.");

		if (leaderboardResponse.Entries.Count == 0)
			return NotFound();

		return leaderboardResponse.Entries[0].ToGetEntryPublic();
	}
}
