using DevilDaggersInfo.Api.Main.Leaderboards;
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
		ResponseWrapper<LeaderboardResponse> wrapper = await _leaderboardClient.GetLeaderboard(rankStart);
		if (wrapper.HasError)
			return BadRequest(wrapper.ErrorMessage);

		return wrapper.GetResponse().ToGetLeaderboardPublic();
	}

	// FORBIDDEN: Used by DDLIVE.
	[HttpGet("entry/by-id")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetEntry>> GetEntryById([Required, Range(1, int.MaxValue)] int id)
	{
		ResponseWrapper<EntryResponse> wrapper = await _leaderboardClient.GetEntryById(id);
		if (wrapper.HasError)
			return BadRequest(wrapper.ErrorMessage);

		return wrapper.GetResponse().ToGetEntryPublic();
	}

	[HttpGet("entry/by-ids")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetEntriesByIds(string commaSeparatedIds)
	{
		IEnumerable<int> ids = commaSeparatedIds.Split(',').Where(s => int.TryParse(s, out _)).Select(int.Parse);

		ResponseWrapper<List<EntryResponse>> wrapper = await _leaderboardClient.GetEntriesByIds(ids);
		if (wrapper.HasError)
			return BadRequest(wrapper.ErrorMessage);

		return wrapper.GetResponse().ConvertAll(e => e.ToGetEntryPublic());
	}

	[HttpGet("entry/by-username")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetEntry>>> GetEntriesByName([Required, MinLength(3), MaxLength(16)] string name)
	{
		ResponseWrapper<List<EntryResponse>> wrapper = await _leaderboardClient.GetEntriesByName(name);
		if (wrapper.HasError)
			return BadRequest(wrapper.ErrorMessage);

		return wrapper.GetResponse().ConvertAll(e => e.ToGetEntryPublic());
	}

	[HttpGet("entry/by-rank")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetEntry>> GetEntryByRank([Required, Range(1, int.MaxValue)] int rank)
	{
		ResponseWrapper<LeaderboardResponse> wrapper = await _leaderboardClient.GetLeaderboard(rank);
		if (wrapper.HasError)
			return BadRequest(wrapper.ErrorMessage);

		LeaderboardResponse leaderboard = wrapper.GetResponse();
		if (leaderboard.Entries.Count == 0)
			return NotFound();

		return leaderboard.Entries[0].ToGetEntryPublic();
	}
}
