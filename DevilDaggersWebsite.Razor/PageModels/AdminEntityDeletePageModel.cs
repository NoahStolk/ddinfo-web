using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityDeletePageModel<TEntity> : PageModel
	   where TEntity : class, IEntity
	{
		private readonly ApplicationDbContext _dbContext;

		public AdminEntityDeletePageModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			DbSet = ((Array.Find(typeof(ApplicationDbContext).GetProperties(), pi => pi.PropertyType == typeof(DbSet<TEntity>)) ?? throw new("Could not retrieve DbSet of TEntity.")).GetValue(_dbContext) as DbSet<TEntity>)!;

			EntityProperties = typeof(TEntity).GetProperties().Where(pi => !pi.PropertyType.IsGenericType).ToArray();
		}

		[BindProperty]
		public TEntity Entity { get; set; } = null!;

		public DbSet<TEntity> DbSet { get; }

		public PropertyInfo[] EntityProperties { get; } = null!;

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
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
