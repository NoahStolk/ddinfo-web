using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityDeletePageModel<TEntity> : AbstractAdminEntityPageModel<TEntity>
		where TEntity : class, IEntity
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

				await DiscordLogger.Instance.TryLog(Channel.AuditLogMonitoring, _env.EnvironmentName, $"`DELETE by {this.GetIdentity()} for {typeof(TEntity).Name} {id}`");
			}

			return RedirectToPage("./Index");
		}
	}
}
