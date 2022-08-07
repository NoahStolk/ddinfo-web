using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class ProfileService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IAuditLogger _auditLogger;

	public ProfileService(ApplicationDbContext dbContext, IAuditLogger auditLogger)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
	}

	public async Task<PlayerProfile> GetProfileAsync(ClaimsPrincipal claimsPrincipal, int id)
	{
		string? userName = claimsPrincipal.GetName();
		UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == userName);
		if (user == null)
			throw new UnauthorizedAccessException();

		if (!user.PlayerId.HasValue)
			throw new InvalidProfileRequestException("User is not linked to a player.");

		if (user.PlayerId != id)
			throw new ForbiddenException("Not allowed to access another player's profile.");

		PlayerEntity? player = await _dbContext.Players
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);
		if (player == null)
			throw new NotFoundException($"Player with ID '{id}' could not be found.");

		if (player.BanType != BanType.NotBanned)
			throw new InvalidProfileRequestException("Player is banned.");

		return new()
		{
			CountryCode = player.CountryCode,
			Dpi = player.Dpi,
			Fov = player.Fov,
			Gamma = player.Gamma,
			HasFlashHandEnabled = player.HasFlashHandEnabled,
			HideDonations = player.HideDonations,
			HidePastUsernames = player.HidePastUsernames,
			HideSettings = player.HideSettings,
			InGameSens = player.InGameSens,
			IsRightHanded = player.IsRightHanded,
			UsesHrtf = player.UsesHrtf,
			UsesInvertY = player.UsesInvertY,
			UsesLegacyAudio = player.UsesLegacyAudio,
			VerticalSync = player.VerticalSync,
		};
	}

	public async Task UpdateProfileAsync(ClaimsPrincipal claimsPrincipal, int id, PlayerProfile editPlayerProfile)
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

		PlayerProfile oldLog = new()
		{
			CountryCode = player.CountryCode,
			Dpi = player.Dpi,
			InGameSens = player.InGameSens,
			Fov = player.Fov,
			IsRightHanded = player.IsRightHanded,
			HasFlashHandEnabled = player.HasFlashHandEnabled,
			Gamma = player.Gamma,
			UsesLegacyAudio = player.UsesLegacyAudio,
			UsesHrtf = player.UsesHrtf,
			UsesInvertY = player.UsesInvertY,
			VerticalSync = player.VerticalSync,
			HideSettings = player.HideSettings,
			HideDonations = player.HideDonations,
			HidePastUsernames = player.HidePastUsernames,
		};

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

		_auditLogger.LogEdit(oldLog.GetLog(), editPlayerProfile.GetLog(), claimsPrincipal, player.Id);
	}
}
