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
			IsDonationsMaintainer = IsInRole(u, Roles.Donations),
			IsModsMaintainer = IsInRole(u, Roles.Mods),
			IsPlayersMaintainer = IsInRole(u, Roles.Players),
			IsSpawnsetsMaintainer = IsInRole(u, Roles.Spawnsets),
		});

		static bool IsInRole(UserEntity user, string roleName)
			=> user.UserRoles?.Any(ur => ur.Role?.Name == roleName) == true;
	}

	[HttpPatch("{id}/assign-role")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult AssignRole(int id, string roleName)
	{
		UserEntity? user = _dbContext.Users
			.AsNoTracking()
			.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
			.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound();

		if (user.UserRoles!.Any(ur => ur.Role?.Name == roleName))
			return BadRequest("User is already in this role.");

		_dbContext.UserRoles.Add(new UserRoleEntity
		{
			RoleName = roleName,
			UserId = id,
		});
		_dbContext.SaveChanges();

		// TODO: Log role assignment in audit log.
		return Ok();
	}

	[HttpPatch("{id}/revoke-role")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult RevokeRole(int id, string roleName)
	{
		UserEntity? user = _dbContext.Users
			.Include(u => u.UserRoles)
			.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound();

		UserRoleEntity? userRole = user.UserRoles!.Find(ur => ur.RoleName == roleName);
		if (userRole == null)
			return BadRequest("User is not in this role.");

		_dbContext.UserRoles.Remove(userRole);
		_dbContext.SaveChanges();

		// TODO: Log role removal in audit log.
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
