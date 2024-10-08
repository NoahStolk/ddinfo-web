using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class PlayerService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IDdLeaderboardService _ddLeaderboardService;

	public PlayerService(ApplicationDbContext dbContext, IDdLeaderboardService ddLeaderboardService)
	{
		_dbContext = dbContext;
		_ddLeaderboardService = ddLeaderboardService;
	}

	public async Task AddPlayerAsync(ApiSpec.Admin.Players.AddPlayer addPlayer)
	{
		Validate(
			banType: addPlayer.BanType.ToDomain(),
			countryCode: addPlayer.CountryCode,
			dpi: addPlayer.Dpi,
			inGameSens: addPlayer.InGameSens,
			fov: addPlayer.Fov,
			isRightHanded: addPlayer.IsRightHanded,
			hasFlashHandEnabled: addPlayer.HasFlashHandEnabled,
			gamma: addPlayer.Gamma,
			usesLegacyAudio: addPlayer.UsesLegacyAudio,
			usesHrtf: addPlayer.UsesHrtf,
			usesInvertY: addPlayer.UsesInvertY,
			verticalSync: addPlayer.VerticalSync.ToDomain(),
			banDescription: addPlayer.BanDescription,
			banResponsibleId: addPlayer.BanResponsibleId);

		if (await _dbContext.Players.AnyAsync(p => p.Id == addPlayer.Id))
			throw new AdminDomainException($"Player with ID '{addPlayer.Id}' already exists.");

		foreach (int modId in addPlayer.ModIds ?? [])
		{
			if (!await _dbContext.Mods.AnyAsync(m => m.Id == modId))
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
			VerticalSync = addPlayer.VerticalSync.ToDomain(),
			BanType = addPlayer.BanType.ToDomain(),
			BanDescription = addPlayer.BanDescription,
			BanResponsibleId = addPlayer.BanResponsibleId,
			IsBannedFromDdcl = addPlayer.IsBannedFromDdcl,
			HideSettings = addPlayer.HideSettings,
			HideDonations = addPlayer.HideDonations,
			HidePastUsernames = addPlayer.HidePastUsernames,
		};
		_dbContext.Players.Add(player);
		await _dbContext.SaveChangesAsync(); // Save changes here so PlayerMod entities can be assigned properly.

		UpdatePlayerMods(addPlayer.ModIds ?? [], player.Id);
		await _dbContext.SaveChangesAsync();
	}

	public async Task EditPlayerAsync(int id, ApiSpec.Admin.Players.EditPlayer editPlayer)
	{
		Validate(
			banType: editPlayer.BanType.ToDomain(),
			countryCode: editPlayer.CountryCode,
			dpi: editPlayer.Dpi,
			inGameSens: editPlayer.InGameSens,
			fov: editPlayer.Fov,
			isRightHanded: editPlayer.IsRightHanded,
			hasFlashHandEnabled: editPlayer.HasFlashHandEnabled,
			gamma: editPlayer.Gamma,
			usesLegacyAudio: editPlayer.UsesLegacyAudio,
			usesHrtf: editPlayer.UsesHrtf,
			usesInvertY: editPlayer.UsesInvertY,
			verticalSync: editPlayer.VerticalSync.ToDomain(),
			banDescription: editPlayer.BanDescription,
			banResponsibleId: editPlayer.BanResponsibleId);

		foreach (int modId in editPlayer.ModIds ?? [])
		{
			if (!await _dbContext.Mods.AnyAsync(m => m.Id == modId))
				throw new AdminDomainException($"Mod with ID '{modId}' does not exist.");
		}

		PlayerEntity? player = await _dbContext.Players
			.Include(p => p.PlayerMods)
			.FirstOrDefaultAsync(p => p.Id == id);
		if (player == null)
			throw new NotFoundException($"Player with ID '{id}' does not exist.");

		player.PlayerName = await GetPlayerName(id);
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
		player.VerticalSync = editPlayer.VerticalSync.ToDomain();
		player.HideSettings = editPlayer.HideSettings;
		player.HideDonations = editPlayer.HideDonations;
		player.HidePastUsernames = editPlayer.HidePastUsernames;
		player.BanDescription = editPlayer.BanDescription;
		player.BanResponsibleId = editPlayer.BanResponsibleId;
		player.BanType = editPlayer.BanType.ToDomain();
		player.IsBannedFromDdcl = editPlayer.IsBannedFromDdcl;

		UpdatePlayerMods(editPlayer.ModIds ?? [], player.Id);
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeletePlayerAsync(int id)
	{
		PlayerEntity? player = await _dbContext.Players.FirstOrDefaultAsync(p => p.Id == id);
		if (player == null)
			throw new NotFoundException($"Player with ID '{id}' does not exist.");

		if (await _dbContext.CustomEntries.AnyAsync(ce => ce.PlayerId == id))
			throw new AdminDomainException("Player with custom leaderboard scores cannot be deleted.");

		if (await _dbContext.Donations.AnyAsync(d => d.PlayerId == id))
			throw new AdminDomainException("Player with donations cannot be deleted.");

		if (await _dbContext.PlayerMods.AnyAsync(pam => pam.PlayerId == id))
			throw new AdminDomainException("Player with mods cannot be deleted.");

		if (await _dbContext.Spawnsets.AnyAsync(sf => sf.PlayerId == id))
			throw new AdminDomainException("Player with spawnsets cannot be deleted.");

		_dbContext.Players.Remove(player);
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

	private static void Validate(
		Entities.Enums.BanType banType,
		string? countryCode,
		int? dpi,
		float? inGameSens,
		int? fov,
		bool? isRightHanded,
		bool? hasFlashHandEnabled,
		float? gamma,
		bool? usesLegacyAudio,
		bool? usesHrtf,
		bool? usesInvertY,
		Entities.Enums.VerticalSync verticalSync,
		string? banDescription,
		int? banResponsibleId)
	{
		if (banType != Entities.Enums.BanType.NotBanned)
		{
			if (!string.IsNullOrWhiteSpace(countryCode))
				throw new AdminDomainException("Banned players must not have a country code.");

			if (dpi.HasValue ||
				inGameSens.HasValue ||
				fov.HasValue ||
				isRightHanded.HasValue ||
				hasFlashHandEnabled.HasValue ||
				gamma.HasValue ||
				usesLegacyAudio.HasValue ||
				usesHrtf.HasValue ||
				usesInvertY.HasValue ||
				verticalSync != Entities.Enums.VerticalSync.Unknown)
			{
				throw new AdminDomainException("Banned players must not have settings.");
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(banDescription))
				throw new AdminDomainException("BanDescription must only be used for banned players.");

			if (banResponsibleId.HasValue)
				throw new AdminDomainException("BanResponsibleId must only be used for banned players.");
		}
	}
}
