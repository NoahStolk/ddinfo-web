using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class DonationsModel : PageModel
	{
		public DonationsModel(ApplicationDbContext dbContext)
		{
			Donations = dbContext.Donations.ToList();

			foreach (Donation donation in Donations.Where(d => !d.IsRefunded))
			{
				int playerId = (int)donation.PlayerId; // TODO: Don't make nullable.
				if (!DonatorsWithReceivedEuroAmounts.ContainsKey(playerId))
					DonatorsWithReceivedEuroAmounts.Add(playerId, donation.ConvertedEuroCentsReceived);
				else
					DonatorsWithReceivedEuroAmounts[playerId] += donation.ConvertedEuroCentsReceived;
			}
		}

		public List<Donation> Donations { get; }

		public Dictionary<int, int> DonatorsWithReceivedEuroAmounts { get; } = new();
	}
}