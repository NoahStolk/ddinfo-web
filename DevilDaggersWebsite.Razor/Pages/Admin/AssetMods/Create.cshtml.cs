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
		private readonly ApplicationDbContext _dbContext;

		public CreateModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			AuthorSelectList = _dbContext.Players
				.OrderBy(p => p.PlayerName)
				.Select(p => new SelectListItem
				{
					Value = p.Id.ToString(CultureInfo.InvariantCulture),
					Text = p.PlayerName,
				})
				.ToList();
		}

		public List<SelectListItem> AuthorSelectList { get; }

		[BindProperty]
		public AssetMod AssetMod { get; set; } = null!;

		[BindProperty]
		public List<int> AuthorIds { get; set; } = new();

		public IActionResult OnGet() => Page();

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("AssetMod.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			AssetMod.PlayerAssetMods = AuthorIds.ConvertAll(id => new PlayerAssetMod { AssetModId = AssetMod.Id, PlayerId = id });

			_dbContext.AssetMods.Add(AssetMod);
			await _dbContext.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}
