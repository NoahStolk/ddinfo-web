using DevilDaggersInfo.Web.Server.Converters.Admin;
using DevilDaggersInfo.Web.Shared.Dto.Admin.Users;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly AuditLogger _auditLogger;
	private readonly IUserService _userService;
	private readonly ILogger<UsersController> _logger;

	public UsersController(ApplicationDbContext dbContext, AuditLogger auditLogger, IUserService userService, ILogger<UsersController> logger)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
		_userService = userService;
		_logger = logger;
	}

	[HttpGet]
	[Authorize(Roles = Roles.Players)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetUser>> GetUsers()
	{
		List<UserEntity> users = _dbContext.Users
			.AsNoTracking()
			.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
			.Include(u => u.Player)
			.OrderBy(u => u.Id)
			.ToList();

		return users.ConvertAll(u => u.ToGetUser());
	}

	[HttpGet("{id}")]
	[Authorize(Roles = Roles.Players)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetUser> GetUserById(int id)
	{
		UserEntity? user = _dbContext.Users
			.AsNoTracking()
			.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
			.Include(u => u.Player)
			.FirstOrDefault(u => u.Id == id);

		if (user == null)
			return NotFound();

		return user.ToGetUser();
	}

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
		bool assigned;
		if (userRole != null)
		{
			assigned = false;
			_dbContext.UserRoles.Remove(userRole);
		}
		else
		{
			assigned = true;
			_dbContext.UserRoles.Add(new UserRoleEntity
			{
				RoleName = roleName,
				UserId = id,
			});
		}

		await _dbContext.SaveChangesAsync();

		if (assigned)
			_auditLogger.LogRoleAssign(user, roleName);
		else
			_auditLogger.LogRoleRevoke(user, roleName);

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

		_auditLogger.LogDelete(user.GetLog(), User, user.Id);

		return Ok();
	}
}
