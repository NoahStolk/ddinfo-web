using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityCreateOrEditPageModel<TEntity> : AbstractAdminEntityPageModel<TEntity>
	   where TEntity : class, IEntity
	{
		public AdminEntityCreateOrEditPageModel(ApplicationDbContext dbContext)
			: base(dbContext)
		{
			CurrencyList = RazorUtils.EnumToSelectList<Currency>();
			PlayerList = DbContext.Players.Select(p => new SelectListItem(p.PlayerName, p.Id.ToString())).ToList();
		}

		public List<SelectListItem> CurrencyList { get; }
		public List<SelectListItem> PlayerList { get; }

		public int? Id { get; private set; }

		[BindProperty]
		public TEntity Entity { get; set; } = null!;

		public bool IsEditing => Id.HasValue;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			Id = id;

			Entity = await DbSet.FirstOrDefaultAsync(m => m.Id == id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

			if (!ModelState.IsValid)
				return Page();

			Id = id;

			if (IsEditing)
			{
				DbContext.Attach(Entity).State = EntityState.Modified;

				try
				{
					await DbContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException) when (!DbSet.Any(e => e.Id == Entity.Id))
				{
					return NotFound();
				}
			}
			else
			{
				DbSet.Add(Entity);
				await DbContext.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
