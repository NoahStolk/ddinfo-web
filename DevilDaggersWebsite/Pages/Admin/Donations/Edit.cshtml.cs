using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Donations
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public EditModel(ApplicationDbContext context)
		{
			this.context = context;

			CurrencyList = ((IEnumerable<Currency>)Enum.GetValues(typeof(Currency))).Select(c => new SelectListItem { Text = c.ToString(), Value = ((int)c).ToString(CultureInfo.InvariantCulture) }).ToList();
		}

		public List<SelectListItem> CurrencyList { get; }

		[BindProperty]
		public Donation Donation { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Donation = await context.Donations.FirstOrDefaultAsync(m => m.Id == id);

			if (Donation == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			context.Attach(Donation).State = EntityState.Modified;

			try
			{
				await context.SaveChangesAsync();
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

		private bool DonationExists(int id) => context.Donations.Any(e => e.Id == id);
	}
}