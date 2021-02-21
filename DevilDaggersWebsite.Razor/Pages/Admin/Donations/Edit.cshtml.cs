using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.Donations
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public EditModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			CurrencyList = RazorUtils.EnumToSelectList<Currency>();
		}

		public List<SelectListItem> CurrencyList { get; }

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

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			_dbContext.Attach(Donation).State = EntityState.Modified;

			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) when (!DonationExists(Donation.Id))
			{
				return NotFound();
			}

			return RedirectToPage("./Index");
		}

		private bool DonationExists(int id)
			=> _dbContext.Donations.Any(e => e.Id == id);
	}
}
