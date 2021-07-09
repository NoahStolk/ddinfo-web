using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Players;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Api
{
	[Route("api/players/admin")]
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
		[Authorize(Roles = Roles.Players)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult<Page<GetPlayerBase>> GetPlayers([Range(0, 1000)] int pageIndex = 0, [Range(5, 50)] int pageSize = 25, string? sortBy = null, bool ascending = false)
		{
			IQueryable<Player> playersQuery = _dbContext.Players.AsNoTracking();

			if (sortBy != null)
				playersQuery = playersQuery.OrderByMember(sortBy, ascending);

			List<Player> players = playersQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetPlayerBase>
			{
				Results = players.ConvertAll(p => new GetPlayerBase
				{
					Id = p.Id,
					PlayerName = p.PlayerName,
					CountryCode = p.CountryCode,
					Dpi = p.Dpi,
					InGameSens = p.InGameSens,
					Fov = p.Fov,
					IsBanned = p.IsBanned,
					BanDescription = p.BanDescription,
					BanResponsibleId = p.BanResponsibleId,
					Gamma = p.Gamma,
					HasFlashHandEnabled = p.HasFlashHandEnabled,
					HideDonations = p.HideDonations,
					HidePastUsernames = p.HidePastUsernames,
					HideSettings = p.HideSettings,
					IsBannedFromDdcl = p.IsBannedFromDdcl,
					IsRightHanded = p.IsRightHanded,
					UsesLegacyAudio = p.UsesLegacyAudio,
				}),
				TotalResults = _dbContext.Players.Count(),
			};
		}

		[HttpGet("names")]
		[Authorize(Roles = Roles.Players)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
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
		[Authorize(Roles = Roles.Players)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult<GetPlayer> GetPlayerById(int id)
		{
			Player? player = _dbContext.Players
				.AsSingleQuery()
				.AsNoTracking()
				.Include(p => p.PlayerTitles)
				.Include(p => p.PlayerAssetMods)
				.FirstOrDefault(p => p.Id == id);
			if (player == null)
				return NotFound();

			return new GetPlayer
			{
				Id = player.Id,
				PlayerName = player.PlayerName,
				CountryCode = player.CountryCode,
				Dpi = player.Dpi,
				InGameSens = player.InGameSens,
				Fov = player.Fov,
				IsRightHanded = player.IsRightHanded,
				HasFlashHandEnabled = player.HasFlashHandEnabled,
				Gamma = player.Gamma,
				UsesLegacyAudio = player.UsesLegacyAudio,
				IsBanned = player.IsBanned,
				BanDescription = player.BanDescription,
				BanResponsibleId = player.BanResponsibleId,
				IsBannedFromDdcl = player.IsBannedFromDdcl,
				HideSettings = player.HideSettings,
				HideDonations = player.HideDonations,
				HidePastUsernames = player.HidePastUsernames,
				AssetModIds = player.PlayerAssetMods.ConvertAll(pam => pam.AssetModId),
				TitleIds = player.PlayerTitles.ConvertAll(pt => pt.TitleId),
			};
		}

		[HttpPost]
		[Authorize(Roles = Roles.Players)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[EndpointConsumer(EndpointConsumers.Admin)]
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
			}

			if (_dbContext.Players.Any(p => p.Id == addPlayer.Id))
				return Conflict($"Player with ID '{addPlayer.Id}' already exists.");

			foreach (int modId in addPlayer.AssetModIds ?? new())
			{
				if (!_dbContext.AssetMods.Any(m => m.Id == modId))
					return BadRequest($"Mod with ID '{modId}' does not exist.");
			}

			foreach (int titleId in addPlayer.TitleIds ?? new())
			{
				if (!_dbContext.Titles.Any(t => t.Id == titleId))
					return BadRequest($"Title with ID '{titleId}' does not exist.");
			}

			Player player = new()
			{
				Id = addPlayer.Id,
				PlayerName = string.IsNullOrWhiteSpace(addPlayer.PlayerName) ? await GetPlayerNameOrDefault(addPlayer.Id, string.Empty) : addPlayer.PlayerName,
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
			_dbContext.SaveChanges(); // Save changes here so PlayerTitle and PlayerAssetMod entities can be assigned properly.

			UpdateManyToManyRelations(addPlayer.TitleIds ?? new(), addPlayer.AssetModIds ?? new(), player.Id);
			_dbContext.SaveChanges();

			await _auditLogger.LogAdd(addPlayer, User, player.Id);

			return Ok(player.Id);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = Roles.Players)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
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
			}

			foreach (int modId in editPlayer.AssetModIds ?? new())
			{
				if (!_dbContext.AssetMods.Any(m => m.Id == modId))
					return BadRequest($"Mod with ID '{modId}' does not exist.");
			}

			foreach (int titleId in editPlayer.TitleIds ?? new())
			{
				if (!_dbContext.Titles.Any(t => t.Id == titleId))
					return BadRequest($"Title with ID '{titleId}' does not exist.");
			}

			Player? player = _dbContext.Players
				.AsSingleQuery()
				.Include(p => p.PlayerAssetMods)
				.Include(p => p.PlayerTitles)
				.FirstOrDefault(p => p.Id == id);
			if (player == null)
				return NotFound();

			EditPlayer logDto = new()
			{
				PlayerName = player.PlayerName,
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
				AssetModIds = player.PlayerAssetMods.ConvertAll(pam => pam.AssetModId),
				TitleIds = player.PlayerTitles.ConvertAll(pt => pt.TitleId),
				BanDescription = player.BanDescription,
				BanResponsibleId = player.BanResponsibleId,
				IsBanned = player.IsBanned,
				IsBannedFromDdcl = player.IsBannedFromDdcl,
			};

			player.PlayerName = string.IsNullOrWhiteSpace(editPlayer.PlayerName) ? await GetPlayerNameOrDefault(id, player.PlayerName) : editPlayer.PlayerName;
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

			UpdateManyToManyRelations(editPlayer.TitleIds ?? new(), editPlayer.AssetModIds ?? new(), player.Id);
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editPlayer, User, player.Id);

			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = Roles.Players)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> DeletePlayerById(int id)
		{
			Player? player = _dbContext.Players
				.Include(p => p.PlayerTitles)
				.FirstOrDefault(p => p.Id == id);
			if (player == null)
				return NotFound();

			if (_dbContext.CustomEntries.Any(ce => ce.PlayerId == id))
				return BadRequest("Player with custom leaderboard scores cannot be deleted.");

			if (_dbContext.Donations.Any(d => d.PlayerId == id))
				return BadRequest("Player with donations cannot be deleted.");

			if (_dbContext.PlayerAssetMods.Any(pam => pam.PlayerId == id))
				return BadRequest("Player with mods cannot be deleted.");

			if (_dbContext.SpawnsetFiles.Any(sf => sf.PlayerId == id))
				return BadRequest("Player with spawnsets cannot be deleted.");

			_dbContext.Players.Remove(player);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(player, User, player.Id);

			return Ok();
		}

		private static async Task<string> GetPlayerNameOrDefault(int id, string defaultValue)
			=> (await LeaderboardClient.Instance.GetUserById(id))?.Username ?? defaultValue;

		private void UpdateManyToManyRelations(List<int> assetModIds, List<int> titleIds, int playerId)
		{
			foreach (PlayerAssetMod newEntity in assetModIds.ConvertAll(ami => new PlayerAssetMod { AssetModId = ami, PlayerId = playerId }))
			{
				if (!_dbContext.PlayerAssetMods.Any(pam => pam.AssetModId == newEntity.AssetModId && pam.PlayerId == newEntity.PlayerId))
					_dbContext.PlayerAssetMods.Add(newEntity);
			}

			foreach (PlayerAssetMod entityToRemove in _dbContext.PlayerAssetMods.Where(pam => pam.PlayerId == playerId && !assetModIds.Contains(pam.AssetModId)))
				_dbContext.PlayerAssetMods.Remove(entityToRemove);

			foreach (PlayerTitle newEntity in titleIds.ConvertAll(ti => new PlayerTitle { TitleId = ti, PlayerId = playerId }))
			{
				if (!_dbContext.PlayerTitles.Any(pt => pt.TitleId == newEntity.TitleId && pt.PlayerId == newEntity.PlayerId))
					_dbContext.PlayerTitles.Add(newEntity);
			}

			foreach (PlayerTitle entityToRemove in _dbContext.PlayerTitles.Where(pam => pam.PlayerId == playerId && !titleIds.Contains(pam.TitleId)))
				_dbContext.PlayerTitles.Remove(entityToRemove);
		}
	}
}
