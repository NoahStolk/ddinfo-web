using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Donations
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public CreateModel(ApplicationDbContext context)
		{
			this.context = context;

			CurrencyList = ((IEnumerable<Currency>)Enum.GetValues(typeof(Currency))).Select(c => new SelectListItem { Text = c.ToString(), Value = ((int)c).ToString(CultureInfo.InvariantCulture) }).ToList();
		}

		public List<SelectListItem> CurrencyList { get; }

		[BindProperty]
		public Donation Donation { get; set; }

		public IActionResult OnGet() => Page();

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			context.Donations.Add(Donation);
			await context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}