using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.Donations
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public CreateModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			CurrencyList = RazorUtils.EnumToSelectList<Currency>();
		}

		public List<SelectListItem> CurrencyList { get; }

		[BindProperty]
		public Donation Donation { get; set; } = null!;

		public IActionResult OnGet() => Page();

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			_dbContext.Donations.Add(Donation);
			await _dbContext.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}
