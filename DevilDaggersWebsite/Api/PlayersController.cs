using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Authorization;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto.Players;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Api
{
	[Route("api/players")]
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
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult<List<GetPlayer>> GetPlayers()
		{
			List<Player> players = _dbContext.Players
				.AsNoTracking()
				.Include(p => p.PlayerTitles)
				.Include(p => p.PlayerAssetMods)
				.ToList();

			return players.ConvertAll(p => new GetPlayer
			{
				Id = p.Id,
				PlayerName = p.PlayerName,
				CountryCode = p.CountryCode,
				Dpi = p.Dpi,
				InGameSens = p.InGameSens,
				Fov = p.Fov,
				IsRightHanded = p.IsRightHanded,
				HasFlashHandEnabled = p.HasFlashHandEnabled,
				Gamma = p.Gamma,
				UsesLegacyAudio = p.UsesLegacyAudio,
				IsBanned = p.IsBanned,
				BanDescription = p.BanDescription,
				BanResponsibleId = p.BanResponsibleId,
				IsBannedFromDdcl = p.IsBannedFromDdcl,
				HideSettings = p.HideSettings,
				HideDonations = p.HideDonations,
				HidePastUsernames = p.HidePastUsernames,
				AssetModIds = p.PlayerAssetMods.ConvertAll(pam => pam.AssetModId),
				TitleIds = p.PlayerTitles.ConvertAll(pt => pt.TitleId),
			});
		}

		[HttpPost]
		[Authorize(Policies.PlayersPolicy)]
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
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> EditPlayer(int id, EditPlayer editPlayer)
		{
			Player? player = _dbContext.Players
				.Include(p => p.PlayerAssetMods)
				.Include(p => p.PlayerTitles)
				.FirstOrDefault(p => p.Id == id);
			if (player == null)
				return NotFound();

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

			UpdateManyToManyRelations(editPlayer.TitleIds ?? new(), editPlayer.AssetModIds ?? new(), player.Id);
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editPlayer, User, player.Id);

			return Ok();
		}

		[HttpPatch("{id}/update-name")]
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> UpdatePlayerName(int id)
		{
			Player? player = _dbContext.Players.FirstOrDefault(p => p.Id == id);
			if (player == null)
				return NotFound();

			player.PlayerName = await GetPlayerNameOrDefault(id, player.PlayerName);
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpPatch("{id}/ban")]
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult BanPlayer(int id, BanPlayer banPlayer)
		{
			Player? player = _dbContext.Players.FirstOrDefault(p => p.Id == id);
			if (player == null)
				return NotFound();

			player.IsBanned = true;
			player.BanDescription = banPlayer.BanDescription;
			player.BanResponsibleId = banPlayer.BanResponsibleId;
			player.CountryCode = null;
			player.Dpi = null;
			player.InGameSens = null;
			player.Fov = null;
			player.IsRightHanded = null;
			player.HasFlashHandEnabled = null;
			player.Gamma = null;
			player.UsesLegacyAudio = null;
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpPatch("{id}/ban-ddcl")]
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult BanPlayerFromDdcl(int id)
		{
			Player? player = _dbContext.Players.FirstOrDefault(p => p.Id == id);
			if (player == null)
				return NotFound();

			player.IsBannedFromDdcl = true;
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpPatch("{id}/unban")]
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult UnbanPlayer(int id)
		{
			Player? player = _dbContext.Players.FirstOrDefault(p => p.Id == id);
			if (player == null)
				return NotFound();

			player.IsBanned = false;
			player.BanDescription = null;
			player.BanResponsibleId = null;
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpPatch("{id}/unban-ddcl")]
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult UnbanPlayerFromDdcl(int id)
		{
			Player? player = _dbContext.Players.FirstOrDefault(p => p.Id == id);
			if (player == null)
				return NotFound();

			player.IsBannedFromDdcl = false;
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> DeletePlayer(int id)
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

		private async Task<string> GetPlayerNameOrDefault(int id, string defaultValue)
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
