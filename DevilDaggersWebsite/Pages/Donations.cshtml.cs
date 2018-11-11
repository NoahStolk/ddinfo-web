using DevilDaggersWebsite.Models.User;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class DonationsModel : PageModel
	{
		public List<Donator> Donators { get; set; }

		public void OnGet()
		{
			Donators = UserUtils.Donators
				.OrderByDescending(d => d.Amount)
				.ThenBy(d => d.CurrencySymbol, new CurrencyComparer())
				.ThenBy(d => d.Username)
				.ToList();
		}
	}
}