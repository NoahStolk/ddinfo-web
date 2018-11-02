using DevilDaggersWebsite.Models;
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
			Donators = UserHelper.Donators
				.OrderByDescending(d => d.Amount)
				.ThenBy(d => d.CurrencySymbol)
				.ThenBy(d => d.Username)
				.ToList();
		}
	}
}