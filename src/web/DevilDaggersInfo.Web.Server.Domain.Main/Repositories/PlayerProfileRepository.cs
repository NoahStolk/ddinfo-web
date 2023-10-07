using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Repositories;

public class PlayerProfileRepository
{
	private readonly ApplicationDbContext _dbContext;

	public PlayerProfileRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<ApiSpec.Main.Players.GetPlayerProfile> GetProfileAsync(ClaimsPrincipal claimsPrincipal, int id)
	{
		string? userName = claimsPrincipal.GetName();
		UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == userName);
		if (user == null)
			throw new UnauthorizedException();

		if (!user.PlayerId.HasValue)
			throw new InvalidProfileRequestException("User is not linked to a player.");

		if (user.PlayerId != id)
			throw new ForbiddenException("Not allowed to access another player's profile.");

		PlayerEntity? player = await _dbContext.Players
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);
		if (player == null)
			throw new NotFoundException($"Player with ID '{id}' could not be found.");

		if (player.BanType != Entities.Enums.BanType.NotBanned)
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
			VerticalSync = player.VerticalSync.ToMainApi(),
		};
	}
}
