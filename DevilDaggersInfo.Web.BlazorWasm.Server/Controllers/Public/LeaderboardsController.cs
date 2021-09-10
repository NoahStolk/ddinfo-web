using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
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
		LeaderboardResponse l = await LeaderboardClient.Instance.GetLeaderboard(rankStart);
		return l.ToGetLeaderboardPublic();
	}

	[HttpGet("entry/by-id")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetEntry>> GetEntryById([Required, Range(1, int.MaxValue)] int id)
	{
		EntryResponse e = await LeaderboardClient.Instance.GetEntryById(id);
		return e.ToGetEntryPublic();
	}

	[HttpGet("entry/by-ids")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetEntriesByIds(string commaSeparatedIds)
	{
		IEnumerable<int> ids = commaSeparatedIds.Split(',').Where(s => int.TryParse(s, out _)).Select(int.Parse);

		List<EntryResponse> el = await LeaderboardClient.Instance.GetEntriesByIds(ids);
		return el.ConvertAll(e => e.ToGetEntryPublic());
	}

	[HttpGet("entry/by-username")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetEntriesByName([Required, MinLength(3)] string name)
	{
		List<EntryResponse> el = await LeaderboardClient.Instance.GetEntriesByName(name);
		return el.ConvertAll(e => e.ToGetEntryPublic());
	}

	[HttpGet("entry/by-rank")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetEntry>> GetEntryByRank([Required, Range(1, int.MaxValue)] int rank)
	{
		// TODO: Implement separate method that only parses the first entry.
		LeaderboardResponse l = await LeaderboardClient.Instance.GetLeaderboard(rank);
		if (l.Entries.Count == 0)
			return NotFound();

		return l.Entries[0].ToGetEntryPublic();
	}
}
