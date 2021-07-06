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
using System.Linq;

namespace DevilDaggersWebsite.Api
{
	[Route("api/users")]
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
		//[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult<List<GetUser>> GetUsers()
		{
			List<ApplicationUser> users = _dbContext.Users
				.AsNoTracking()
				.ToList();

			List<IdentityUserRole<string>> userRoles = _dbContext.UserRoles
				.AsNoTracking()
				.ToList();

			List<IdentityRole> roles = _dbContext.Roles
				.AsNoTracking()
				.ToList();

			return users.ConvertAll(u => new GetUser
			{
				Id = u.Id,
				AccessFailedCount = u.AccessFailedCount,
				ConcurrencyStamp = u.ConcurrencyStamp,
				Email = u.Email,
				EmailConfirmed = u.EmailConfirmed,
				LockoutEnabled = u.LockoutEnabled,
				LockoutEnd = u.LockoutEnd,
				NormalizedEmail = u.NormalizedEmail,
				NormalizedUserName = u.NormalizedUserName,
				PasswordHash = u.PasswordHash,
				PhoneNumber = u.PhoneNumber,
				PhoneNumberConfirmed = u.PhoneNumberConfirmed,
				SecurityStamp = u.SecurityStamp,
				TwoFactorEnabled = u.TwoFactorEnabled,
				UserName = u.UserName,
				Roles = userRoles.Where(ur => ur.UserId == u.Id).Select(ur => roles.FirstOrDefault(r => r.Id == ur.RoleId)?.Name ?? string.Empty).ToList(),
			});
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult DeleteUser(string id)
		{
			ApplicationUser? user = _dbContext.Users
				.FirstOrDefault(t => t.Id == id);
			if (user == null)
				return NotFound();

			_dbContext.Users.Remove(user);
			_dbContext.SaveChanges();

			return Ok();
		}
	}
}
