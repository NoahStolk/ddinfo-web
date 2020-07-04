using DevilDaggersWebsite.Code.Donations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class DonationsModel : PageModel
	{
		public Dictionary<int, int> DonatorsWithReceivedEuroAmounts { get; } = new Dictionary<int, int>();

		public DonationsModel()
		{
			foreach (Donation donation in DonationList.Donations.Where(d => !d.IsRefunded))
			{
				if (!DonatorsWithReceivedEuroAmounts.ContainsKey(donation.DonatorId))
					DonatorsWithReceivedEuroAmounts.Add(donation.DonatorId, donation.ConvertedEuroCentsReceived);
				else
					DonatorsWithReceivedEuroAmounts[donation.DonatorId] += donation.ConvertedEuroCentsReceived;
			}
		}
	}
}