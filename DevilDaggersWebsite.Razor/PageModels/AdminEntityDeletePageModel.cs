﻿using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityDeletePageModel<TEntity, TAdminDto> : AbstractAdminEntityPageModel<TEntity>
		where TEntity : class, IAdminUpdatableEntity<TAdminDto>, new()
		where TAdminDto : class, IAdminDto
	{
		private readonly IWebHostEnvironment _env;

		public AdminEntityDeletePageModel(IWebHostEnvironment env, ApplicationDbContext dbContext)
			: base(dbContext)
		{
			_env = env;
		}

		[BindProperty]
		public TEntity Entity { get; set; } = null!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Entity = await DbSet.FirstOrDefaultAsync(m => m.Id == id);
			if (Entity == null)
				return NotFound();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Entity = await DbSet.FindAsync(id);
			if (Entity != null)
			{
				DbSet.Remove(Entity);
				await DbContext.SaveChangesAsync();

				StringBuilder auditLogger = new($"`DELETE` by `{this.GetIdentity()}` for `{typeof(TEntity).Name}` `{id}`\n");
				LogDelete(auditLogger, Entity.Populate().Log());
				await DiscordLogger.TryLog(Channel.MonitoringAuditLog, _env.EnvironmentName, auditLogger.ToString());
			}

			return RedirectToPage("./Index");
		}
	}
}
