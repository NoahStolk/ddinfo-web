using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.AssetMods
{
	[Authorize]
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public DeleteModel(ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public AssetMod AssetMod { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			AssetMod = await _context.AssetMods.FirstOrDefaultAsync(m => m.Id == id);

			if (AssetMod == null)
			{
				return NotFound();
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			AssetMod = await _context.AssetMods.FindAsync(id);

			if (AssetMod != null)
			{
				_context.AssetMods.Remove(AssetMod);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}