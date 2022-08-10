using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Commands.Players;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class PlayerService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IDdLeaderboardService _ddLeaderboardService;

	public PlayerService(ApplicationDbContext dbContext, IDdLeaderboardService ddLeaderboardService)
	{
		_dbContext = dbContext;
		_ddLeaderboardService = ddLeaderboardService;
	}

	public async Task AddPlayerAsync(AddPlayer addPlayer)
	{
		if (addPlayer.BanType != BanType.NotBanned)
		{
			if (!string.IsNullOrWhiteSpace(addPlayer.CountryCode))
				throw new AdminDomainException("Banned players must not have a country code.");

			if (addPlayer.Dpi.HasValue ||
				addPlayer.InGameSens.HasValue ||
				addPlayer.Fov.HasValue ||
				addPlayer.IsRightHanded.HasValue ||
				addPlayer.HasFlashHandEnabled.HasValue ||
				addPlayer.Gamma.HasValue ||
				addPlayer.UsesLegacyAudio.HasValue ||
				addPlayer.UsesHrtf.HasValue ||
				addPlayer.UsesInvertY.HasValue ||
				addPlayer.VerticalSync != VerticalSync.Unknown)
			{
				throw new AdminDomainException("Banned players must not have settings.");
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(addPlayer.BanDescription))
				throw new AdminDomainException("BanDescription must only be used for banned players.");

			if (addPlayer.BanResponsibleId.HasValue)
				throw new AdminDomainException("BanResponsibleId must only be used for banned players.");
		}

		if (_dbContext.Players.Any(p => p.Id == addPlayer.Id))
			throw new AdminDomainException($"Player with ID '{addPlayer.Id}' already exists.");

		foreach (int modId in addPlayer.ModIds ?? new())
		{
			if (!_dbContext.Mods.Any(m => m.Id == modId))
				throw new AdminDomainException($"Mod with ID '{modId}' does not exist.");
		}

		PlayerEntity player = new()
		{
			Id = addPlayer.Id,
			PlayerName = await GetPlayerName(addPlayer.Id),
			CommonName = addPlayer.CommonName,
			DiscordUserId = (ulong?)addPlayer.DiscordUserId,
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
			VerticalSync = addPlayer.VerticalSync,
			BanType = addPlayer.BanType,
			BanDescription = addPlayer.BanDescription,
			BanResponsibleId = addPlayer.BanResponsibleId,
			IsBannedFromDdcl = addPlayer.IsBannedFromDdcl,
			HideSettings = addPlayer.HideSettings,
			HideDonations = addPlayer.HideDonations,
			HidePastUsernames = addPlayer.HidePastUsernames,
		};
		_dbContext.Players.Add(player);
		_dbContext.SaveChanges(); // Save changes here so PlayerMod entities can be assigned properly.

		UpdatePlayerMods(addPlayer.ModIds ?? new(), player.Id);
		await _dbContext.SaveChangesAsync();
	}

	public async Task EditPlayerAsync(EditPlayer editPlayer)
	{
		if (editPlayer.BanType != BanType.NotBanned)
		{
			if (!string.IsNullOrWhiteSpace(editPlayer.CountryCode))
				throw new AdminDomainException("Banned players must not have a country code.");

			if (editPlayer.Dpi.HasValue ||
				editPlayer.InGameSens.HasValue ||
				editPlayer.Fov.HasValue ||
				editPlayer.IsRightHanded.HasValue ||
				editPlayer.HasFlashHandEnabled.HasValue ||
				editPlayer.Gamma.HasValue ||
				editPlayer.UsesLegacyAudio.HasValue ||
				editPlayer.UsesHrtf.HasValue ||
				editPlayer.UsesInvertY.HasValue ||
				editPlayer.VerticalSync != VerticalSync.Unknown)
			{
				throw new AdminDomainException("Banned players must not have settings.");
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(editPlayer.BanDescription))
				throw new AdminDomainException("BanDescription must only be used for banned players.");

			if (editPlayer.BanResponsibleId.HasValue)
				throw new AdminDomainException("BanResponsibleId must only be used for banned players.");
		}

		foreach (int modId in editPlayer.ModIds ?? new())
		{
			if (!_dbContext.Mods.Any(m => m.Id == modId))
				throw new AdminDomainException($"Mod with ID '{modId}' does not exist.");
		}

		PlayerEntity? player = _dbContext.Players
			.Include(p => p.PlayerMods)
			.FirstOrDefault(p => p.Id == editPlayer.Id);
		if (player == null)
			throw new NotFoundException($"Player with ID '{editPlayer.Id}' does not exist.");

		player.PlayerName = await GetPlayerName(editPlayer.Id);
		player.CommonName = editPlayer.CommonName;
		player.DiscordUserId = (ulong?)editPlayer.DiscordUserId;
		player.CountryCode = editPlayer.CountryCode;
		player.Dpi = editPlayer.Dpi;
		player.InGameSens = editPlayer.InGameSens;
		player.Fov = editPlayer.Fov;
		player.IsRightHanded = editPlayer.IsRightHanded;
		player.HasFlashHandEnabled = editPlayer.HasFlashHandEnabled;
		player.Gamma = editPlayer.Gamma;
		player.UsesLegacyAudio = editPlayer.UsesLegacyAudio;
		player.UsesHrtf = editPlayer.UsesHrtf;
		player.UsesInvertY = editPlayer.UsesInvertY;
		player.VerticalSync = editPlayer.VerticalSync;
		player.HideSettings = editPlayer.HideSettings;
		player.HideDonations = editPlayer.HideDonations;
		player.HidePastUsernames = editPlayer.HidePastUsernames;
		player.BanDescription = editPlayer.BanDescription;
		player.BanResponsibleId = editPlayer.BanResponsibleId;
		player.BanType = editPlayer.BanType;
		player.IsBannedFromDdcl = editPlayer.IsBannedFromDdcl;

		UpdatePlayerMods(editPlayer.ModIds ?? new(), player.Id);
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeletePlayerAsync(int id)
	{
		PlayerEntity? player = _dbContext.Players.FirstOrDefault(p => p.Id == id);
		if (player == null)
			throw new NotFoundException($"Player with ID '{id}' does not exist.");

		if (_dbContext.CustomEntries.Any(ce => ce.PlayerId == id))
			throw new AdminDomainException("Player with custom leaderboard scores cannot be deleted.");

		if (_dbContext.Donations.Any(d => d.PlayerId == id))
			throw new AdminDomainException("Player with donations cannot be deleted.");

		if (_dbContext.PlayerMods.Any(pam => pam.PlayerId == id))
			throw new AdminDomainException("Player with mods cannot be deleted.");

		if (_dbContext.Spawnsets.Any(sf => sf.PlayerId == id))
			throw new AdminDomainException("Player with spawnsets cannot be deleted.");

		_dbContext.Players.Remove(player);
		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdateProfileAsync(ClaimsPrincipal claimsPrincipal, int id, EditPlayerProfile editPlayerProfile)
	{
		string? userName = claimsPrincipal.GetName();
		UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
		if (user == null)
			throw new UnauthorizedAccessException();

		if (!user.PlayerId.HasValue)
			throw new InvalidProfileRequestException("User is not linked to a player.");

		if (user.PlayerId != id)
			throw new ForbiddenException("Not allowed to access another player's profile.");

		PlayerEntity? player = await _dbContext.Players.FirstOrDefaultAsync(p => p.Id == id);
		if (player == null)
			throw new NotFoundException($"Player with ID '{id}' could not be found.");

		if (player.BanType != BanType.NotBanned)
			throw new InvalidProfileRequestException("Player is banned.");

		player.CountryCode = editPlayerProfile.CountryCode;
		player.Dpi = editPlayerProfile.Dpi;
		player.Fov = editPlayerProfile.Fov;
		player.Gamma = editPlayerProfile.Gamma;
		player.HasFlashHandEnabled = editPlayerProfile.HasFlashHandEnabled;
		player.HideDonations = editPlayerProfile.HideDonations;
		player.HidePastUsernames = editPlayerProfile.HidePastUsernames;
		player.HideSettings = editPlayerProfile.HideSettings;
		player.InGameSens = editPlayerProfile.InGameSens;
		player.IsRightHanded = editPlayerProfile.IsRightHanded;
		player.UsesHrtf = editPlayerProfile.UsesHrtf;
		player.UsesInvertY = editPlayerProfile.UsesInvertY;
		player.UsesLegacyAudio = editPlayerProfile.UsesLegacyAudio;
		player.VerticalSync = editPlayerProfile.VerticalSync;

		await _dbContext.SaveChangesAsync();
	}

	private async Task<string> GetPlayerName(int id)
	{
		try
		{
			return (await _ddLeaderboardService.GetEntryById(id)).Username;
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
}
