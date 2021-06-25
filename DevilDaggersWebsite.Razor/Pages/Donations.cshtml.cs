using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class DonationsModel : PageModel
	{
		public DonationsModel(ApplicationDbContext dbContext)
		{
			var donations = dbContext.Donations
				.AsNoTracking()
				.Include(d => d.Player)
				.Select(d => new { d.Amount, d.ConvertedEuroCentsReceived, d.Currency, d.IsRefunded, d.PlayerId })
				.Where(d => !d.IsRefunded && d.ConvertedEuroCentsReceived > 0)
				.ToList();

			List<int> donatorIds = donations.ConvertAll(d => d.PlayerId);
			var donators = dbContext.Players
				.AsNoTracking()
				.Select(p => new { p.Id, p.HideDonations, p.PlayerName })
				.Where(p => donatorIds.Contains(p.Id))
				.ToList();

			Donators = donators
				.ToDictionary(
					p => new DonatorModel
					{
						HideDonations = p.HideDonations,
						PlayerId = p.Id,
						PlayerName = p.PlayerName,
					},
					p => donations
						.Where(d => d.PlayerId == p.Id)
						.Select(d => new DonationModel
						{
							Amount = d.Amount,
							ConvertedEuroCentsReceived = d.ConvertedEuroCentsReceived,
							Currency = d.Currency,
							IsRefunded = d.IsRefunded,
						})
						.ToList())
				.OrderByDescending(kvp => kvp.Value.Sum(d => d.ConvertedEuroCentsReceived))
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		public Dictionary<DonatorModel, List<DonationModel>> Donators { get; }
	}
}
