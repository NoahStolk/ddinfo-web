using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public class AdminEntityCreateOrEditPageModel<TEntity, TAdminDto> : AbstractAdminEntityPageModel<TEntity>
		where TEntity : class, IAdminUpdatableEntity<TAdminDto>, new()
		where TAdminDto : class
	{
		private TEntity? _entity;

		public AdminEntityCreateOrEditPageModel(ApplicationDbContext dbContext)
			: base(dbContext)
		{
			CurrencyList = RazorUtils.EnumToSelectList<Currency>();
			PlayerList = DbContext.Players.Select(p => new SelectListItem(p.PlayerName, p.Id.ToString())).ToList();
			CategoryList = RazorUtils.EnumToSelectList<CustomLeaderboardCategory>();
			SpawnsetFileList = DbContext.SpawnsetFiles.Select(sf => new SelectListItem(sf.Name, sf.Id.ToString())).ToList();
		}

		public List<SelectListItem> CurrencyList { get; }
		public List<SelectListItem> PlayerList { get; }
		public List<SelectListItem> CategoryList { get; }
		public List<SelectListItem> SpawnsetFileList { get; }

		public int? Id { get; private set; }

		[BindProperty]
		public TAdminDto? AdminDto { get; set; }

		public bool IsEditing => Id.HasValue;

		public IActionResult OnGet(int? id)
		{
			Id = id;

			_entity = DbSet.FirstOrDefault(m => m.Id == id) ?? (TEntity?)new();

			AdminDto = _entity.Populate();

			return Page();
		}

		public IActionResult OnPost(int? id)
		{
			IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

			if (!ModelState.IsValid)
				return Page();

			if (AdminDto == null)
				throw new($"{nameof(AdminDto)} should not be null on POST.");

			Id = id;

			if (IsEditing)
			{
				_entity = DbSet.FirstOrDefault(m => m.Id == id);
				if (_entity == null)
					return NotFound();

				DbContext.Attach(_entity).State = EntityState.Modified;

				try
				{
					_entity.Edit(DbContext, AdminDto);
					DbContext.SaveChanges();
				}
				catch (DbUpdateConcurrencyException) when (!DbSet.Any(e => e.Id == _entity.Id))
				{
					return NotFound();
				}
			}
			else
			{
				_entity = new();
				_entity.Create(DbContext, AdminDto);
				DbContext.SaveChanges();
			}

			return RedirectToPage("./Index");
		}
	}
}
