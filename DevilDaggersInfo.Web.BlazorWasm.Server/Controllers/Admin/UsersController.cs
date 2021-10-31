using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Users;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Admin;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/users")]
[ApiController]
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
	public ActionResult<Page<GetUser>> GetUsers(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault,
		UserSorting? sortBy = null,
		bool ascending = false)
	{
		var usersQuery = _dbContext.Users
			.AsNoTracking()
			.Select(u => new { u.Id, u.Name });

		usersQuery = sortBy switch
		{
			// TODO: Implement sorting for roles.
			UserSorting.UserName => usersQuery.OrderBy(s => s.Name, ascending),
			_ => usersQuery.OrderBy(s => s.Id, ascending),
		};

		var users = usersQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		List<UserRoleEntity> userRoles = _dbContext.UserRoles
			.AsNoTracking()
			.ToList();

		var roles = _dbContext.Roles
			.AsNoTracking()
			.Select(r => new { r.Id, r.Name })
			.ToList();

		List<(int RoleId, string RoleName, List<int> UserIds)> usersByRole = new();
		foreach (var role in roles)
			usersByRole.Add((role.Id, role.Name, new()));

		foreach ((int roleId, string roleName, List<int> userIds) in usersByRole)
		{
			IEnumerable<UserRoleEntity> userRolesWithRole = userRoles.Where(ur => ur.RoleId == roleId);
			userIds.AddRange(userRolesWithRole.Select(ur => ur.UserId));
		}

		return new Page<GetUser>
		{
			Results = users.ConvertAll(u => new GetUser
			{
				Id = u.Id,
				Name = u.Name,
				IsAdmin = usersByRole.Find(t => t.RoleName == Roles.Admin).UserIds.Contains(u.Id),
				IsCustomLeaderboardsMaintainer = usersByRole.Find(t => t.RoleName == Roles.CustomLeaderboards).UserIds.Contains(u.Id),
				IsDonationsMaintainer = usersByRole.Find(t => t.RoleName == Roles.Donations).UserIds.Contains(u.Id),
				IsModsMaintainer = usersByRole.Find(t => t.RoleName == Roles.Mods).UserIds.Contains(u.Id),
				IsPlayersMaintainer = usersByRole.Find(t => t.RoleName == Roles.Players).UserIds.Contains(u.Id),
				IsSpawnsetsMaintainer = usersByRole.Find(t => t.RoleName == Roles.Spawnsets).UserIds.Contains(u.Id),
			}),
			TotalResults = _dbContext.Users.Count(),
		};
	}

	[HttpPatch("{id}/add-to-role")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult AddUserToRoleById(int userId, int roleId)
	{
		UserEntity? user = _dbContext.Users
			.AsNoTracking()
			.FirstOrDefault(u => u.Id == userId);
		if (user == null)
			return NotFound();

		RoleEntity? role = _dbContext.Roles
			.AsNoTracking()
			.FirstOrDefault(r => r.Id == roleId);
		if (role == null)
			return NotFound();

		UserRoleEntity? userRole = _dbContext.UserRoles
			.AsNoTracking()
			.FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == roleId);
		if (userRole != null)
			return BadRequest("User is already in this role.");

		_dbContext.UserRoles.Add(new UserRoleEntity
		{
			RoleId = roleId,
			UserId = userId,
		});
		_dbContext.SaveChanges();

		// TODO: Log role assignment in audit log.
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteUserById(int id)
	{
		UserEntity? user = _dbContext.Users
			.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound();

		_dbContext.Users.Remove(user);
		_dbContext.SaveChanges();

		await _auditLogger.LogDelete(user, User, user.Id);

		return Ok();
	}
}
