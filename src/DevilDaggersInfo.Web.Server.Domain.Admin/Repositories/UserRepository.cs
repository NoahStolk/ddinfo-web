using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Users;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public class UserRepository
{
	private readonly ApplicationDbContext _dbContext;

	public UserRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Page<GetUser>> GetUsersAsync(string? filter, int pageIndex, int pageSize, UserSorting? sortBy, bool ascending)
	{
		// ! Navigation property.
		IQueryable<UserEntity> usersQuery = _dbContext.Users
			.AsNoTracking()
			.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
			.Include(u => u.Player)
			.OrderBy(u => u.Id);

		if (!string.IsNullOrWhiteSpace(filter))
		{
			filter = filter.Trim();

			// ! Navigation property.
			usersQuery = usersQuery.Where(u =>
				u.Name.Contains(filter) ||
				u.Player!.CommonName != null && u.Player!.CommonName.Contains(filter) ||
				u.Player!.PlayerName.Contains(filter) ||
				u.PlayerId.HasValue && u.PlayerId.Value.ToString().Contains(filter));
		}

		// ! Navigation property.
		usersQuery = sortBy switch
		{
			UserSorting.Name => usersQuery.OrderBy(u => u.Name, ascending).ThenBy(u => u.Id),
			UserSorting.PlayerId => usersQuery.OrderBy(u => u.PlayerId, ascending).ThenBy(u => u.Id),
			UserSorting.PlayerName => usersQuery.OrderBy(u => u.Player!.PlayerName, ascending).ThenBy(u => u.Id),
			UserSorting.RegistrationDate => usersQuery.OrderBy(u => u.DateRegistered, ascending).ThenBy(u => u.Id),
			UserSorting.AdminRole => usersQuery.OrderBy(u => u.UserRoles!.Any(ur => ur.Role!.Name == "Admin"), ascending).ThenBy(u => u.Id),
			UserSorting.CustomLeaderboardsRole => usersQuery.OrderBy(u => u.UserRoles!.Any(ur => ur.Role!.Name == "CustomLeaderboards"), ascending).ThenBy(u => u.Id),
			UserSorting.ModsRole => usersQuery.OrderBy(u => u.UserRoles!.Any(ur => ur.Role!.Name == "Mods"), ascending).ThenBy(u => u.Id),
			UserSorting.PlayersRole => usersQuery.OrderBy(u => u.UserRoles!.Any(ur => ur.Role!.Name == "Players"), ascending).ThenBy(u => u.Id),
			UserSorting.SpawnsetsRole => usersQuery.OrderBy(u => u.UserRoles!.Any(ur => ur.Role!.Name == "Spawnsets"), ascending).ThenBy(u => u.Id),
			_ => usersQuery.OrderBy(u => u.Id, ascending),
		};

		List<UserEntity> users = await usersQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return new Page<GetUser>
		{
			Results = users.ConvertAll(u => u.ToAdminApi()),
			TotalResults = await usersQuery.CountAsync(),
		};
	}

	public async Task<GetUser> GetUserAsync(int id)
	{
		// ! Navigation property.
		UserEntity? user = await _dbContext.Users
			.AsNoTracking()
			.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
			.Include(u => u.Player)
			.FirstOrDefaultAsync(u => u.Id == id);

		if (user == null)
			throw new NotFoundException();

		return user.ToAdminApi();
	}
}
