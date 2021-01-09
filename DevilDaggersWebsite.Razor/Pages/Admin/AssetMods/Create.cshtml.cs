using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public CreateModel(ApplicationDbContext context)
		{
			_context = context;

			AuthorSelectList = context.Players
				.OrderBy(p => p.Username)
				.Select(p => new SelectListItem
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

		public IActionResult OnGet() => Page();

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("AssetMod.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			AssetMod.PlayerAssetMods = AuthorIds.Select(id => new PlayerAssetMod { AssetModId = AssetMod.Id, PlayerId = id }).ToList();

			_context.AssetMods.Add(AssetMod);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}