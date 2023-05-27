using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Players;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public class PlayerRepository
{
	private readonly ApplicationDbContext _dbContext;

	public PlayerRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Page<GetPlayerForOverview>> GetPlayersAsync(int pageIndex, int pageSize, PlayerSorting? sortBy, bool ascending)
	{
		IQueryable<PlayerEntity> playersQuery = _dbContext.Players.AsNoTracking();

		playersQuery = sortBy switch
		{
			PlayerSorting.BanDescription => playersQuery.OrderBy(p => p.BanDescription, ascending).ThenBy(p => p.Id),
			PlayerSorting.BanResponsibleId => playersQuery.OrderBy(p => p.BanResponsibleId, ascending).ThenBy(p => p.Id),
			PlayerSorting.BanType => playersQuery.OrderBy(p => p.BanType, ascending).ThenBy(p => p.Id),
			PlayerSorting.CommonName => playersQuery.OrderBy(p => p.CommonName, ascending).ThenBy(p => p.Id),
			PlayerSorting.CountryCode => playersQuery.OrderBy(p => p.CountryCode, ascending).ThenBy(p => p.Id),
			PlayerSorting.DiscordUserId => playersQuery.OrderBy(p => p.DiscordUserId, ascending).ThenBy(p => p.Id),
			PlayerSorting.Dpi => playersQuery.OrderBy(p => p.Dpi, ascending).ThenBy(p => p.Id),
			PlayerSorting.Fov => playersQuery.OrderBy(p => p.Fov, ascending).ThenBy(p => p.Id),
			PlayerSorting.Gamma => playersQuery.OrderBy(p => p.Gamma, ascending).ThenBy(p => p.Id),
			PlayerSorting.HasFlashHandEnabled => playersQuery.OrderBy(p => p.HasFlashHandEnabled, ascending).ThenBy(p => p.Id),
			PlayerSorting.HideDonations => playersQuery.OrderBy(p => p.HideDonations, ascending).ThenBy(p => p.Id),
			PlayerSorting.HidePastUsernames => playersQuery.OrderBy(p => p.HidePastUsernames, ascending).ThenBy(p => p.Id),
			PlayerSorting.HideSettings => playersQuery.OrderBy(p => p.HideSettings, ascending).ThenBy(p => p.Id),
			PlayerSorting.InGameSens => playersQuery.OrderBy(p => p.InGameSens, ascending).ThenBy(p => p.Id),
			PlayerSorting.IsBannedFromDdcl => playersQuery.OrderBy(p => p.IsBannedFromDdcl, ascending).ThenBy(p => p.Id),
			PlayerSorting.IsRightHanded => playersQuery.OrderBy(p => p.IsRightHanded, ascending).ThenBy(p => p.Id),
			PlayerSorting.PlayerName => playersQuery.OrderBy(p => p.PlayerName, ascending).ThenBy(p => p.Id),
			PlayerSorting.UsesLegacyAudio => playersQuery.OrderBy(p => p.UsesLegacyAudio, ascending).ThenBy(p => p.Id),
			PlayerSorting.UsesHrtf => playersQuery.OrderBy(p => p.UsesHrtf, ascending).ThenBy(p => p.Id),
			PlayerSorting.UsesInvertY => playersQuery.OrderBy(p => p.UsesInvertY, ascending).ThenBy(p => p.Id),
			PlayerSorting.VerticalSync => playersQuery.OrderBy(p => p.VerticalSync, ascending).ThenBy(p => p.Id),
			_ => playersQuery.OrderBy(p => p.Id, ascending),
		};

		List<PlayerEntity> players = await playersQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return new Page<GetPlayerForOverview>
		{
			Results = players.ConvertAll(p => p.ToAdminApiOverview()),
			TotalResults = _dbContext.Players.Count(),
		};
	}

	public async Task<List<GetPlayerName>> GetPlayerNamesAsync()
	{
		var players = await _dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.PlayerName })
			.ToListAsync();

		return players.ConvertAll(p => new GetPlayerName
		{
			Id = p.Id,
			PlayerName = p.PlayerName,
		});
	}

	public async Task<GetPlayer> GetPlayerAsync(int id)
	{
		PlayerEntity? player = await _dbContext.Players
			.AsNoTracking()
			.Include(p => p.PlayerMods)
			.FirstOrDefaultAsync(p => p.Id == id);
		if (player == null)
			throw new NotFoundException();

		return player.ToAdminApi();
	}
}
