using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using System.Runtime.Intrinsics.Arm;

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
		var players = _dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.BanDescription, p.IsBanned, p.CountryCode, p.HideDonations })
			.ToList();

		var donations = _dbContext.Donations
			.AsNoTracking()
			.Select(d => new { d.PlayerId, d.IsRefunded, d.ConvertedEuroCentsReceived })
			.Where(d => !d.IsRefunded && d.ConvertedEuroCentsReceived > 0)
			.ToList();

		List<PlayerTitleEntity> playerTitles = _dbContext.PlayerTitles
			.AsNoTracking()
			.Include(pt => pt.Title)
			.ToList();

		return players.ConvertAll(p => new GetPlayerForLeaderboard
		{
			Id = p.Id,
			BanDescription = p.BanDescription,
			IsBanned = p.IsBanned,
			IsPublicDonator = !p.HideDonations && donations.Any(d => d.PlayerId == p.Id),
			Titles = playerTitles.Where(pt => pt.PlayerId == p.Id).Select(pt => pt.Title.Name).ToList(),
			CountryCode = p.CountryCode,
		});
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
			.Where(p => !p.IsBanned && !p.HideSettings && (p.Dpi.HasValue || p.InGameSens.HasValue || p.Fov.HasValue || p.IsRightHanded.HasValue || p.HasFlashHandEnabled.HasValue || p.Gamma.HasValue || p.UsesLegacyAudio.HasValue))
			.ToList();

		var donations = _dbContext.Donations
			.AsNoTracking()
			.Select(d => new { d.PlayerId, d.IsRefunded, d.ConvertedEuroCentsReceived })
			.Where(d => !d.IsRefunded && d.ConvertedEuroCentsReceived > 0)
			.ToList();

		List<PlayerTitleEntity> playerTitles = _dbContext.PlayerTitles
			.AsNoTracking()
			.Include(pt => pt.Title)
			.ToList();

		return players.ConvertAll(p => new GetPlayerForSettings
		{
			Id = p.Id,
			IsPublicDonator = !p.HideDonations && donations.Any(d => d.PlayerId == p.Id),
			Titles = playerTitles.Where(pt => pt.PlayerId == p.Id).Select(pt => pt.Title.Name).ToList(),
			CountryCode = p.CountryCode,
			Dpi = p.Dpi,
			Fov = p.Fov,
			Gamma = p.Gamma,
			HasFlashHandEnabled = p.HasFlashHandEnabled,
			InGameSens = p.InGameSens,
			IsRightHanded = p.IsRightHanded,
			UsesLegacyAudio = p.UsesLegacyAudio,
		});
	}
}
