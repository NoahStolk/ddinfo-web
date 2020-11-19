using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Donations
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public DeleteModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
		public Donation Donation { get; set; } = null!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Donation = await _dbContext.Donations.FirstOrDefaultAsync(m => m.Id == id);

			if (Donation == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Donation = await _dbContext.Donations.FindAsync(id);

			if (Donation != null)
			{
				_dbContext.Donations.Remove(Donation);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}