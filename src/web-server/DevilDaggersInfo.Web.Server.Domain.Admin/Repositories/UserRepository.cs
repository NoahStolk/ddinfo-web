using DevilDaggersInfo.Api.Admin.Users;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;

public class UserRepository
{
	private readonly ApplicationDbContext _dbContext;

	public UserRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<GetUser>> GetUsersAsync()
	{
		// ! Navigation property.
		List<UserEntity> users = await _dbContext.Users
			.AsNoTracking()
			.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
			.Include(u => u.Player)
			.OrderBy(u => u.Id)
			.ToListAsync();

		return users.ConvertAll(u => u.ToAdminApi());
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
