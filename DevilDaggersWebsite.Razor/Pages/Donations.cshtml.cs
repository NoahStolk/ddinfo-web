using DevilDaggersWebsite.Entities;
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
			Donations = dbContext.Donations.AsNoTracking().ToList();

			foreach (Donation donation in Donations.Where(d => !d.IsRefunded && d.ConvertedEuroCentsReceived > 0))
			{
				if (!DonatorsWithReceivedEuroAmounts.ContainsKey(donation.PlayerId))
					DonatorsWithReceivedEuroAmounts.Add(donation.PlayerId, donation.ConvertedEuroCentsReceived);
				else
					DonatorsWithReceivedEuroAmounts[donation.PlayerId] += donation.ConvertedEuroCentsReceived;
			}
		}

		public List<Donation> Donations { get; }

		public Dictionary<int, int> DonatorsWithReceivedEuroAmounts { get; } = new();
	}
}
