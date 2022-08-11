using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Players;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly PlayerService _playerService;

	public PlayersController(ApplicationDbContext dbContext, PlayerService playerService)
	{
		_dbContext = dbContext;
		_playerService = playerService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Players)]
	public ActionResult<Page<GetPlayerForOverview>> GetPlayers(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		PlayerSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<PlayerEntity> playersQuery = _dbContext.Players.AsNoTracking();

		playersQuery = sortBy switch
		{
			PlayerSorting.BanDescription => playersQuery.OrderBy(p => p.BanDescription, ascending).ThenBy(p => p.Id),
			PlayerSorting.BanResponsibleId => playersQuery.OrderBy(p => p.BanResponsibleId, ascending).ThenBy(p => p.Id),
			PlayerSorting.BanType => playersQuery.OrderBy(p => p.BanType, ascending).ThenBy(p => p.Id),
			PlayerSorting.CommonName => playersQuery.OrderBy(p => p.CommonName, ascending).ThenBy(p => p.Id),
			PlayerSorting.CountryCode => playersQuery.OrderBy(p => p.CountryCode, ascending).ThenBy(p => p.Id),
			PlayerSorting.DiscordUserId => playersQuery.OrderBy(p => p.DiscordUserId, ascending).ThenBy(p => p.Id),
			PlayerSorting.Dpi => playersQuery.OrderBy(p => p.Dpi, ascending).ThenBy(p => p.Id),
			PlayerSorting.Fov => playersQuery.OrderBy(p => p.Fov, ascending).ThenBy(p => p.Id),
			PlayerSorting.Gamma => playersQuery.OrderBy(p => p.Gamma, ascending).ThenBy(p => p.Id),
			PlayerSorting.HasFlashHandEnabled => playersQuery.OrderBy(p => p.HasFlashHandEnabled, ascending).ThenBy(p => p.Id),
			PlayerSorting.HideDonations => playersQuery.OrderBy(p => p.HideDonations, ascending).ThenBy(p => p.Id),
			PlayerSorting.HidePastUsernames => playersQuery.OrderBy(p => p.HidePastUsernames, ascending).ThenBy(p => p.Id),
			PlayerSorting.HideSettings => playersQuery.OrderBy(p => p.HideSettings, ascending).ThenBy(p => p.Id),
			PlayerSorting.InGameSens => playersQuery.OrderBy(p => p.InGameSens, ascending).ThenBy(p => p.Id),
			PlayerSorting.IsBannedFromDdcl => playersQuery.OrderBy(p => p.IsBannedFromDdcl, ascending).ThenBy(p => p.Id),
			PlayerSorting.IsRightHanded => playersQuery.OrderBy(p => p.IsRightHanded, ascending).ThenBy(p => p.Id),
			PlayerSorting.PlayerName => playersQuery.OrderBy(p => p.PlayerName, ascending).ThenBy(p => p.Id),
			PlayerSorting.UsesLegacyAudio => playersQuery.OrderBy(p => p.UsesLegacyAudio, ascending).ThenBy(p => p.Id),
			PlayerSorting.UsesHrtf => playersQuery.OrderBy(p => p.UsesHrtf, ascending).ThenBy(p => p.Id),
			PlayerSorting.UsesInvertY => playersQuery.OrderBy(p => p.UsesInvertY, ascending).ThenBy(p => p.Id),
			PlayerSorting.VerticalSync => playersQuery.OrderBy(p => p.VerticalSync, ascending).ThenBy(p => p.Id),
			_ => playersQuery.OrderBy(p => p.Id, ascending),
		};

		List<PlayerEntity> players = playersQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetPlayerForOverview>
		{
			Results = players.ConvertAll(p => p.ToGetPlayerForOverview()),
			TotalResults = _dbContext.Players.Count(),
		};
	}

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = $"{Roles.Players},{Roles.Mods},{Roles.Spawnsets}")]
	public ActionResult<List<GetPlayerName>> GetPlayerNames()
	{
		var players = _dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.PlayerName })
			.ToList();

		return players.ConvertAll(p => new GetPlayerName
		{
			Id = p.Id,
			PlayerName = p.PlayerName,
		});
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Players)]
	public ActionResult<GetPlayer> GetPlayerById(int id)
	{
		PlayerEntity? player = _dbContext.Players
			.AsSingleQuery()
			.AsNoTracking()
			.Include(p => p.PlayerMods)
			.FirstOrDefault(p => p.Id == id);
		if (player == null)
			return NotFound();

		return player.ToGetPlayer();
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult> AddPlayer(AddPlayer addPlayer)
	{
		await _playerService.AddPlayerAsync(new Domain.Admin.Commands.Players.AddPlayer
		{
			Id = addPlayer.Id,
			ModIds = addPlayer.ModIds,
			CommonName = addPlayer.CommonName,
			DiscordUserId = addPlayer.DiscordUserId,
			CountryCode = addPlayer.CountryCode,
			Dpi = addPlayer.Dpi,
			InGameSens = addPlayer.InGameSens,
			Fov = addPlayer.Fov,
			IsRightHanded = addPlayer.IsRightHanded,
			HasFlashHandEnabled = addPlayer.HasFlashHandEnabled,
			Gamma = addPlayer.Gamma,
			UsesLegacyAudio = addPlayer.UsesLegacyAudio,
			UsesHrtf = addPlayer.UsesHrtf,
			UsesInvertY = addPlayer.UsesInvertY,
			VerticalSync = addPlayer.VerticalSync.ToDomain(),
			BanType = addPlayer.BanType.ToDomain(),
			BanDescription = addPlayer.BanDescription,
			BanResponsibleId = addPlayer.BanResponsibleId,
			IsBannedFromDdcl = addPlayer.IsBannedFromDdcl,
			HideSettings = addPlayer.HideSettings,
			HideDonations = addPlayer.HideDonations,
			HidePastUsernames = addPlayer.HidePastUsernames,
		});
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult> EditPlayerById(int id, EditPlayer editPlayer)
	{
		await _playerService.EditPlayerAsync(new Domain.Admin.Commands.Players.EditPlayer
		{
			Id = id,
			BanDescription = editPlayer.BanDescription,
			BanResponsibleId = editPlayer.BanResponsibleId,
			BanType = editPlayer.BanType.ToDomain(),
			CommonName = editPlayer.CommonName,
			CountryCode = editPlayer.CountryCode,
			DiscordUserId = editPlayer.DiscordUserId,
			Dpi = editPlayer.Dpi,
			Fov = editPlayer.Fov,
			Gamma = editPlayer.Gamma,
			HasFlashHandEnabled = editPlayer.HasFlashHandEnabled,
			HideDonations = editPlayer.HideDonations,
			HideSettings = editPlayer.HideSettings,
			HidePastUsernames = editPlayer.HidePastUsernames,
			InGameSens = editPlayer.InGameSens,
			IsBannedFromDdcl = editPlayer.IsBannedFromDdcl,
			IsRightHanded = editPlayer.IsRightHanded,
			ModIds = editPlayer.ModIds,
			UsesHrtf = editPlayer.UsesHrtf,
			UsesInvertY = editPlayer.UsesInvertY,
			UsesLegacyAudio = editPlayer.UsesLegacyAudio,
			VerticalSync = editPlayer.VerticalSync.ToDomain(),
		});
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult> DeletePlayerById(int id)
	{
		await _playerService.DeletePlayerAsync(id);
		return Ok();
	}
}
