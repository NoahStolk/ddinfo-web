using CoreBase3.Services;
using DevilDaggersWebsite.Code.Users;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class DonationsModel : PageModel
	{
		public List<Donator> Donators { get; }
		public List<Donation> Donations { get; }

		public Dictionary<int, int> DonatorsWithReceivedEuroAmounts { get; } = new Dictionary<int, int>();

		public DonationsModel(ICommonObjects commonObjects)
		{
			Donators = UserUtils.GetUserObjects<Donator>(commonObjects);
			Donations = UserUtils.GetUserObjects<Donation>(commonObjects);

			foreach (Donation donation in Donations.Where(d => !d.IsRefunded))
			{
				if (!DonatorsWithReceivedEuroAmounts.ContainsKey(donation.DonatorId))
					DonatorsWithReceivedEuroAmounts.Add(donation.DonatorId, donation.ConvertedEuroCentsReceived);
				else
					DonatorsWithReceivedEuroAmounts[donation.DonatorId] += donation.ConvertedEuroCentsReceived;
			}
		}
	}
}