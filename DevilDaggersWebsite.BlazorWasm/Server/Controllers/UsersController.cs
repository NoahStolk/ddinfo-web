using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Users;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Api
{
	[Route("api/users/admin")]
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
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<Page<GetUser>> GetUsers([Range(0, 1000)] int pageIndex = 0, [Range(5, 50)] int pageSize = 25)
		{
			var users = _dbContext.Users
				.AsNoTracking()
				.Select(u => new { u.Id, u.UserName })
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

			return new Page<GetUser>
			{
				Results = users.ConvertAll(u => new GetUser
				{
					Id = u.Id,
					UserName = u.UserName,
					Roles = userRoles.Where(ur => ur.UserId == u.Id).Select(ur => roles.Find(r => r.Id == ur.RoleId)?.Name ?? string.Empty).ToList(),
				}),
				TotalResults = _dbContext.Users.Count(),
			};
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
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
