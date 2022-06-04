using DevilDaggersInfo.Api.Clubber;

namespace DevilDaggersInfo.Web.Server.Controllers.Clubber;

[Route("api/clubber/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public PlayersController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[Obsolete("Use the new route instead.")]
	[HttpGet("/api/players/{id}/flag")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<string> GetPlayerFlagById([Required] int id)
	{
		var player = _dbContext.Players.AsNoTracking().Select(p => new { p.Id, p.CountryCode }).FirstOrDefault(p => p.Id == id);
		return player?.CountryCode ?? string.Empty;
	}

	[Obsolete("Use the new route instead.")]
	[HttpGet("{id}/country-code")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<GetPlayerCountryCode> GetPlayerCountryCodeById([Required] int id)
	{
		var player = _dbContext.Players.AsNoTracking().Select(p => new { p.Id, p.CountryCode }).FirstOrDefault(p => p.Id == id);
		return new GetPlayerCountryCode
		{
			CountryCode = player?.CountryCode,
		};
	}
}
