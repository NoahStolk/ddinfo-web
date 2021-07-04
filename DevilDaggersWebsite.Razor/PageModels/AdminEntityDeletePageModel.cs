using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityDeletePageModel<TEntity> : AbstractAdminEntityPageModel<TEntity>
		where TEntity : class, IEntity
	{
		public AdminEntityDeletePageModel(ApplicationDbContext dbContext, IWebHostEnvironment environment, DiscordLogger discordLogger)
			: base(dbContext, environment, discordLogger)
		{
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

				//StringBuilder auditLogger = new($"`DELETE` by `{GetIdentity()}` for `{typeof(TEntity).Name}` `{id}`\n");
				//LogDelete(auditLogger, Entity.Populate().Log());
				//await DiscordLogger.TryLog(Channel.MonitoringAuditLog, auditLogger.ToString());
			}

			return RedirectToPage("./Index");
		}
	}
}
