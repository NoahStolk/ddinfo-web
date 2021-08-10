using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons.AuditLog;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Admin
{
	[Route("api/admin/users")]
	[Authorize(Roles = Roles.Admin)]
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
			string? sortBy = null,
			bool ascending = false)
		{
			var usersQuery = _dbContext.Users
				.AsNoTracking()
				.Select(u => new { u.Id, u.UserName });

			if (sortBy != null)
				usersQuery = usersQuery.OrderByMember(sortBy, ascending);

			var users = usersQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			List<IdentityUserRole<string>> userRoles = _dbContext.UserRoles
				.AsNoTracking()
				.ToList();

			var roles = _dbContext.Roles
				.AsNoTracking()
				.Select(r => new { r.Id, r.Name })
				.ToList();

			List<(string RoleId, string RoleName, List<string> UserIds)> usersByRole = new();
			foreach (var role in roles)
				usersByRole.Add((role.Id, role.Name, new()));

			foreach ((string roleId, string roleName, List<string> userIds) in usersByRole)
			{
				IEnumerable<IdentityUserRole<string>> userRolesWithRole = userRoles.Where(ur => ur.RoleId == roleId);
				userIds.AddRange(userRolesWithRole.Select(ur => ur.UserId));
			}

			return new Page<GetUser>
			{
				Results = users.ConvertAll(u => new GetUser
				{
					Id = u.Id,
					UserName = u.UserName,
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
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> AddUserToRoleById(string id, string role)
		{
			ApplicationUser? user = _dbContext.Users
				.FirstOrDefault(t => t.Id == id);
			if (user == null)
				return NotFound();

			//await _userManager.AddToRoleAsync(user, role);

			return Ok();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteUserById(string id)
		{
			ApplicationUser? user = _dbContext.Users
				.FirstOrDefault(t => t.Id == id);
			if (user == null)
				return NotFound();

			_dbContext.Users.Remove(user);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(user, User, user.Id);

			return Ok();
		}
	}
}
