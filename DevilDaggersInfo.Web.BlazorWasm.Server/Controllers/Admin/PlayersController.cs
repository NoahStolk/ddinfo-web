using DevilDaggersInfo.Web.BlazorWasm.Server.Clients.Leaderboard;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Server.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Server.Singletons.AuditLog;
using DevilDaggersInfo.Web.BlazorWasm.Shared;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/players")]
[Authorize(Roles = Roles.Players)]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly AuditLogger _auditLogger;

	public PlayersController(ApplicationDbContext dbContext, AuditLogger auditLogger)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Page<GetPlayerForOverview>> GetPlayers(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault,
		PlayerSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<PlayerEntity> playersQuery = _dbContext.Players.AsNoTracking();

		playersQuery = sortBy switch
		{
			PlayerSorting.BanDescription => playersQuery.OrderBy(p => p.BanDescription, ascending),
			PlayerSorting.BanResponsibleId => playersQuery.OrderBy(p => p.BanResponsibleId, ascending),
			PlayerSorting.CountryCode => playersQuery.OrderBy(p => p.CountryCode, ascending),
			PlayerSorting.Dpi => playersQuery.OrderBy(p => p.Dpi, ascending),
			PlayerSorting.Fov => playersQuery.OrderBy(p => p.Fov, ascending),
			PlayerSorting.Gamma => playersQuery.OrderBy(p => p.Gamma, ascending),
			PlayerSorting.HasFlashHandEnabled => playersQuery.OrderBy(p => p.HasFlashHandEnabled, ascending),
			PlayerSorting.HideDonations => playersQuery.OrderBy(p => p.HideDonations, ascending),
			PlayerSorting.HidePastUsernames => playersQuery.OrderBy(p => p.HidePastUsernames, ascending),
			PlayerSorting.HideSettings => playersQuery.OrderBy(p => p.HideSettings, ascending),
			PlayerSorting.InGameSens => playersQuery.OrderBy(p => p.InGameSens, ascending),
			PlayerSorting.IsBanned => playersQuery.OrderBy(p => p.IsBanned, ascending),
			PlayerSorting.IsBannedFromDdcl => playersQuery.OrderBy(p => p.IsBannedFromDdcl, ascending),
			PlayerSorting.IsRightHanded => playersQuery.OrderBy(p => p.IsRightHanded, ascending),
			PlayerSorting.PlayerName => playersQuery.OrderBy(p => p.PlayerName, ascending),
			PlayerSorting.UsesLegacyAudio => playersQuery.OrderBy(p => p.UsesLegacyAudio, ascending),
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
	public ActionResult<GetPlayer> GetPlayerById(int id)
	{
		PlayerEntity? player = _dbContext.Players
			.AsSingleQuery()
			.AsNoTracking()
			.Include(p => p.PlayerTitles)
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
	public async Task<ActionResult> AddPlayer(AddPlayer addPlayer)
	{
		if (addPlayer.IsBanned)
		{
			if (!string.IsNullOrWhiteSpace(addPlayer.CountryCode))
				return BadRequest("Banned players must not have a country code.");

			if (addPlayer.Dpi.HasValue ||
				addPlayer.InGameSens.HasValue ||
				addPlayer.Fov.HasValue ||
				addPlayer.IsRightHanded.HasValue ||
				addPlayer.HasFlashHandEnabled.HasValue ||
				addPlayer.Gamma.HasValue ||
				addPlayer.UsesLegacyAudio.HasValue)
			{
				return BadRequest("Banned players must not have settings.");
			}

			if (string.IsNullOrWhiteSpace(addPlayer.BanDescription))
				return BadRequest("BanDescription is required for banned players.");
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(addPlayer.BanDescription))
				return BadRequest("BanDescription must only be used for banned players.");

			if (addPlayer.BanResponsibleId.HasValue)
				return BadRequest("BanResponsibleId must only be used for banned players.");
		}

		if (_dbContext.Players.Any(p => p.Id == addPlayer.Id))
			return Conflict($"Player with ID '{addPlayer.Id}' already exists.");

		foreach (int modId in addPlayer.ModIds ?? new())
		{
			if (!_dbContext.Mods.Any(m => m.Id == modId))
				return BadRequest($"Mod with ID '{modId}' does not exist.");
		}

		foreach (int titleId in addPlayer.TitleIds ?? new())
		{
			if (!_dbContext.Titles.Any(t => t.Id == titleId))
				return BadRequest($"Title with ID '{titleId}' does not exist.");
		}

		PlayerEntity player = new()
		{
			Id = addPlayer.Id,
			PlayerName = await GetPlayerName(addPlayer.Id),
			CountryCode = addPlayer.CountryCode,
			Dpi = addPlayer.Dpi,
			InGameSens = addPlayer.InGameSens,
			Fov = addPlayer.Fov,
			IsRightHanded = addPlayer.IsRightHanded,
			HasFlashHandEnabled = addPlayer.HasFlashHandEnabled,
			Gamma = addPlayer.Gamma,
			UsesLegacyAudio = addPlayer.UsesLegacyAudio,
			IsBanned = addPlayer.IsBanned,
			BanDescription = addPlayer.BanDescription,
			BanResponsibleId = addPlayer.BanResponsibleId,
			IsBannedFromDdcl = addPlayer.IsBannedFromDdcl,
			HideSettings = addPlayer.HideSettings,
			HideDonations = addPlayer.HideDonations,
			HidePastUsernames = addPlayer.HidePastUsernames,
		};
		_dbContext.Players.Add(player);
		_dbContext.SaveChanges(); // Save changes here so PlayerTitle and PlayerMod entities can be assigned properly.

		UpdatePlayerMods(addPlayer.ModIds ?? new(), player.Id);
		UpdatePlayerTitles(addPlayer.TitleIds ?? new(), player.Id);
		_dbContext.SaveChanges();

		await _auditLogger.LogAdd(addPlayer, User, player.Id);

		return Ok(player.Id);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditPlayerById(int id, EditPlayer editPlayer)
	{
		if (editPlayer.IsBanned)
		{
			if (!string.IsNullOrWhiteSpace(editPlayer.CountryCode))
				return BadRequest("Banned players must not have a country code.");

			if (editPlayer.Dpi.HasValue ||
				editPlayer.InGameSens.HasValue ||
				editPlayer.Fov.HasValue ||
				editPlayer.IsRightHanded.HasValue ||
				editPlayer.HasFlashHandEnabled.HasValue ||
				editPlayer.Gamma.HasValue ||
				editPlayer.UsesLegacyAudio.HasValue)
			{
				return BadRequest("Banned players must not have settings.");
			}

			if (string.IsNullOrWhiteSpace(editPlayer.BanDescription))
				return BadRequest("BanDescription is required for banned players.");
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(editPlayer.BanDescription))
				return BadRequest("BanDescription must only be used for banned players.");

			if (editPlayer.BanResponsibleId.HasValue)
				return BadRequest("BanResponsibleId must only be used for banned players.");
		}

		foreach (int modId in editPlayer.ModIds ?? new())
		{
			if (!_dbContext.Mods.Any(m => m.Id == modId))
				return BadRequest($"Mod with ID '{modId}' does not exist.");
		}

		foreach (int titleId in editPlayer.TitleIds ?? new())
		{
			if (!_dbContext.Titles.Any(t => t.Id == titleId))
				return BadRequest($"Title with ID '{titleId}' does not exist.");
		}

		PlayerEntity? player = _dbContext.Players
			.AsSingleQuery()
			.Include(p => p.PlayerMods)
			.Include(p => p.PlayerTitles)
			.FirstOrDefault(p => p.Id == id);
		if (player == null)
			return NotFound();

		EditPlayer logDto = new()
		{
			CountryCode = player.CountryCode,
			Dpi = player.Dpi,
			InGameSens = player.InGameSens,
			Fov = player.Fov,
			IsRightHanded = player.IsRightHanded,
			HasFlashHandEnabled = player.HasFlashHandEnabled,
			Gamma = player.Gamma,
			UsesLegacyAudio = player.UsesLegacyAudio,
			HideSettings = player.HideSettings,
			HideDonations = player.HideDonations,
			HidePastUsernames = player.HidePastUsernames,
			ModIds = player.PlayerMods.ConvertAll(pam => pam.ModId),
			TitleIds = player.PlayerTitles.ConvertAll(pt => pt.TitleId),
			BanDescription = player.BanDescription,
			BanResponsibleId = player.BanResponsibleId,
			IsBanned = player.IsBanned,
			IsBannedFromDdcl = player.IsBannedFromDdcl,
		};

		player.PlayerName = await GetPlayerName(id);
		player.CountryCode = editPlayer.CountryCode;
		player.Dpi = editPlayer.Dpi;
		player.InGameSens = editPlayer.InGameSens;
		player.Fov = editPlayer.Fov;
		player.IsRightHanded = editPlayer.IsRightHanded;
		player.HasFlashHandEnabled = editPlayer.HasFlashHandEnabled;
		player.Gamma = editPlayer.Gamma;
		player.UsesLegacyAudio = editPlayer.UsesLegacyAudio;
		player.HideSettings = editPlayer.HideSettings;
		player.HideDonations = editPlayer.HideDonations;
		player.HidePastUsernames = editPlayer.HidePastUsernames;
		player.BanDescription = editPlayer.BanDescription;
		player.BanResponsibleId = editPlayer.BanResponsibleId;
		player.IsBanned = editPlayer.IsBanned;
		player.IsBannedFromDdcl = editPlayer.IsBannedFromDdcl;

		UpdatePlayerMods(editPlayer.ModIds ?? new(), player.Id);
		UpdatePlayerTitles(editPlayer.TitleIds ?? new(), player.Id);
		_dbContext.SaveChanges();

		await _auditLogger.LogEdit(logDto, editPlayer, User, player.Id);

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeletePlayerById(int id)
	{
		PlayerEntity? player = _dbContext.Players
			.Include(p => p.PlayerTitles)
			.FirstOrDefault(p => p.Id == id);
		if (player == null)
			return NotFound();

		if (_dbContext.CustomEntries.Any(ce => ce.PlayerId == id))
			return BadRequest("Player with custom leaderboard scores cannot be deleted.");

		if (_dbContext.Donations.Any(d => d.PlayerId == id))
			return BadRequest("Player with donations cannot be deleted.");

		if (_dbContext.PlayerMods.Any(pam => pam.PlayerId == id))
			return BadRequest("Player with mods cannot be deleted.");

		if (_dbContext.Spawnsets.Any(sf => sf.PlayerId == id))
			return BadRequest("Player with spawnsets cannot be deleted.");

		_dbContext.Players.Remove(player);
		_dbContext.SaveChanges();

		await _auditLogger.LogDelete(player, User, player.Id);

		return Ok();
	}

	private static async Task<string> GetPlayerName(int id)
	{
		try
		{
			return (await LeaderboardClient.Instance.GetUserById(id)).Username;
		}
		catch
		{
			return string.Empty;
		}
	}

	private void UpdatePlayerMods(List<int> modIds, int playerId)
	{
		foreach (PlayerModEntity newEntity in modIds.ConvertAll(ami => new PlayerModEntity { ModId = ami, PlayerId = playerId }))
		{
			if (!_dbContext.PlayerMods.Any(pam => pam.ModId == newEntity.ModId && pam.PlayerId == newEntity.PlayerId))
				_dbContext.PlayerMods.Add(newEntity);
		}

		foreach (PlayerModEntity entityToRemove in _dbContext.PlayerMods.Where(pam => pam.PlayerId == playerId && !modIds.Contains(pam.ModId)))
			_dbContext.PlayerMods.Remove(entityToRemove);
	}

	private void UpdatePlayerTitles(List<int> titleIds, int playerId)
	{
		foreach (PlayerTitleEntity newEntity in titleIds.ConvertAll(ti => new PlayerTitleEntity { TitleId = ti, PlayerId = playerId }))
		{
			if (!_dbContext.PlayerTitles.Any(pt => pt.TitleId == newEntity.TitleId && pt.PlayerId == newEntity.PlayerId))
				_dbContext.PlayerTitles.Add(newEntity);
		}

		foreach (PlayerTitleEntity entityToRemove in _dbContext.PlayerTitles.Where(pam => pam.PlayerId == playerId && !titleIds.Contains(pam.TitleId)))
			_dbContext.PlayerTitles.Remove(entityToRemove);
	}
}
