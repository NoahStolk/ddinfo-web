using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.AssetMods
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public EditModel(ApplicationDbContext context)
		{
			this.context = context;

			AuthorSelectList = context.Players
				.OrderBy(p => p.Username).
				Select(p => new SelectListItem
				{
					Value = p.Id.ToString(CultureInfo.InvariantCulture),
					Text = p.Username,
				})
				.ToList();
		}

		public List<SelectListItem> AuthorSelectList { get; }

		[BindProperty]
		public AssetMod AssetMod { get; set; }

		[BindProperty]
		public List<int> AuthorIds { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			AssetMod = await context.AssetMods.Include(am => am.PlayerAssetMods).FirstOrDefaultAsync(m => m.Id == id);

			if (AssetMod == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("AssetMod.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			context.Attach(AssetMod).State = EntityState.Modified;

			try
			{
				IQueryable<PlayerAssetMod> alreadyExistingPlayerAssetMods = context.PlayerAssetMods.Where(pam => pam.AssetModId == AssetMod.Id);
				context.PlayerAssetMods.RemoveRange(alreadyExistingPlayerAssetMods);

				AssetMod.PlayerAssetMods = AuthorIds.Select(id => new PlayerAssetMod { AssetModId = AssetMod.Id, PlayerId = id }).ToList();

				await context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AssetModExists(AssetMod.Id))
					return NotFound();
				else
					throw;
			}

			return RedirectToPage("./Index");
		}

		private bool AssetModExists(int id) => context.AssetMods.Any(e => e.Id == id);
	}
}