using DevilDaggersInfo.Api.Admin.Users;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class UserService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly UserManager _userManager;
	private readonly ILogger<UserService> _logger;

	public UserService(ApplicationDbContext dbContext, UserManager userManager, ILogger<UserService> logger)
	{
		_dbContext = dbContext;
		_userManager = userManager;
		_logger = logger;
	}

	public async Task ToggleRoleAsync(int id, ToggleRole toggleRole)
	{
		string roleName = toggleRole.RoleName;

		if (!_dbContext.Roles.Any(r => r.Name == roleName))
			throw new NotFoundException();

		// ! Navigation property.
		UserEntity? user = _dbContext.Users
			.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
			.FirstOrDefault(u => u.Id == id);
		if (user == null)
			throw new NotFoundException();

		// ! Navigation property.
		UserRoleEntity? userRole = user.UserRoles!.Find(ur => ur.RoleName == roleName);
		if (userRole != null)
		{
			_dbContext.UserRoles.Remove(userRole);
		}
		else
		{
			_dbContext.UserRoles.Add(new UserRoleEntity
			{
				RoleName = roleName,
				UserId = id,
			});
		}

		await _dbContext.SaveChangesAsync();
	}

	public async Task AssignPlayerAsync(int id, AssignPlayer assignPlayer)
	{
		UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
		if (user == null)
			throw new NotFoundException($"User with ID '{id}' was not found.");

		if (assignPlayer.PlayerId == 0)
		{
			user.PlayerId = null;
			await _dbContext.SaveChangesAsync();

			_logger.LogInformation("User '{userName}' ({userId}) has been unlinked.", user.Name, user.Id);
		}
		else
		{
			var player = await _dbContext.Players.Select(p => new { p.Id, p.PlayerName }).FirstOrDefaultAsync(p => p.Id == assignPlayer.PlayerId);
			if (player == null)
				throw new NotFoundException($"Player with ID '{assignPlayer.PlayerId}' was not found.");

			if (_dbContext.Users.Any(u => u.PlayerId == player.Id))
				throw new AdminDomainException($"Player with ID '{player.Id}' is already linked.");

			user.PlayerId = player.Id;
			await _dbContext.SaveChangesAsync();

			_logger.LogInformation("Player '{playerName}' ({playerId}) has been linked to user '{userName}' ({userId}).", player.PlayerName, player.Id, user.Name, user.Id);
		}
	}

	public async Task ResetPasswordForUser(int id, ResetPassword resetPassword)
	{
		UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
		if (user == null)
			throw new NotFoundException();

		await _userManager.UpdatePasswordAsync(id, resetPassword.NewPassword);

		_logger.LogInformation("Password was reset for user '{user}'.", user.Name);
	}

	public async Task DeleteUser(int id)
	{
		UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
		if (user == null)
			throw new NotFoundException();

		_dbContext.Users.Remove(user);
		await _dbContext.SaveChangesAsync();
	}
}
