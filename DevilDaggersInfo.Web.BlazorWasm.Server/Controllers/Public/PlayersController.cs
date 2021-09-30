using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public PlayersController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet("leaderboard")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetPlayerForLeaderboard>> GetPlayersForLeaderboard()
	{
		return _dbContext.Players
			.AsNoTracking()

			// TODO: Check if this can be combined without querying entire entity.
			.Select(p => new { p.Id, p.BanDescription, p.IsBanned, p.CountryCode })
			.Select(p => new GetPlayerForLeaderboard
			{
				Id = p.Id,
				BanDescription = p.BanDescription,
				IsBanned = p.IsBanned,
				CountryCode = p.CountryCode,
			})
			.ToList();
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetPlayer> GetPlayerById([Required] int id)
	{
		PlayerEntity? player = _dbContext.Players
			.AsNoTracking()
			.FirstOrDefault(p => p.Id == id);
		if (player == null)
			return NotFound();

		bool isPublicDonator = !player.HideDonations && _dbContext.Donations.Any(d => d.PlayerId == id && !d.IsRefunded && d.ConvertedEuroCentsReceived > 0);

		List<string> playerTitles = _dbContext.PlayerTitles
			.AsNoTracking()
			.Include(pt => pt.Title)
			.Select(pt => new { TitleName = pt.Title.Name, pt.PlayerId })
			.Where(pt => pt.PlayerId == player.Id)
			.Select(pt => pt.TitleName)
			.ToList();

		return player.ToGetPlayer(isPublicDonator, playerTitles);
	}

	[HttpGet("{id}/flag")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<string> GetPlayerFlagById([Required] int id)
	{
		var player = _dbContext.Players.AsNoTracking().Select(p => new { p.Id, p.CountryCode }).FirstOrDefault(p => p.Id == id);
		return player?.CountryCode ?? string.Empty;
	}

	[HttpGet("settings")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetPlayerForSettings>> GetPlayersForSettings()
	{
		List<PlayerEntity> players = _dbContext.Players
			.AsNoTracking()
			.Where(p => !p.IsBanned && !p.HideSettings)
			.ToList();

		// Note; cannot evaluate HasSettings() against database (IQueryable).
		return players.Where(p => p.HasSettings()).Select(p => p.ToGetPlayerForSettings()).ToList();
	}
}
