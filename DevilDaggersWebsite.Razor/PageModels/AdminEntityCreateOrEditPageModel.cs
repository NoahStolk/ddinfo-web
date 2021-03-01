using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityCreateOrEditPageModel<TEntity> : PageModel
	   where TEntity : class, IEntity
	{
		private readonly ApplicationDbContext _dbContext;

		public AdminEntityCreateOrEditPageModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			DbSet = ((Array.Find(typeof(ApplicationDbContext).GetProperties(), pi => pi.PropertyType == typeof(DbSet<TEntity>)) ?? throw new("Could not retrieve DbSet of TEntity.")).GetValue(_dbContext) as DbSet<TEntity>)!;
		}

		public int? Id { get; private set; }

		[BindProperty]
		public TEntity Entity { get; set; } = null!;

		public DbSet<TEntity> DbSet { get; }

		public bool IsEditing => Id.HasValue;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			Id = id;

			Entity = await DbSet.FirstOrDefaultAsync(m => m.Id == id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (!ModelState.IsValid)
				return Page();

			Id = id;

			if (IsEditing)
			{
				_dbContext.Attach(Entity).State = EntityState.Modified;

				try
				{
					await _dbContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException) when (!DbSet.Any(e => e.Id == Entity.Id))
				{
					return NotFound();
				}
			}
			else
			{
				DbSet.Add(Entity);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
