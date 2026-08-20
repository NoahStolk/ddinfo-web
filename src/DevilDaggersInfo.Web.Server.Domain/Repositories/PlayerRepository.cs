using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Repositories;

public sealed class PlayerRepository(ApplicationDbContext dbContext)
{
	public async Task<List<PlayerForLeaderboard>> GetPlayersForLeaderboardAsync()
	{
		return await dbContext.Players
			.AsNoTracking()
			.Select(p => new PlayerForLeaderboard
			{
				Id = p.Id,
				BanType = p.BanType,
				BanDescription = p.BanDescription,
				BanResponsibleId = p.BanResponsibleId,
				CountryCode = p.CountryCode,
			})
			.ToListAsync();
	}

	public async Task<List<PlayerForSettings>> GetPlayersForSettingsAsync()
	{
		List<PlayerEntity> players = await dbContext.Players
			.AsNoTracking()
			.Where(p => p.BanType == BanType.NotBanned && !p.HideSettings)
			.ToListAsync();

		// Note; cannot evaluate HasVisibleSettings() against database (IQueryable).
		return [.. players.Where(p => p.HasVisibleSettings()).Select(PlayerForSettings.FromEntity)];
	}

	public async Task<Player> GetPlayerAsync(int id)
	{
		PlayerEntity? player = await dbContext.Players
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);
		if (player == null)
			throw new NotFoundException();

		bool isPublicDonor = !player.HideDonations && await dbContext.Donations.AnyAsync(d => d.PlayerId == id && !d.IsRefunded && d.ConvertedEuroCentsReceived > 0);
		return Player.FromEntity(player, isPublicDonor);
	}

	public async Task<List<PlayerCommonName>> GetCommonNamesAsync()
	{
		// ! LINQ filters out null values.
		return await dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.CommonName })
			.Where(p => p.CommonName != null)
			.Select(p => new PlayerCommonName
			{
				Id = p.Id,
				CommonName = p.CommonName!,
			})
			.ToListAsync();
	}

	public async Task<string?> GetPlayerCountryCodeAsync(int id)
	{
		var player = await dbContext.Players.AsNoTracking().Select(p => new { p.Id, p.CountryCode }).FirstOrDefaultAsync(p => p.Id == id);
		return player?.CountryCode;
	}
}
