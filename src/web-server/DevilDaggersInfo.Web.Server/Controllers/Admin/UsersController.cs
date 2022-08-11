using DevilDaggersInfo.Api.Admin.Users;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly UserRepository _userRepository;
	private readonly IUserService _userService;
	private readonly ILogger<UsersController> _logger;

	public UsersController(ApplicationDbContext dbContext, UserRepository userRepository, IUserService userService, ILogger<UsersController> logger)
	{
		_dbContext = dbContext;
		_userRepository = userRepository;
		_userService = userService;
		_logger = logger;
	}

	[HttpGet]
	[Authorize(Roles = Roles.Players)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetUser>>> GetUsers()
		=> await _userRepository.GetUsersAsync();

	[HttpGet("{id}")]
	[Authorize(Roles = Roles.Players)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetUser>> GetUserById(int id)
		=> await _userRepository.GetUserAsync(id);

	[HttpPatch("{id}/toggle-role")]
	[Authorize(Roles = Roles.Admin)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> ToggleRole(int id, ToggleRole toggleRole)
	{
		string roleName = toggleRole.RoleName;

		if (!_dbContext.Roles.Any(r => r.Name == roleName))
			return NotFound();

		UserEntity? user = _dbContext.Users
			.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
			.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound();

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

		return Ok();
	}

	[HttpPut("{id}/assign-player")]
	[Authorize(Roles = Roles.Players)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> AssignPlayer(int id, AssignPlayer assignPlayer)
	{
		UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound($"User with ID '{id}' was not found.");

		var player = _dbContext.Players.Select(p => new { p.Id, p.PlayerName }).FirstOrDefault(p => p.Id == assignPlayer.PlayerId);
		if (player == null)
			return NotFound($"Player with ID '{assignPlayer.PlayerId}' was not found.");

		if (_dbContext.Users.Any(u => u.PlayerId == player.Id))
			return BadRequest($"Player with ID '{player.Id}' is already linked.");

		user.PlayerId = player.Id;
		await _dbContext.SaveChangesAsync();

		_logger.LogWarning("Player '{playerName}' ({playerId}) was linked to user '{userName}' ({userId}).", player.PlayerName, player.Id, user.Name, user.Id);

		return Ok();
	}

	[HttpPut("{id}/reset-password")]
	[Authorize(Roles = Roles.Admin)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> ResetPasswordForUserById(int id, ResetPassword resetPassword)
	{
		UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound();

		_userService.UpdatePassword(id, resetPassword.NewPassword);
		await _dbContext.SaveChangesAsync();

		_logger.LogWarning("Password was reset for user '{user}'.", user.Name);

		return Ok();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = Roles.Admin)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteUserById(int id)
	{
		UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound();

		_dbContext.Users.Remove(user);
		await _dbContext.SaveChangesAsync();

		return Ok();
	}
}
