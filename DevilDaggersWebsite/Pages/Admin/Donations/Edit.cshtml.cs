using DevilDaggersWebsite.Code.Utils;
using DevilDaggersWebsite.Core.Entities;
using DevilDaggersWebsite.Core.Enumerators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Donations
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public EditModel(ApplicationDbContext context)
		{
			_context = context;

			CurrencyList = RazorUtils.EnumToSelectList<Currency>();
		}

		public List<SelectListItem> CurrencyList { get; }

		[BindProperty]
		public Donation Donation { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Donation = await _context.Donations.FirstOrDefaultAsync(m => m.Id == id);

			if (Donation == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			_context.Attach(Donation).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DonationExists(Donation.Id))
					return NotFound();
				else
					throw;
			}

			return RedirectToPage("./Index");
		}

		private bool DonationExists(int id)
			=> _context.Donations.Any(e => e.Id == id);
	}
}