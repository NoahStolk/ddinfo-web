using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Donations
{
	public class DetailsModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public DetailsModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

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
	}
}