using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Main.Converters.ApiToDomain;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Services;

public class PlayerProfileService
{
	private readonly ApplicationDbContext _dbContext;

	public PlayerProfileService(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task UpdateProfileAsync(ClaimsPrincipal claimsPrincipal, int id, ApiSpec.Main.Players.EditPlayerProfile editPlayerProfile)
	{
		string? userName = claimsPrincipal.GetName();
		UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == userName);
		if (user == null)
			throw new UnauthorizedException();

		if (!user.PlayerId.HasValue)
			throw new InvalidProfileRequestException("User is not linked to a player.");

		if (user.PlayerId != id)
			throw new ForbiddenException("Not allowed to access another player's profile.");

		PlayerEntity? player = await _dbContext.Players.FirstOrDefaultAsync(p => p.Id == id);
		if (player == null)
			throw new NotFoundException($"Player with ID '{id}' could not be found.");

		if (player.BanType != Entities.Enums.BanType.NotBanned)
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
		player.VerticalSync = editPlayerProfile.VerticalSync.ToDomain();

		await _dbContext.SaveChangesAsync();
	}
}
