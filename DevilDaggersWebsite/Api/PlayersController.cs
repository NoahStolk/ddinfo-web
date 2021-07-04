using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto.Players;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Api
{
	[Route("api/players")]
	[ApiController]
	public class PlayersController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;

		public PlayersController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		//[Authorize(Policies.PlayersPolicy)]
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
		//[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult AddTitle(AddPlayer addPlayer)
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
				return Conflict($"Player with ID {addPlayer.Id} already exists.");

			Player player = new()
			{
				Id = addPlayer.Id,
				PlayerName = string.IsNullOrWhiteSpace(addPlayer.PlayerName) ? (LeaderboardClient.Instance.GetUserById(addPlayer.Id).Result?.Username ?? string.Empty) : addPlayer.PlayerName,
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

			return Ok();
		}

		/*
		[HttpPut("{id}")]
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult EditTitle(int id, EditTitle editTitle)
		{
			foreach (int playerId in editTitle.PlayerIds ?? new())
			{
				if (!_dbContext.Players.Any(p => p.Id == playerId))
					return BadRequest($"Player with ID {playerId} does not exist.");
			}

			Title? title = _dbContext.Titles.FirstOrDefault(t => t.Id == id);
			if (title == null)
				return NotFound();

			title.Name = editTitle.Name;

			UpdatePlayerTitles(editTitle.PlayerIds ?? new(), title.Id);
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Policies.PlayersPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult DeleteTitle(int id)
		{
			Title? title = _dbContext.Titles
				.Include(t => t.PlayerTitles)
				.FirstOrDefault(t => t.Id == id);
			if (title == null)
				return NotFound();

			_dbContext.Titles.Remove(title);
			_dbContext.SaveChanges();

			return Ok();
		}
		*/

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
				if (!_dbContext.PlayerTitles.Any(pam => pam.TitleId == newEntity.TitleId && pam.PlayerId == newEntity.PlayerId))
					_dbContext.PlayerTitles.Add(newEntity);
			}

			foreach (PlayerTitle entityToRemove in _dbContext.PlayerTitles.Where(pam => pam.PlayerId == playerId && !titleIds.Contains(pam.TitleId)))
				_dbContext.PlayerTitles.Remove(entityToRemove);
		}
	}
}
