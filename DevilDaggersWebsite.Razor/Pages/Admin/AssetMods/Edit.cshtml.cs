using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public EditModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			AuthorSelectList = dbContext.Players
				.OrderBy(p => p.Username).
				Select(p => new SelectListItem
				{
					Value = p.Id.ToString(CultureInfo.InvariantCulture),
					Text = p.Username,
				})
				.ToList();
		}

		public List<SelectListItem> AuthorSelectList { get; } = null!;

		[BindProperty]
		public AssetMod AssetMod { get; set; } = null!;

		[BindProperty]
		public List<int> AuthorIds { get; set; } = null!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			AssetMod = await _dbContext.AssetMods.Include(am => am.PlayerAssetMods).FirstOrDefaultAsync(m => m.Id == id);

			if (AssetMod == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("AssetMod.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			_dbContext.Attach(AssetMod).State = EntityState.Modified;

			try
			{
				IQueryable<PlayerAssetMod> alreadyExistingPlayerAssetMods = _dbContext.PlayerAssetMods.Where(pam => pam.AssetModId == AssetMod.Id);
				_dbContext.PlayerAssetMods.RemoveRange(alreadyExistingPlayerAssetMods);

				AssetMod.PlayerAssetMods = AuthorIds.Select(id => new PlayerAssetMod { AssetModId = AssetMod.Id, PlayerId = id }).ToList();

				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) when (!AssetModExists(AssetMod.Id))
			{
				return NotFound();
			}

			return RedirectToPage("./Index");
		}

		private bool AssetModExists(int id) => _dbContext.AssetMods.Any(e => e.Id == id);
	}
}