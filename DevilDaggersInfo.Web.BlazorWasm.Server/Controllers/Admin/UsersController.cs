using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Users;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/users")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class UsersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly AuditLogger _auditLogger;

	public UsersController(ApplicationDbContext dbContext, AuditLogger auditLogger)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetUser>> GetUsers()
	{
		List<UserEntity> users = _dbContext.Users
			.AsNoTracking()
			.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
			.OrderBy(u => u.Id)
			.ToList();

		return users.ConvertAll(u => new GetUser
		{
			Id = u.Id,
			Name = u.Name,
			IsAdmin = IsInRole(u, Roles.Admin),
			IsCustomLeaderboardsMaintainer = IsInRole(u, Roles.CustomLeaderboards),
			IsModsMaintainer = IsInRole(u, Roles.Mods),
			IsPlayersMaintainer = IsInRole(u, Roles.Players),
			IsSpawnsetsMaintainer = IsInRole(u, Roles.Spawnsets),
		});

		static bool IsInRole(UserEntity user, string roleName)
			=> user.UserRoles?.Any(ur => ur.Role?.Name == roleName) == true;
	}

	[HttpPatch("{id}/toggle-role")]
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

		_dbContext.SaveChanges();

		if (assigned)
			await _auditLogger.LogRoleAssign(user, roleName);
		else
			await _auditLogger.LogRoleRevoke(user, roleName);

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteUserById(int id)
	{
		UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound();

		_dbContext.Users.Remove(user);
		_dbContext.SaveChanges();

		await _auditLogger.LogDelete(user, User, user.Id);

		return Ok();
	}
}
